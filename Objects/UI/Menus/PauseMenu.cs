using Godot;
using System;

public partial class PauseMenu : Node2D
{
	[Export] private Control _primaryCont;
	[Export] private Button _resBut;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_resBut.Pressed += () => _primaryCont.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
