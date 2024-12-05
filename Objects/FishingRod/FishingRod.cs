using Godot;
using System;

public partial class FishingRod : Node2D
{
	public PlayerData playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

	public float castLevel = -1;
	public float castTimer = (float)((3*Math.PI)/2); // This value will start the sine function at -1
	
	private float _castSpeed = 0.05f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (playerData.isCasting) { // If the player is casting their rod...
			GD.Print("BAL " + castLevel, " GAL " + castTimer);
			castLevel = Mathf.Sin(castTimer); // Set the level of the casting power
			castTimer += _castSpeed; // Increase the cast timer

			Visible = true;
		} else {
			castTimer = (float)((3*Math.PI)/2);
			castLevel = -1;

			Visible = false;
		}
	}

	private static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}
}
