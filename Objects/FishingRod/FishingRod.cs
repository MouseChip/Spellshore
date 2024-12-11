using Godot;
using System;

public partial class FishingRod : Node2D
{
	public String fishType;
	public String baitAttached;
	public String sourceBuff;
	public float castLevel = -1;
	public float castTimer = (float)((3*Math.PI)/2); // This value will start the sine function at -1
	public float maxPower = 32*5; // Number of pixels per tile multiplied by the number of tiles
	public float rodLuck = 50;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private Bobber _bobber;
	private Timer _hookTimer;
	private float _castSpeed = 0.05f;
	private float _throwSpeed = 15;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hookTimer = GetNode<Timer>("HookTimer"); // Get the timer for when the bobber lands in water
		_hookTimer.Timeout += OnHook;

		_bobber = GetNode<Bobber>("Bobber");
		_bobber.BobberWater += OnBobberWater;
		_bobber.ReelIn += OnReelIn;
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

	#region Fishing signal functions

	private void OnBobberWater() {
		// Start the hook timer
		_hookTimer.WaitTime = GD.RandRange(1, 30);
		_hookTimer.Start();
		GD.Print(_hookTimer.WaitTime);
	}

	private void OnReelIn() {
		// Determine if the reel is early or on time
		// Choose a fish
	}

	private void OnHook() {
		var fishData = GD.Load<FishData>("res://Data/FishData.tres");
		var sceneData = GD.Load<TestSceneData>($"res://Scenes/SceneData/{GetTree().Root.GetNode("main").GetChild(0).Name}Data.tres");
		var sceneFish = sceneData.potentialFish;
		var availableFish = new Godot.Collections.Array();

		#region Fish availability logic

		for (int i = 0; i < sceneFish.Count; i++) { // For each fish
			var fish = (Godot.Collections.Dictionary)fishData.fishDict[sceneFish[i]]; // Get the specific fish
			var fishSource = (Godot.Collections.Array)fish["source"]; // Get the fish's water sources
			
			if (fishSource.Contains("all") || fishSource.Contains(_bobber.bobberSource)) { // If the bobber was cast in a place where the fish is available...
				var fishClimates = (Godot.Collections.Array)fish["climate"]; // Get the fish's preferred climates

				if (fishClimates.Contains("all") || fishClimates.Contains(sceneData.weather)) { // If the scene's climate is suitable...
					var fishTimes = (Godot.Collections.Array)fish["hours"]; // Get the fish's active times
					var realTime = Time.GetTimeDictFromSystem(); // Get the system time
					var fishActive = false;
					
					for (int j = 0; j < fishTimes.Count; j += 2) { // For each hour that the fish will begin activity
						if ((int)realTime["hour"] >= (int)fishTimes[j] && (int)realTime["hour"] <= (int)fishTimes[j+1]) { // If the real time is within one of the active time ranges...
							fishActive = true; // The fish is active
							break;
						}
					}

					if (fishActive) { // If the fish is active...
						availableFish.Add(i); // Add the fish's id to the available fish array
					}
				}
			}
		}

		#endregion

		#region Fish determination logic
		
		// Each fish is assigned an overall rarity value
		// Start with their base rarity value
		// Add an additional 10 points if a particular bait is being used
		// For rod luck:
			// Rod's have a luck stat from 0-100
			// The higher the luck stat, the rarer the fish it affects, and the greater amount it increases their value
			// For base rarity, a luck of 0 will impact no fish, a luck of 10 will impact all fish with a rarity of 90 or lower, a luck of 20 will impact all with rarities of 80 or lower, a luck of 90 will impact all fish with rarities of 10 or lower, and a luck of 100 will impact all with rarities of 1 or lower
			// For increase value, additional points are determined by x/5, so a luck of 10 will add 2, a luck of 20 will add 4, a luck of 80 will add 16, a luck of 90 will add 18, and a luck of 100 will add 20
		// For ocean buffs:
			// All fish with a rarity below 30 will be awarded additional points

		

		#endregion
	}

	#endregion

	public static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}
}
