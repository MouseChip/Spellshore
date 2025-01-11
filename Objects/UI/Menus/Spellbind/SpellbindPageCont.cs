using Godot;
using System;

public partial class SpellbindPageCont : Control
{
	[Export] private SpellbindMenu _spellbindMenu;
	[Export] private SpellbindRuneCont _lRune;
	[Export] private SpellbindRuneCont _rRune;
	[Export] private Area2D _lFlipCol;
	[Export] private Area2D _rFlipCol;

	private Godot.Collections.Array _runeConts;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_runeConts = new Godot.Collections.Array { _lRune, _rRune };

		_lFlipCol.SetProcessInput(true);
		_lFlipCol.InputEvent += (Node viewport, InputEvent @event, long shapeIdx) => OnPageTurn(viewport, @event, shapeIdx, true); // Turn the page left
		_rFlipCol.SetProcessInput(true);
		_rFlipCol.InputEvent += (Node viewport, InputEvent @event, long shapeIdx) => OnPageTurn(viewport, @event, shapeIdx, false); // Turn the page right

		_spellbindMenu.PageTurn += SetRuneConts;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetRuneConts() {
		foreach (Control runeCont in _runeConts) runeCont.Visible = false; // Hide the rune containers

		for (int i = 0; i < _runeConts.Count; i++) { // For each rune that is to be displayed on the page
			var runeContId = _spellbindMenu.CurRuneId + i;

			var runeCont = (SpellbindRuneCont)_runeConts[i]; // Get the rune container
			runeCont.RuneId = runeContId; // Set the rune id
			runeCont.Visible = true; // Display the rune container
			runeCont.SetInfo(); // Set the rune container's information
		}
	}

	private void OnPageTurn(Node viewport, InputEvent @event, long shapeIdx, bool isLeft) {
		if (@event is InputEventMouseButton mouseButton) { // If the input event regards a mouse button...
            if (mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left) { // If the left mouse button was clicked...
				
				if (!isLeft) _spellbindMenu.CurRuneId += 2; // Increment the rune ID by the number of rune displayed
				else _spellbindMenu.CurRuneId -= 2; // Decrement the rune ID by the number of rune displayed
			}
		}
	}
}
