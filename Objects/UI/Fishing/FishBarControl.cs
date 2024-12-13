using Godot;
using System;

public partial class FishBarControl : RigidBody2D
{ 
	public float barSpeed = 500;
	public float barBuff = 0;

	private bool _isUp = false; // Should the fish bar go up?

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
		Vector2 newVel = LinearVelocity; // The new linear velocity

		if (Input.IsActionPressed("fishButton")) { // If the player is clicking (or performing the binded action)...
			_isUp = true; // The fishing bar will go up
		} else { // Otherwise...
			_isUp = false; // The fishing bar will go down
		}

		if (!_isUp) { // If the fishing bar shouldn't go up...
			newVel -= new Vector2((float)delta * GravityScale * barSpeed + barBuff, 0); // Make them go down according to the gravity scale
		} else { // Otherwise...
			newVel += new Vector2((float)delta * GravityScale * barSpeed + barBuff, 0); // Make them go up according to the gravity scale
		}
		LinearVelocity = newVel;
    }
}
