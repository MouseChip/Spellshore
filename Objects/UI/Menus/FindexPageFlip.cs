using Godot;
using System;

public partial class FindexPageFlip : Area2D
{
	[Export] private FindexMenu _findexMenu;
	[Export] private int _incrementVal;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcessInput(true);
		InputEvent += OnInputEvent;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton) { // If the input event regards a mouse button...
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) { // If the left mouse button was clicked...
                _findexMenu.CurFishId += _incrementVal; // Decrement the fish ID by four and thus flip the page
            }
        }
    }
}
