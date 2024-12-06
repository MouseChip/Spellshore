using Godot;
using System;

public partial class FishingRod : Node2D
{
	public float castLevel = -1;
	public float castTimer = (float)((3*Math.PI)/2); // This value will start the sine function at -1
	public float maxPower = 32*5; // Number of pixels per tile multiplied by the number of tiles

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private Bobber _bobber;
	private float _castSpeed = 0.05f;
	private float _throwSpeed = 15;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bobber = GetNode<Bobber>("Bobber");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_playerData.isCasting) { // If the player is casting their rod...
			castLevel = Mathf.Sin(castTimer); // Set the level of the casting power
			castTimer += _castSpeed; // Increase the cast timer
		} else { // Otherwise...
			castTimer = (float)((3*Math.PI)/2); // Reset the cast power
			castLevel = -1;
		}
	}
}
