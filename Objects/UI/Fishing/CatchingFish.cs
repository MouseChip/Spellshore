using Godot;
using System;

public partial class CatchingFish : RigidBody2D
{	
	[Export] private RayCast2D _leftRayCast;
	[Export] private RayCast2D _rightRayCast;

	private FishData _fishData = GD.Load<FishData>("res://Data/FishData.tres");
	private Timer _moveTimer;
	private StaticBody2D _desPointBox;
	private int _fishId = 0;
	private int _fishDir = 0;
	private int _barBgWidth = 480; // Width of the fishing bar background
	private float _speed;
	private float _activity;
	private float _desPos = 0;
	private bool shouldMove = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fishId = GetNode<FishLogic>(GetOwner().GetNode("Fish").GetPath()).fishId;
		var fish = (Godot.Collections.Dictionary)_fishData.FishDict[_fishId];
		var fishDifficulty = (float)fish["difficulty"];
		_speed = MapFloat(fishDifficulty, 0, 100, 75, 500);
		_activity = MapFloat(fishDifficulty, 0, 100, 30, 85); // There will be a 30-85% chance of the fish choosing to move

		_moveTimer = GetNode<Timer>("MoveTimer");
		_moveTimer.WaitTime = 1;
		_moveTimer.Start();
		_moveTimer.Timeout += DecideMove;

		_desPointBox = GetNode<StaticBody2D>(GetParent().GetNode<StaticBody2D>("DesiredPoint").GetPath());

		BodyEntered += CheckCollider;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta) {
		if (_fishDir != 0) {
			if (Position.X != _desPos) {
				LinearVelocity += new Vector2((float)delta * _speed, 0) * _fishDir; // Move the fish towards the desired point
			}
		}
	}

	private void DecideMove() {
		var moveChance = GD.Randf() * 100; // Chance to determine if the fish should move
		if (moveChance < _activity) { // If the chance of moving aligns with the activity chance (i.e. the fish should move)...
			float leftDist = 0;
			float rightDist = 0;

			if (_leftRayCast.IsColliding()) { // If the left raycast is colliding (it can only collide with the bar borders)...
				var colPoint = _leftRayCast.GetCollisionPoint(); // Get the point of collision
				var colDist = _leftRayCast.GlobalTransform.Origin.DistanceTo(colPoint); // Get the distance to the point of collision
				leftDist = -colDist; // Set the maximum possible left distance (negative so that the fish will travel left)
			}
			if (_rightRayCast.IsColliding()) { // If the right raycast is colliding (it can only collide with the bar borders)...
				var colPoint = _rightRayCast.GetCollisionPoint(); // Get the point of collision
				var colDist = _rightRayCast.GlobalTransform.Origin.DistanceTo(colPoint); // Get the distance to the point of collision
				rightDist = colDist; // Set the maximum possible right distance
			}

			var pointDir = (float)GD.RandRange(leftDist, rightDist); // Pick a random direction (either left or right)
			var desCol = _desPointBox.GetNode<CollisionShape2D>("DesPointShape").Shape as RectangleShape2D; // Get the rectangle collision shape of the desired point box
			if (pointDir <= 0) { // If left...
				_desPos = (float)GD.RandRange(_leftRayCast.GetCollisionPoint().X + desCol.Size.X, _leftRayCast.GlobalTransform.Origin.X - desCol.Size.X); // Pick a random position on the left side of the fish (ensure that enough space is left for the desired point collision shape itself)
				_fishDir = -1;
			} else if (pointDir > 0) { // If right...
				_desPos = (float)GD.RandRange(_rightRayCast.GlobalTransform.Origin.X - desCol.Size.X, _rightRayCast.GetCollisionPoint().X + desCol.Size.X); // Pick a random position on the right side of the fish (ensure that enough space is left for the desired point collision shape itself)
				_fishDir = 1;
			}

			_moveTimer.Stop();
			_desPointBox.Position = new Vector2(_desPos, _desPointBox.Position.Y); // Set the position of the desired position box
		} 
	}

	private void CheckCollider(Node body) {
		if (body == _desPointBox) { // If the fish is at its destination...
			_moveTimer.Start();  // Restart the timer
			_fishDir = 0; // Make the fish stationary
			LinearVelocity = Vector2.Zero; // Reset the timer
		}
	}

	public static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}
}
