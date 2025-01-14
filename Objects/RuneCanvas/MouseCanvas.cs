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

		#region Real

		GD.Print("Click positions: ", clickPos);

		var relativePos = new Godot.Collections.Array(); // Stores the position of each point relative to the last
		for (int i = 1; i < clickPos.Count; i++) { // For each position that the mouse went over...
			relativePos.Add((Vector2)clickPos[i] - (Vector2)clickPos[i-1]); // Add the difference between the current and last position to the array
		}

		GD.Print("Relative positions: ", relativePos);

		var normalisedPos = new Godot.Collections.Array(); // Stores the normalised position of each relative position
		foreach (Vector2 point in relativePos) normalisedPos.Add(point.Normalized()); // For each relative position, add the normalised vector to the array

		GD.Print("Normalised positions: ", normalisedPos);

		var dotProducts = new Godot.Collections.Array(); // Stores the dot product (how similar two vectors are with regard to direction) of each normalised position with the last
		for (int i = 1; i < normalisedPos.Count; i++) dotProducts.Add(((Vector2)normalisedPos[i]).Dot((Vector2)normalisedPos[i-1])); // For each normalised position, add the dot product of the current and last normalised position to the array

		GD.Print("Dot products: ", dotProducts);

		var supTurningPoints = new Godot.Collections.Array(); // Stores the supposed turning points of the path
		for (int i = 1; i < dotProducts.Count; i++) { // For every dot product...
			var dotProduct = (float)dotProducts[i]; // Get the dot product
			var roundProd = RoundToNearest(dotProduct, 0.2f); // Round the dot product to the nearest 0.2
			var lastRoundProd = RoundToNearest((float)dotProducts[i-1], 0.2f); // Round the next dot product to the nearest 0.2

			if (roundProd != lastRoundProd) supTurningPoints.Add(clickPos[i+1]); // If the rounded dot product is different to the previous rounded dot product, add the turning position to the array
		}

		GD.Print("Supposed turning points: ", supTurningPoints);

		var normalisedSupPos = new Godot.Collections.Array(); // Stores the normalised position of each supposed turning point
		foreach (Vector2 point in supTurningPoints) { // For each supposed turning point...
			var pointX = point.X / GetViewportRect().Size.X; // Get the x-coordinate of the point as a fraction of the viewport width
			var pointY = point.Y / GetViewportRect().Size.Y; // Get the x-coordinate of the point as a fraction of the viewport width
			normalisedSupPos.Add(new Vector2(pointX, pointY)); // Add the normalised vector to the array
		}

		GD.Print("Normalised supposed turning points: ", normalisedSupPos);

		var simpTurningPoints = new Godot.Collections.Array(); // Stores the simplified turning points of the path
		float distFactor = 0.035f; // The distance between two points to determine if they are the same turning point
		for (int i = 0; i < normalisedSupPos.Count; i++) {
			if (i == normalisedSupPos.Count - 1) { // If the current point is the last...
				var prevPoint = (Vector2)simpTurningPoints[simpTurningPoints.Count - 1]; // Get the last turning point
				if ((Vector2)supTurningPoints[i] != prevPoint) simpTurningPoints.Add(supTurningPoints[i]); // If the current turning point is different to the previous point, add the current point to the array

				break;
			}

			Vector2 firstPoint = (Vector2)normalisedSupPos[i]; // Get the first point
			Vector2 secondPoint = (Vector2)normalisedSupPos[i+1]; // Get the second point

			if (firstPoint.DistanceTo(secondPoint) < distFactor) { // If the distance between the two points is less than the distance factor (i.e. they are the same turning point)...
				simpTurningPoints.Add(supTurningPoints[i+1]); // Add the second point to the array
				if (simpTurningPoints.Count >= 2) { // If there are more than two points in the array...
					if ((Vector2)simpTurningPoints[simpTurningPoints.Count - 2] == (Vector2)supTurningPoints[i]) simpTurningPoints.RemoveAt(simpTurningPoints.Count - 2); // If the second-last point is the same as the current point (because it was added in a previous iteration), remove the repeat point
				}
			} else { // Otherwise, if they are part of two separate turning points...
				if (!simpTurningPoints.Contains(supTurningPoints[i])) simpTurningPoints.Add(supTurningPoints[i]); // If the current turning point is not already in the array, add it
			}
		}
		var lastPos = (Vector2)clickPos[clickPos.Count - 1];
		var lastTurnPos = (Vector2)simpTurningPoints[simpTurningPoints.Count - 1];
		simpTurningPoints.Add(lastPos); // Add the last position to the array
		if (lastPos.DistanceTo(lastTurnPos) < distFactor) simpTurningPoints.RemoveAt(simpTurningPoints.Count - 1); // If the last position is the same as the last turning point, remove the last turning point

		GD.Print("Simplified turning points: ", simpTurningPoints);
		
		// MARK: DEBUGGING
		drawPos = simpTurningPoints;
		QueueRedraw();

		for (int i = 0; i < simpTurningPoints.Count; i++) {
			var curPos = (Vector2)simpTurningPoints[i]; // The current position
			Vector2 prevPos; // The previous position
			if (i == 0) prevPos = (Vector2)clickPos[0]; // If the current position is the first, the previous position is the first click position
			else prevPos = (Vector2)simpTurningPoints[i-1]; // Otherwise, the previous position is the previous turning point

			var turnDir = (curPos - prevPos).Normalized(); // Get the direction of the turn by normalising the difference between the two points
			turnDir = new Vector2(RoundToNearest(turnDir.X, 0.5f), RoundToNearest(turnDir.Y, 0.5f)); // Round the direction to the nearest 0.25
			GD.Print("Turn direction: ", turnDir);
		}

		#endregion

		#region Testing

		/* GD.Print("Click pos: ", clickPos);
		var relativePos = new Godot.Collections.Array();
		for (int i = 0; i < clickPos.Count; i++) {
			if (i > 0) relativePos.Add((Vector2)clickPos[i] - (Vector2)clickPos[i-1]);
			else relativePos.Add(Vector2.Zero);
		}
		GD.Print("Relative pos: ", relativePos); */

		/* var normalisedPos = new Godot.Collections.Array();
		foreach (Vector2 point in relativePos) {
			normalisedPos.Add(point.Normalized());
		}
		GD.Print("Normalised pos: ", normalisedPos); */

		/* var dotPos = new Godot.Collections.Array();
		for (int i = 0; i < normalisedPos.Count; i++) {
			if (i > 1) {
				var point = (Vector2)normalisedPos[i];
				var lastPoint = (Vector2)normalisedPos[i-1];
				dotPos.Add(point.Dot(lastPoint));
			}
			else continue;
		}
		GD.Print("Dot pos: ", dotPos); */

		/* var roundedDotPos = new Godot.Collections.Array();
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
		GD.Print("Supposed turn points: ", supposedTurns); */

		/* var normalisedSupPos = new Godot.Collections.Array();
		foreach (Vector2 point in supposedTurns) {
			normalisedSupPos.Add(point.Normalized());
		}
		GD.Print("Normalised supposed pos: ", normalisedSupPos); */

		/* var simplifiedSupPos = new Godot.Collections.Array();
		for (int i = 0; i < normalisedSupPos.Count; i++) {
			if (i == normalisedSupPos.Count - 1) {
				var lastSimplePoint = (Vector2)simplifiedSupPos[simplifiedSupPos.Count - 1];
				var lastSupposedPoint = (Vector2)supposedTurns[supposedTurns.Count - 1];

				if (lastSimplePoint != lastSupposedPoint) simplifiedSupPos.Add(lastSupposedPoint);
				break;
			}

			Vector2 firstPoint = (Vector2)normalisedSupPos[i];
			Vector2 secondPoint = (Vector2)normalisedSupPos[i+1];

			if (firstPoint.DistanceTo(secondPoint) < 0.05f) {
				simplifiedSupPos.Add(supposedTurns[i+1]);
				if (simplifiedSupPos.Count >= 2) {
					if ((Vector2)simplifiedSupPos[simplifiedSupPos.Count - 2] == (Vector2)supposedTurns[i]) simplifiedSupPos.RemoveAt(simplifiedSupPos.Count - 2);
				}
			}
		}
		if ((Vector2)clickPos[clickPos.Count - 1] != (Vector2)simplifiedSupPos[simplifiedSupPos.Count - 1]) simplifiedSupPos.Add(clickPos[clickPos.Count - 1]);
		GD.Print("Simplified supposed pos: ", simplifiedSupPos);
		drawPos = simplifiedSupPos;
		QueueRedraw(); */

		/* for (int i = 0; i < simplifiedSupPos.Count; i++) {
			Vector2 lastPos;
			if (i == 0) lastPos = (Vector2)clickPos[0];
			else lastPos = (Vector2)simplifiedSupPos[i-1];
			var supPos = (Vector2)simplifiedSupPos[i];
			var supRelPos = (Vector2)relativePos[clickPos.IndexOf(simplifiedSupPos[i])];
			GD.Print(supPos + " / " + supRelPos + " / " + supRelPos.Normalized()   + " / " + supRelPos.Angle() + " / " + supPos.Angle() + " / " + supPos.AngleTo(lastPos));

			var relSup = supPos - lastPos;
			GD.Print(relSup.Normalized());
		} */

		/* var roundedPos = new Godot.Collections.Array();
		foreach (Vector2 point in normalisedPos) {
			var roundPoint = new Vector2(RoundToNearest(point.X, 0.5f), RoundToNearest(point.Y, 0.5f));
			if (roundedPos.Count > 0) {
				if (roundPoint != (Vector2)roundedPos[roundedPos.Count - 1]) roundedPos.Add(roundPoint);
			} else {
				roundedPos.Add(roundPoint);
			}
		}
		GD.Print("Rounded pos: ", roundedPos); */

		#endregion
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
