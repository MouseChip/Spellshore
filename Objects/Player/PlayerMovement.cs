using Godot;
using System;

public partial class PlayerMovement : Node2D
{
	[Export]
	public int playerSpeed = 3;

	public Vector2 playerVel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MovePlayer(); // Move the player
	}

    private void MovePlayer() {
		Vector2 inputDir = Input.GetVector("PlayerLeft", "PlayerRight", "PlayerUp", "PlayerDown"); // Get the normalised direction of input
		if (!inputDir.IsZeroApprox()) { // If there is a direction (i.e. the player is supposed to move)...
			playerVel = inputDir * playerSpeed; // Set the velocity of the player to the direction with the provided speed
			Position += playerVel; // Add the velocity to the position of the player container
		}
	}
}
