using Godot;
using System;

public partial class FishingRod : Node2D
{
	public String FishType;
	public String BaitAttached;
	public String SourceBuff;
	public int HookedFish = 0;
	public float CastLevel = -1;
	public float CastTimer = (float)((3*Math.PI)/2); // This value will start the sine function at -1
	public float MaxPower = 32*5; // Number of pixels per tile multiplied by the number of tiles
	public float RodLuck = 50;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private FishData _fishData = GD.Load<FishData>("res://Data/FishData.tres");
	private Bobber _bobber;
	private Timer _hookTimer;
	private Timer _reelTimer;
	private float _castSpeed = 0.05f;
	private float _throwSpeed = 15;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hookTimer = GetNode<Timer>("HookTimer"); // Get the timer for when the bobber lands in water
		_hookTimer.Timeout += OnHook;
		_reelTimer = GetNode<Timer>("ReelTimer"); // Get the timer for player reaction time when they've hooked a fish
		_reelTimer.Timeout += OnReelTimeout;

		_bobber = GetNode<Bobber>("Bobber");
		_bobber.BobberWater += OnBobberWater;
		_bobber.ReelIn += OnReelIn;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_playerData.IsCasting) { // If the player is casting their rod...
			CastLevel = Mathf.Sin(CastTimer); // Set the level of the casting power
			CastTimer += _castSpeed; // Increase the cast timer
		} else { // Otherwise...
			CastTimer = (float)((3*Math.PI)/2); // Reset the cast power
			CastLevel = -1;
		}

		if (_playerData.IsCasting || _playerData.IsFishing) Visible = true; // If the rod is being used, make it visible...
		else Visible = false; // Otherwise, hide it
	}

	#region Fishing signal functions

	private void OnBobberWater() {
		// Start the hook timer
		_hookTimer.WaitTime = GD.RandRange(1, 30);
		_hookTimer.Start();
		GD.Print(_hookTimer.WaitTime);
	}

	private void OnReelIn() {
		if (_hookTimer.TimeLeft == 0 && _reelTimer.TimeLeft > 0) { // If there is a fish on the hook and it was reeled in on time...
			var fish = (Godot.Collections.Dictionary)_fishData.FishDict[HookedFish];
			_fishData.HookedFish = HookedFish;

			GetTree().Root.GetNode("main").AddChild(ResourceLoader.Load<PackedScene>($"res://Scenes/TestingScene2.tscn").Instantiate()); // Add the fishing scene to the main node in the root scene
			GetOwner().GetOwner().QueueFree(); // Get the current scene by first getting the fishing rod's owner (the player) and then the player's owner (the scene)
		}

		_hookTimer.Stop();
		_reelTimer.Stop();
	}

	private void OnReelTimeout() {
		bool inWater = _bobber.CheckWater(); // Check if the bobber is in water

		if (inWater) OnBobberWater(); // If the player didn't reel in their rod, start the timer for the next fish
	}

	private void OnHook() {
		HookedFish = 0; // Reset the fish on the hook
		
		var sceneData = GD.Load<TestSceneData>($"res://Scenes/SceneData/{GetTree().Root.GetNode("main").GetChild(0).Name}Data.tres");
		var sceneFish = sceneData.PotentialFish;
		var availableFish = new Godot.Collections.Array();

		#region Fish availability logic

		for (int i = 0; i < sceneFish.Count; i++) { // For each fish...
			var fish = (Godot.Collections.Dictionary)_fishData.FishDict[sceneFish[i]]; // Get the specific fish
			var fishSource = (Godot.Collections.Array)fish["source"]; // Get the fish's water sources
			
			if (fishSource.Contains("all") || fishSource.Contains(_bobber.BobberSource)) { // If the bobber was cast in a place where the fish is available...
				var fishClimates = (Godot.Collections.Array)fish["climate"]; // Get the fish's preferred climates

				if (fishClimates.Contains("all") || fishClimates.Contains(sceneData.Weather)) { // If the scene's climate is suitable...
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

		var fishChances = new Godot.Collections.Array();
		var totalChance = 0f;

		// Get the basic chances for each fish to be found
		for (int i = 0; i < availableFish.Count; i++) { // For every available fish...
			var fish = (Godot.Collections.Dictionary)_fishData.FishDict[availableFish[i]]; // Get the specific fish
			var fishRarity = (float)fish["rarity"]; // The base rarity value of the fish
			var fishChance = fishRarity; // The total chance of the fish being caught
			var fishBait = (Godot.Collections.Array)fish["bait"]; // The bait the fish is attracted to

			if (BaitAttached != null && (fishBait.Contains("all") || fishBait.Contains(BaitAttached))) { // If there is bait attached to the rod and the fish is affected by bait...
				fishChance += 10; // Increase the fish's chance of being found
			}

			if (RodLuck != 0) { // If the rod has a luck stat...
				if (fishRarity >= Math.Abs(RodLuck)) { // If the fish is equal to or greater than the luck stat...
					var addChance = RodLuck/5; // Add additional points, a fifth of the rod's luck stat
					fishChance += addChance;
				}
			}

			if (SourceBuff == "luck") { // If the water source has a luck buff applied...
				if (fishRarity >= 70) { // If the fish is rarer than the chosen value...
					fishChance += 5; // Add five points
				}
			}

			fishChance = 100 - fishChance; // Invert the fish chance (so that fish's with higher rarity have lower chance of being found)
			fishChances.Add(fishChance);
			totalChance += fishChance;
		}

		for (int i = 0; i < fishChances.Count; i++) { // For each fish's chance...
			var mappedChance = MapFloat((float)fishChances[i], 0, totalChance, 0, 100); // Map the chance to a percentage value
			fishChances[i] = mappedChance;
			continue;
		}

		// Weighted fish probability (regardless of order)
		var fishChoice = GD.Randf() * 100; // Get a random floating point value between 0 and 100
		for (int i = 0; i < fishChances.Count; i++) { // For each fish's chance...
			fishChoice -= (float)fishChances[i]; // Subtract it from the fish choice value
			if (fishChoice > 0) continue; // If that value is positive, then continue on to the next fish

			HookedFish = (int)availableFish[i]; // Otherwise, set the hooked fish
			var fish = (Godot.Collections.Dictionary)_fishData.FishDict[HookedFish];
			GD.Print(fish);
			break; // Return
		}

		#endregion

		GD.Print("FISH ON HOOK!");

		// Start the timer to see if the player reels the fish in on time
		_hookTimer.Stop();
		_reelTimer.WaitTime = 1.5;
		_reelTimer.Start();
	}

	#endregion

	public static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}

	// [3, 41, 15, 19, 22]
	// 16
}
