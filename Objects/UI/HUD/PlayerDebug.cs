using Godot;
using System;

public partial class PlayerDebug : Control
{
	[Export] ColorPickerButton _catCol;
	[Export] ColorPickerButton _robeCol;
	[Export] OptionButton _hatOptions;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_catCol.ColorChanged += (Color colour) => _playerData.CatColour = colour;
		_robeCol.ColorChanged += (Color colour) => _playerData.RobeColour = colour;
		_hatOptions.ItemSelected += (long index) => _playerData.HatType = (string)_hatOptions.GetItemText((int)index).Replace(" ", "");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
