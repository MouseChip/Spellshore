using Godot;
using System;

public partial class BookMenuButton : Button
{
	[Export] private Control _menuCont;
	[Export] private String _inputName;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnClick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_inputName)) {
			OnClick();
		}
	}

	private void OnClick() {
		if (_menuCont.Visible) _menuCont.Visible = false; // If the container is already open, close the menu
		else _menuCont.Visible = true; // Otherwise, open the menu
	}
}
