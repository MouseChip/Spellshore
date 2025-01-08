using Godot;
using System;

public partial class FindexMenu : Control
{
	[Signal] public delegate void PageTurnEventHandler();

	[Export] public int CurFishId {
		get => _curFishId;
		set {
			if (value > 0 && value < _fishData.FishDict.Count) { // If the value does not go beyond the bounds of the book...
				_curFishId = value; // Set the page value
				EmitSignal(SignalName.PageTurn); // Turn the page
			}
			GD.Print(_curFishId);
		}
	}
	[Export] private FishData _fishData;

	public Godot.Collections.Dictionary FirstLocationFish; // Stores the id of the first fish for each location

	private int _curFishId = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FirstLocationFish = RetrieveLocIds(); // Retrieve the ids of the first fish for each location
		VisibilityChanged += () => EmitSignal(SignalName.PageTurn);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Godot.Collections.Dictionary RetrieveLocIds() {
		Godot.Collections.Dictionary locFishIds = new Godot.Collections.Dictionary();

		var prevScene = "";

		foreach (var fishId in _fishData.FishDict) {
			var fish = (Godot.Collections.Dictionary)fishId.Value; // Get the fish information
			if ((string)fish["scene"] != prevScene) { // If the location of the fish is new...
				locFishIds[(string)fish["scene"]] = fishId.Key; // Store the fish's id
				prevScene = (string)fish["scene"];
			}
		}

		return locFishIds;
	}
}
