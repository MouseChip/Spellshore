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

	private int _curFishId = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VisibilityChanged += () => EmitSignal(SignalName.PageTurn);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
