using Godot;
using System;

public partial class BasicHud : Control
{
	private float _scaleMag = 1;

	public float ScaleMag { 
		get => _scaleMag; 
		set {
			_scaleMag = value;

			Godot.Collections.Array children = (Godot.Collections.Array)GetChildren(); // Get all the children
			foreach (Control child in children) { // For each child...
				child.Scale = new Vector2(_scaleMag, _scaleMag); // Set its scale value
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
