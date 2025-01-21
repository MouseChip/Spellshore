using Godot;
using System;

public partial class CatchingFish : RigidBody2D
{	
	[Export] private AttackObject _attackObject;
	[Export] private RayCast2D _leftRayCast;
	[Export] private RayCast2D _rightRayCast;
	[Export] private RigidBody2D _fishBar;

	public Timer MoveTimer;
	public float secCount = 0;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/PlayerData.tres");
	private FishData _fishData = GD.Load<FishData>("res://Data/FishData.tres");
	private RuneData _runeData = GD.Load<RuneData>("res://Data/RuneData.tres");
	private StaticBody2D _desPointBox;
	private int _fishId = 0;
	private int _fishDir = 0;
	private int _barBgWidth = 480; // Width of the fishing bar background
	private float _speed;
	private float _activity;
	private float _desPos = 0;
	private bool shouldMove = false;
	private bool _addInvFish = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fishId = _fishData.HookedFish;
		GD.Print(_fishId);
		var fish = (Godot.Collections.Dictionary)_fishData.FishDict[_fishId];
		var fishDifficulty = (float)fish["difficulty"];
		_speed = MapFloat(fishDifficulty, 0, 100, 75, 500);
		_activity = MapFloat(fishDifficulty, 0, 100, 30, 85); // There will be a 30-85% chance of the fish choosing to move

		secCount = 1;

		MoveTimer = GetNode<Timer>("MoveTimer");
		MoveTimer.WaitTime = 2;
		MoveTimer.Start();
		MoveTimer.Timeout += DecideMove;

		_desPointBox = GetNode<StaticBody2D>(GetParent().GetNode<StaticBody2D>("DesiredPoint").GetPath());

		BodyEntered += CheckCollider;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (secCount >= 10) { // If the fish was successfully caught...
			GD.Print("Fish successfully caught!");

			// Add fish to inventory
			if (_addInvFish) {
				var fish = (Godot.Collections.Dictionary)_fishData.FishDict[_fishId];
				var lenVar = (float)fish["lenVar"]; // Potential for variation in length for the fish
				var fishLength = GD.RandRange((float)fish["baseLength"]-lenVar, (float)fish["baseLength"]+lenVar);
				var invFish = new Godot.Collections.Dictionary{
					{"id", _fishId},
					{"length", fishLength}
				};
				var fishInv = (Godot.Collections.Array)_playerData.PlayerInventory["fish"];
				fishInv.Add(invFish);
				GD.Print(invFish);

				_addInvFish = false;
			}

			EndFishScene();
			
		} else if (secCount <= 0) { // If the fish got away...
			GD.Print("Fish got away!");

			EndFishScene();

		} else {
			if (!_attackObject.CanDraw) {
				if (CheckBar()) secCount += (float)delta;
				else secCount -= (float)delta;
			}
		}
	}

	public override void _PhysicsProcess(double delta) {
		if (_fishDir != 0) {
			if (Position.X != _desPos) {
				LinearVelocity += new Vector2((float)delta * _speed, 0) * _fishDir; // Move the fish towards the desired point
			}
		}
	}

	private void EndFishScene() {
		// Pause systems
		MoveTimer.Stop();
		LinearVelocity = Vector2.Zero;

		// Change scene
		GetTree().Root.GetNode("main").AddChild(ResourceLoader.Load<PackedScene>($"res://Scenes/TestingScene1.tscn").Instantiate()); // Add the fishing scene to the main node in the root scene
		GetOwner().QueueFree(); // Get the current scene and free (delete) it from the tree
		GetTree().Root.GetNode("main").MoveChild(GetTree().Root.GetNode("main").GetChild(GetTree().Root.GetNode("main").GetChildCount()-1), 0); // Get the last child added (the scene) and move it to the beginning
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

			MoveTimer.Stop();
			_desPointBox.Position = new Vector2(_desPos, _desPointBox.Position.Y); // Set the position of the desired position box
		} 
	}

	private void CheckCollider(Node body) {
		if (body == _desPointBox) { // If the fish is at its destination...
			_fishDir = 0; // Make the fish stationary
			LinearVelocity = Vector2.Zero; // Reset the timer
			
			float attackChance = GD.Randf() * 100; // Chance to determine if the fish should attack
			if (attackChance <= (float)((Godot.Collections.Dictionary)_fishData.FishDict[_fishId])["difficulty"]) {
				GD.Print("Attack!");
				string attackName = (string)((Godot.Collections.Array)((Godot.Collections.Dictionary)_fishData.FishDict[_fishId])["attacks"]).PickRandom(); // Choose a random attack from the fish's list of attacks

				var fishRunes = new Godot.Collections.Array();
				foreach (int rune in _runeData.RunesDict.Keys) { // For each rune...
					var dictRune = (Godot.Collections.Dictionary)_runeData.RunesDict[rune]; // Store that rune
					var runeAttacks = (Godot.Collections.Array)dictRune["attacks"];  // Get the attacks that the rune defends against
					if (runeAttacks.Contains(attackName) && !fishRunes.Contains(rune)) fishRunes.Add(rune); // If the rune defends against the fish attack, store it
				}
				int runeId = (int)fishRunes.PickRandom(); // Choose a random rune from the list of runes that the fish can defend against

				GD.Print($"Fish is attacking with {attackName} and can be defended against with {runeId}");

				_attackObject.AttackName = attackName; // Set the attack name of the attack object
				_attackObject.RuneId = runeId; // Set the rune id of the attack object

				_attackObject.IsAttack = true;
			} else {
				MoveTimer.Start();  // Restart the timer
			}
		}
	}

	private bool CheckBar() {
		var spaceState = GetWorld2D().DirectSpaceState; // Get a reference to the worldspace
		// Create a ray query that stretches across the fish
		// Added/subtracted 15 so that the bar has to be at least 5 pixels into the fish for the ray to hit
		// 15 was chosen because half of the boxes width is 20 pixels, 20-5 = 15
		var query = PhysicsRayQueryParameters2D.Create(new Vector2(GlobalPosition.X+15, GlobalPosition.Y), new Vector2(GlobalPosition.X-15, GlobalPosition.Y), 1024);
		query.HitFromInside = true; // Ensure that collisions are detected regardless of the bar's direction
		var results = spaceState.IntersectRay(query); // Check for collisions with the particular ray query
		if (results.Count > 0) { // If there was a result (i.e. the fish is over the bar)...
			if (results["collider"].ToString() == _fishBar.ToString()) { // If the colliding body is the fish bar...
				return true;
            }
		}

		return false;
	}

    public static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}
}
