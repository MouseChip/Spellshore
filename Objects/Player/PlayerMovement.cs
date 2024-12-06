using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export] public int playerSpeed = 100;
	
	public int spdMagnifier = 1;
	
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

	public void GetInput()
	{
		if (!_playerData.isCasting && !_playerData.isFishing) { // If the player isn't casting or fishing...
			Vector2 inputDir = Input.GetVector("playerLeft","playerRight","playerUp","playerDown");
			if (inputDir != Vector2.Zero) _playerData.playerDir = inputDir; // If there is input, log the direction
			
			if (Input.IsActionPressed("playerSprint")) spdMagnifier = 2;
			else spdMagnifier = 1;

			Velocity = inputDir * playerSpeed * spdMagnifier;
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
