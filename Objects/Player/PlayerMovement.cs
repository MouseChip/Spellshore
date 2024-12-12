using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export] public int PlayerSpeed = 100;
	
	public int SpdMagnifier = 1;
	
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

	public void GetInput()
	{
		if (!_playerData.IsCasting && !_playerData.IsFishing) { // If the player isn't casting or fishing...
			Vector2 inputDir = Input.GetVector("playerLeft","playerRight","playerUp","playerDown");
			if (inputDir != Vector2.Zero) _playerData.PlayerDir = inputDir; // If there is input, log the direction
			
			if (Input.IsActionPressed("playerSprint")) SpdMagnifier = 2;
			else SpdMagnifier = 1;

			Velocity = inputDir * PlayerSpeed * SpdMagnifier;
		} else {
			Velocity = Vector2.Zero;
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
