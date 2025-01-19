using Godot;
using System;

public partial class MouseCanvas : Node2D
{
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private RuneData _runeData = GD.Load<RuneData>("res://Data/RuneData.tres");
	private Godot.Collections.Array _clickPos = new Godot.Collections.Array();
	private Godot.Collections.Array _drawPos = new Godot.Collections.Array();
	private bool _isDrawing = false;

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
				if (!_isDrawing) _clickPos.Clear();
				_clickPos.Add(mouseMotion.Position); // If the mouse is being pressed, add the current position to the array
				_isDrawing = true;
				QueueRedraw(); // Queue a redraw
			}
		} else if (@event is InputEventMouseButton mouseButton) {
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsReleased()) {
				_isDrawing = false;
				DetermineRune();
			}
		}
	}

    public override void _Draw()
    {
        foreach (Vector2 point in _clickPos) {
			DrawCircle(point, 10, new Color(1, 0, 0));
		}

		foreach (Vector2 point in _drawPos) {
			DrawCircle(point, 10, new Color(0, 1, 0));
		}
    }

	private void DetermineRune() {
		GD.Print("Click positions: ", _clickPos);

		#region Get Turning Points

		var relativePos = new Godot.Collections.Array(); // Stores the position of each point relative to the last
		for (int i = 1; i < _clickPos.Count; i++) { // For each position that the mouse went over...
			relativePos.Add((Vector2)_clickPos[i] - (Vector2)_clickPos[i-1]); // Add the difference between the current and last position to the array
		}

		GD.Print("Relative positions: ", relativePos);

		var normalisedPos = new Godot.Collections.Array(); // Stores the normalised position of each relative position
		foreach (Vector2 point in relativePos) normalisedPos.Add(point.Normalized()); // For each relative position, add the normalised vector to the array

		GD.Print("Normalised positions: ", normalisedPos);

		var dotProducts = new Godot.Collections.Array(); // Stores the dot product (how similar two vectors are with regard to direction) of each normalised position with the last
		for (int i = 1; i < normalisedPos.Count; i++) dotProducts.Add(((Vector2)normalisedPos[i]).Dot((Vector2)normalisedPos[i-1])); // For each normalised position, add the dot product of the current and last normalised position to the array

		GD.Print("Dot products: ", dotProducts);

		var supTurningPoints = new Godot.Collections.Array{_clickPos[0]}; // Add the first position to the array}; // Stores the supposed turning points of the path
		for (int i = 2; i < dotProducts.Count; i++) { // For every dot product...
			// The loop starts at i = 2 because the first two dot products will 
			// always be flawed (because they are dependent of the first normalised
			// position, which will be incorrect as the first point will not have
			// the correct direction)
			var dotProduct = (float)dotProducts[i]; // Get the dot product
			var roundProd = RoundToNearest(dotProduct, 0.2f); // Round the dot product to the nearest 0.2
			var lastRoundProd = RoundToNearest((float)dotProducts[i-1], 0.2f); // Round the previous dot product to the nearest 0.2
			GD.Print(i + "-----");
			GD.Print(roundProd, lastRoundProd);

			if (roundProd != lastRoundProd) supTurningPoints.Add(_clickPos[i+1]); // If the rounded dot product is different to the previous rounded dot product, add the turning position to the array
		}
		supTurningPoints.Add(_clickPos[_clickPos.Count - 1]); // Add the last position to the array
		GD.Print(_clickPos[_clickPos.Count - 1]);

		GD.Print("Supposed turning points: ", supTurningPoints);

		#endregion

		#region Get Shape (Bounding Box)

		// Get the scale of the bounding box of the path
		Vector2 min = (Vector2)supTurningPoints[0]; // Smallest x and y values
		Vector2 max = (Vector2)supTurningPoints[0]; // Largest x and y values
		Vector2 scale; // The width and height of the bounding box
		foreach (Vector2 point in supTurningPoints) {
			min = new Vector2(Mathf.Min(min.X, point.X), Mathf.Min(min.Y, point.Y));
			max = new Vector2(Mathf.Max(max.X, point.X), Mathf.Max(max.Y, point.Y));
		}
		scale = max - min;

		// Rescale the path to fit into a 1x1 square
		var unitPoints = new Godot.Collections.Array(); // Stores the points of the path relative to a unit square
		foreach (Vector2 point in supTurningPoints) {
			var unitPoint = (point - min) / scale; // Get the point relative to the minimum value and then divide by the scale to get the point relative to (0, 0) i.e. the unit square
			unitPoints.Add(unitPoint);
		}
		
		GD.Print("Unit points: ", unitPoints);

		var debugPoints = new Godot.Collections.Array(); // MARK: DEBUGGING

		var simpUnitPoints = new Godot.Collections.Array(); // Stores the simplified unit points of the path
		float distFactor = 0.2f; // The distance between two points to determine if they are the same turning point
		for (int i = 0; i < unitPoints.Count; i++) {
			if (i == unitPoints.Count - 1) { // If the current point is the last...
				Vector2 prevPoint;
				if (simpUnitPoints.Count > 0) prevPoint = (Vector2)simpUnitPoints[simpUnitPoints.Count - 1]; // Get the previous turning point
				else prevPoint = (Vector2)unitPoints[0]; // Otherwise, get the first click position
				if ((Vector2)unitPoints[i] != prevPoint) {
					simpUnitPoints.Add(unitPoints[i]); // If the current turning point is different to the previous point, add the current point to the array
					debugPoints.Add(supTurningPoints[i]); // MARK: DEBUGGING
				}

				break;
			}

			Vector2 firstPoint = (Vector2)unitPoints[i]; // Get the first point
			Vector2 secondPoint = (Vector2)unitPoints[i+1]; // Get the second point

			float vecDist = firstPoint.DistanceTo(secondPoint);

			if (vecDist < distFactor) { // If the distance between the two points is less than the distance factor (i.e. they are the same turning point)...
				simpUnitPoints.Add(unitPoints[i+1]); // Add the second point to the array
				debugPoints.Add(supTurningPoints[i+1]); // MARK: DEBUGGING
				if (simpUnitPoints.Count >= 2) { // If there are more than two points in the array...
					if ((Vector2)simpUnitPoints[simpUnitPoints.Count - 2] == (Vector2)unitPoints[i]) {
						simpUnitPoints.RemoveAt(simpUnitPoints.Count - 2); // If the second-last point is the same as the current point (because it was added in a previous iteration), remove the repeat point
						debugPoints.RemoveAt(debugPoints.Count - 2); // MARK: DEBUGGING
					}
				}
			} else { // Otherwise, if they are part of two separate turning points...
				if (!simpUnitPoints.Contains(unitPoints[i])) {
					simpUnitPoints.Add(unitPoints[i]); // If the current turning point is not already in the array, add it
					debugPoints.Add(supTurningPoints[i]); // MARK: DEBUGGING
				}
			}
		}

		GD.Print("Simplified unit points: ", simpUnitPoints);

		float smallestDist = float.MaxValue;
		float tolerance = 1.5f; // The maximum possible distance for a path to be considered a rune
		int closestRune = 0;
		foreach (int runeId in (Godot.Collections.Array)_playerData.PlayerInventory["runes"]) {
			Godot.Collections.Array keys = (Godot.Collections.Array)_runeData.RunesDict.Keys;
			var rune = (Godot.Collections.Dictionary)_runeData.RunesDict[runeId];
			var runeTemplate = (Godot.Collections.Array)rune["template"];
			float shapeDistance = CompareShapes(simpUnitPoints, runeTemplate); // Compare the shape of the path to a test template

			if (shapeDistance < smallestDist) {
				smallestDist = shapeDistance;
				closestRune = runeId;
			}
			
			GD.Print("Shape distance: " + shapeDistance);
		}
		GD.Print("Smallest distance: " + smallestDist);

		if (smallestDist < tolerance) {
			GD.Print("Rune found: " + (string)((Godot.Collections.Dictionary)_runeData.RunesDict[closestRune])["name"]);
		} else {
			GD.Print("No rune found.");
		}

		#endregion
		
		// MARK: DEBUGGING
		_drawPos = debugPoints;
		QueueRedraw();
	}

	private float CompareShapes(Godot.Collections.Array source, Godot.Collections.Array template) {
		// Compares the shape of two paths by comparing the relative positions of the turning points
		float forShapeDist = Math.Abs(source.Count - template.Count); // The difference in the number of turning points between the two paths - the shape distance going forwards
		float backShapeDist = forShapeDist; // The shape distance going backwards
		float shortLen = source.Count < template.Count ? source.Count : template.Count; // The length of the shorter path

		for (int i = 0; i < shortLen; i++) {
			Vector2 sourcePoint = (Vector2)source[i];
			Vector2 templatePoint = (Vector2)template[i];
			Vector2 backTempPoint = (Vector2)template[template.Count - 1 - i];

			forShapeDist += sourcePoint.DistanceTo(templatePoint); // Add the distance between the two points to the shape distance
			backShapeDist += sourcePoint.DistanceTo(backTempPoint); // Add the distance between the two points to the shape distance
		}

		GD.Print(forShapeDist, " / ", backShapeDist);

		float returnDist = Mathf.Min(forShapeDist, backShapeDist); // Return the smallest shape distance

		return returnDist;
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
