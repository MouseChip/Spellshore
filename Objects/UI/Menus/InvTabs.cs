using Godot;
using System;

public partial class InvTabs : VBoxContainer
{
	[Export] private Button _rodsButton;
	[Export] private Button _wandsButton;
	[Export] private HBoxContainer _rodsCont;
	[Export] private HBoxContainer _wandsCont;

	private Godot.Collections.Array _invMenus = new Godot.Collections.Array();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_invMenus.Add(_rodsCont);
		_invMenus.Add(_wandsCont);

		_rodsButton.Pressed += () => OnButtonPressed(0);
		_wandsButton.Pressed += () => OnButtonPressed(1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonPressed(int buttonIndex) {
		// Hide all the menus
		foreach (HBoxContainer menu in _invMenus) {
			menu.Visible = false;
		}

		switch(buttonIndex) {
			case 0:
				_rodsCont.Visible = true;
				break;
			
			case 1:
				_wandsCont.Visible = true;
				break;
		}
	}
}
