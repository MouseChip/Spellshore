using Godot;
using System;

public partial class MapLocation : Button
{
	[Export] private PackedScene _sceneToLoad;

	RayCast2D rayCast;

    private bool _isMouseOver = false;

    public override void _Ready()
    {
        // Connect mouse enter/exit signals
        Pressed += OnPress;
    }

    public override void _Process(double delta)
    {
    }

    private void OnPress() {
		GetTree().Root.GetNode("main").AddChild(_sceneToLoad.Instantiate()); // Add the scene to the main node in the root scene
		GetOwner().GetOwner().GetChild(0).Free(); // Get the current scene and free (delete) it from the tree
		GetTree().Root.GetNode("main").MoveChild(GetTree().Root.GetNode("main").GetChild(GetTree().Root.GetNode("main").GetChildCount()-1), 0); // Get the last child added (the scene) and move it to the beginning
	}
}
