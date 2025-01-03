using Godot;
using System;

public partial class WindowSettings : Node2D
{
	[Export] private OptionButton _windowModeBut;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_windowModeBut.ItemSelected += SetWinMode;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetWinMode(long itemInd) {
		if (itemInd == 0) { // If the selected item is fullscreen...
    		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen); // Set the window to fullscreen
		} else if (itemInd == 1) { // Otherwise, if the selected item is windowed...
   			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed); // Set the window to windowed mode
		}
	}
}
