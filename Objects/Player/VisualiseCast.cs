using Godot;
using System;

public partial class VisualiseCast : ProgressBar
{
	// Called when the node enters the scene tree for the first time.
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private FishingRod _fishingRod;
	private StyleBoxFlat _progressStyle;

	public override void _Ready()
	{
		_fishingRod = GetParent().GetParent().GetNode<FishingRod>("FishingRod"); // Get the player's fishing rod object
		_progressStyle = GetThemeStylebox("fill") as StyleBoxFlat; // Get the style box for the fill of the theme
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_playerData.isCasting) { // If the player is casting the rod...
			Visible = true; // Display the progress bar

			Value = _fishingRod.castLevel; // Set the value of the bar to the cast level
			_progressStyle.BgColor = Colors.Orange.Lerp(Colors.Green, (float)(Value / MaxValue)); // Make the colour of the bar dependent on the value
		} else {
			Visible = false;
		}
	}
}
