using Godot;
using System;

public partial class FindexPageCont : Control
{
	[Export] private FindexMenu _findexMenu;
	[Export] private FindexSetInfo _lFishCont1;
	[Export] private FindexSetInfo _lFishCont2;
	[Export] private FindexSetInfo _rFishCont1;
	[Export] private FindexSetInfo _rFishCont2;
	[Export] private Area2D _lFlipCol;
	[Export] private Area2D _rFlipCol;

	private Godot.Collections.Array _fishConts;
	private int _fishDisplayed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fishConts = new Godot.Collections.Array { _lFishCont1, _lFishCont2, _rFishCont1, _rFishCont2 };

		_lFlipCol.SetProcessInput(true);
		_lFlipCol.InputEvent += (Node viewport, InputEvent @event, long shapeIdx) => OnPageTurn(viewport, @event, shapeIdx, true); // Turn the page left
		_rFlipCol.SetProcessInput(true);
		_rFlipCol.InputEvent += (Node viewport, InputEvent @event, long shapeIdx) => OnPageTurn(viewport, @event, shapeIdx, false); // Turn the page right

		_findexMenu.PageTurn += SetFishConts;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetFishConts() {
		foreach (Control fishCont in _fishConts) fishCont.Visible = false; // Hide the fish containers
		_fishDisplayed = 0;

		for (int i = 0; i < _fishConts.Count; i++) { // For each fish that is to be displayed on the page
			var fishContId = _findexMenu.CurFishId + i;

			if (i > 0) { // If this is not the very first fish...
				// Check if the fish container should be displayed for this location
				foreach (var locKeyVal in _findexMenu.FirstLocationFish) { // For each fish in the dictionary of first fish for each location...
					if ((int)locKeyVal.Value == fishContId) { // If the fish id matches the current fish id...
						goto LoopExit; // Skip the fish container and all those after it
					}
				}
			}

			var fishCont = (FindexSetInfo)_fishConts[i]; // Get the fish container
			fishCont.FishId = fishContId; // Set the fish id
			fishCont.Visible = true; // Display the fish container
			fishCont.SetInfo(); // Set the fish container's information
			_fishDisplayed++;
		}

		LoopExit:
			return;
	}

	private void OnPageTurn(Node viewport, InputEvent @event, long shapeIdx, bool isLeft) {
		if (@event is InputEventMouseButton mouseButton) { // If the input event regards a mouse button...
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) { // If the left mouse button was clicked...
				
				if (!isLeft) _findexMenu.CurFishId += _fishDisplayed; // Increment the fish ID by the number of fish displayed
				else {
					var fishLocIds = (Godot.Collections.Array)_findexMenu.FirstLocationFish.Values; // Get all of the first location fish values
					if (!fishLocIds.Contains(_findexMenu.CurFishId)) { // If the current fish id is not a location transition...
						_findexMenu.CurFishId -= _fishConts.Count; // Decrement the fish by the number of fish containers (4)
					} else { // Otherwise...
						var valInd = fishLocIds.IndexOf(_findexMenu.CurFishId); // Get the index of the current fish id
						int prevPageCount = 0;
						if (valInd > 0) { // If this is not the first location transition...
							// Get the number of fish containers on the previous page 
							// by finding the number of fish that in between the two location transitions 
							// and then dividing by four (the number of fish per page) to get the number of pages,
							// with the remainder being representative of the number of fish on the previous page
							prevPageCount = (_findexMenu.CurFishId - (int)fishLocIds[valInd - 1]) % _fishConts.Count;

							if (prevPageCount == 0) prevPageCount = 4; // If the result was 0, then the number of fish on the previous page is 4
						}

						_findexMenu.CurFishId -= prevPageCount;
					}
				}
			}
		}
	}
}
