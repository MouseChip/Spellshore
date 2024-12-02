using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	public int playerSpeed = 100;
	
	public int spdMagnifier = 1;

	public void GetInput()
	{
		Vector2 inputDir=Input.GetVector("playerLeft","playerRight","playerUp","playerDown");
		
		if (Input.IsActionPressed("playerSprint")) spdMagnifier = 2;
		else spdMagnifier = 1;

        Velocity = inputDir * playerSpeed * spdMagnifier;
	}
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
