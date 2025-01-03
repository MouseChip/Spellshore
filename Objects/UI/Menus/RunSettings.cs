using Godot;
using System;

public partial class RunSettings : Node2D
{
	[Export] private CheckButton _autoRunBut;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_autoRunBut.Toggled += SetAutoRun;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetAutoRun(bool toggle) {
		_playerData.AutoRun = toggle;
	}
}
