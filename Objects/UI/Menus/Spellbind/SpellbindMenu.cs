using Godot;
using System;

public partial class SpellbindMenu : Control
{
	[Signal] public delegate void PageTurnEventHandler();

	[Export] private RuneData _runeData;

	public int CurRuneId {
		get => _curRuneId;
		set {
			if (value > 0 && value <= _runeData.RunesDict.Count) { // If the value does not go beyond the bounds of the book...
				_curRuneId = value; // Set the page value
				EmitSignal(SignalName.PageTurn); // Turn the page
			}
		}
	}

	private int _curRuneId = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VisibilityChanged += MenuOpened; // When the menu is opened, emit the page turn signal
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void MenuOpened() {
		if (Visible) EmitSignal(SignalName.PageTurn);
	}
}
