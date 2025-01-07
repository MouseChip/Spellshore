using Godot;
using System;

public partial class FindexPageLogic : TextureRect
{
	[Export] private FindexMenu _findexMenu;
	[Export] private FindexSetInfo _fishCont1;
	[Export] private FindexSetInfo _fishCont2;
	[Export] private int _pageStartVal;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_findexMenu.PageTurn += OnPageTurn;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPageTurn() {
		// Set the ids of the two fish on the page
		_fishCont1.FishId = _findexMenu.CurFishId + _pageStartVal; // Equal to the current fish id plus the page start value (0 for left, 2 for right)
		_fishCont2.FishId = _findexMenu.CurFishId + _pageStartVal + 1; // Equal to the above fish id plus one (i.e. the next fish)
	}
}
