using Godot;
using System;

public partial class FindexMenu : Control
{
	[Signal] public delegate void PageTurnEventHandler();

	[Export] public int CurFishId {
		get => _curFishId;
		set {
			_curFishId = value;
			EmitSignal(SignalName.PageTurn);
			GD.Print(_curFishId);
		}
	}

	private int _curFishId = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
