using Godot;
using System;

public partial class MouseCanvas : Node2D
{
	Godot.Collections.Array clickPos = new Godot.Collections.Array();
	Godot.Collections.Array drawPos = new Godot.Collections.Array();
	bool isDrawing = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseMotion) {
			if (Input.IsMouseButtonPressed(MouseButton.Left)) {
				if (!isDrawing) clickPos.Clear();
				clickPos.Add(mouseMotion.Position); // If the mouse is being pressed, add the current position to the array
				isDrawing = true;
				QueueRedraw(); // Queue a redraw
			}
		} else if (@event is InputEventMouseButton mouseButton) {
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased()) {
				isDrawing = false;
				DetermineRune();
			}
		}
	}

    public override void _Draw()
    {
        foreach (Vector2 point in clickPos) {
			DrawCircle(point, 10, new Color(1, 0, 0));
		}

		foreach (Vector2 point in drawPos) {
			DrawCircle(point, 10, new Color(0, 1, 0));
		}
    }

	private void DetermineRune() {

		GD.Print("Click pos: ", clickPos);
		var relativePos = new Godot.Collections.Array();
		for (int i = 0; i < clickPos.Count; i++) {
			if (i > 0) relativePos.Add((Vector2)clickPos[i] - (Vector2)clickPos[i-1]);
			else relativePos.Add(Vector2.Zero);
		}
		GD.Print("Relative pos: ", relativePos);

		var normalisedPos = new Godot.Collections.Array();
		foreach (Vector2 point in relativePos) {
			normalisedPos.Add(point.Normalized());
		}
		GD.Print("Normalised pos: ", normalisedPos);

		var dotPos = new Godot.Collections.Array();
		for (int i = 0; i < normalisedPos.Count; i++) {
			if (i > 1) {
				var point = (Vector2)normalisedPos[i];
				var lastPoint = (Vector2)normalisedPos[i-1];
				dotPos.Add(point.Dot(lastPoint));
			}
			else continue;
		}
		GD.Print("Dot pos: ", dotPos);

		var roundedDotPos = new Godot.Collections.Array();
		var supposedTurns = new Godot.Collections.Array();
		for (int i = 0; i < dotPos.Count; i++) {
			var point = (float)dotPos[i];
			var roundPoint = RoundToNearest(point, 0.2f);
			if (roundedDotPos.Count > 0) {
				if (roundPoint != (float)roundedDotPos[roundedDotPos.Count - 1]) {roundedDotPos.Add(roundPoint); supposedTurns.Add(clickPos[i]);}
			} else {
				roundedDotPos.Add(roundPoint);
				supposedTurns.Add(Vector2.Zero);
			}
		}
		GD.Print("Rounded dot pos: ", roundedDotPos);
		GD.Print("Supposed turn points: ", supposedTurns);
		drawPos = supposedTurns;
		QueueRedraw();

		var roundedPos = new Godot.Collections.Array();
		foreach (Vector2 point in normalisedPos) {
			var roundPoint = new Vector2(RoundToNearest(point.X, 0.5f), RoundToNearest(point.Y, 0.5f));
			if (roundedPos.Count > 0) {
				if (roundPoint != (Vector2)roundedPos[roundedPos.Count - 1]) roundedPos.Add(roundPoint);
			} else {
				roundedPos.Add(roundPoint);
			}
		}
		GD.Print("Rounded pos: ", roundedPos);
	}

	private float RoundToNearest(float num, float nearest) {
		// Rounds a provided number to its nearest multiple of another number
		// Works by taking the number as a fraction of the desired multiple, 
		// then rounding it so that the fraction becomes a whole number 
		// and is therefore a multiple of the nearest number (still as a fraction),
		// and then finally multiplying this fraction by the nearest number
		// to cancel out the fraction and get the rounded number
		return Mathf.Round(num / nearest) * nearest;
	}
}
