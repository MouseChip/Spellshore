using Godot;
using System;

public partial class PrimaryMenuButton : Button
{
	[Export] private Control _primaryContainer;
	[Export] private TabContainer _tabContainer;
	[Export] private String _inputName;
	[Export] private int _tabIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnClick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_inputName != "closeMenu") { // If this is not the close button...
			if (Input.IsActionJustPressed(_inputName)) {
				ToggleMenus();
			}
		}
	}

	private void OnClick() {
		if (_primaryContainer.Visible) { // If the primary container is already open...
			if (_inputName == "closeMenu") { // If the close button was clicked...
				_primaryContainer.Visible = false; // Close the menu
				return; // Ignore the button press
			} 
		}
		ToggleMenus();
	}

	private void ToggleMenus() {
		if (_primaryContainer.Visible) { // If the primary container is already open...
			if (_tabContainer.CurrentTab == _tabIndex) _primaryContainer.Visible = false; // If the tab opened was the one already opened, close the menu
			else _tabContainer.CurrentTab = _tabIndex; // Otherwise, set the tab
		} else { // Otherwise...
			_primaryContainer.Visible = true;
			_tabContainer.CurrentTab = _tabIndex;
		}
	}
}
