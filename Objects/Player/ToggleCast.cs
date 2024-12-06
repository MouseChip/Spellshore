using Godot;
using System;

public partial class ToggleCast : TileMapLayer
{
	public delegate void EndCastEventHandler();

	public PlayerData playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton) {
			if (mouseButton.ButtonIndex == MouseButton.Left)  {
				if (mouseButton.IsPressed()) {
					#region Raycast for water collision

					var spaceState = GetWorld2D().DirectSpaceState; // Get a reference to the worldspace
					var query = new PhysicsPointQueryParameters2D(); // Create a new raycast query that is to be set to a single point
					query.Position = GetGlobalMousePosition(); // Set the position of the query to the global mouse position
					var results = spaceState.IntersectPoint(query); // Perform the raycast query and store any collisions in the form of an array of dictionaries (representing each collision object)

					if (results.Count > 0) { // If there was at least one collision...
						for (int i = 0; i < results.Count; i++) { // For each colliding body...
							if (results[i]["collider"].ToString() != GetNode(GetPath()).ToString()) { // If that colliding body is not this one (i.e. it is not the water layer)...
								return; // Return so that none of the fishing occurs
							}
							playerData.isCasting = true; // Set the casting state
						}
					}

					#endregion
				}
			}
		}
    }
}
