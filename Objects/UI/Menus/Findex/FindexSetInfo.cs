using Godot;
using System;

public partial class FindexSetInfo : HBoxContainer
{
	[Export] private PlayerData _playerData;
	[Export] private FishData _fishData;
	[Export] private RuneData _runeData;
	[Export] private TextureRect _fishPhoto;
	[Export] private RichTextLabel _pbText;
	[Export] private TextureRect _pbStar;
	[Export] private RichTextLabel _fishName;
	[Export] private RichTextLabel _fishDif;
	[Export] private RichTextLabel _fishRar;
	[Export] private RichTextLabel _fishDesc;
	[Export] private Control _runesCont;
	[Export] private PackedScene _runeObj;

	public int FishId = 1;

	private bool _isDiscovered;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetInfo() {
		if (Visible && _fishData.FishDict.ContainsKey(FishId)) {
			var fish = (Godot.Collections.Dictionary)_fishData.FishDict[FishId];
			_isDiscovered = (int)fish["caught"] > 0 ? true : false; // If the amount of this fish caught is greater than 0, then the fish has been discovered.

			if (_isDiscovered) { // If the fish has been discovered...
				_fishPhoto.Modulate = new Color(0, 1, 0); // Set the fish photo to the fish's sprite
				_pbText.Text = $"PB: {fish["pb"]} cm"; // Set the personal best length text
				if ((int)fish["pb"] < (int)fish["baseLength"]) _pbStar.Modulate = new Color(0.63f, 0.45f, 0.12f); // If the PB is below average, award a bronze star
				if ((int)fish["pb"] == (int)fish["baseLength"]) _pbStar.Modulate = new Color(0.84f, 0.84f, 0.84f); // If the PB is average, award a silver star
				if ((int)fish["pb"] > (int)fish["baseLength"]) _pbStar.Modulate = new Color(0.91f, 0.86f, 0.35f); // If the PB is above average, award a gold star
				_fishName.Text = (string)fish["name"]; // Set the fish name
				_fishDif.Text = $"Dif: {fish["difficulty"]}"; // Set the fish's difficulty
				_fishRar.Text = $"Rar: {fish["rarity"]}"; // Set the fish's rarity
				_fishDesc.Text = (string)fish["description"]; // Set the fish's description

			} else { // Otherwise, if the fish has not been discovered...
				_fishPhoto.Modulate = new Color(0, 0, 1); // Set the fish photo to the default image
				_pbText.Text = $"PB: ??? cm"; // Set the personal best length text
				_pbStar.Modulate = new Color(1, 1, 1); // Default star image

				var fishIdText = FishId.ToString();
				while (fishIdText.Length < 3) {
					fishIdText = "0" + fishIdText;
				}

				_fishName.Text = $"Fish {fishIdText}"; // Set the fish name to the fish's id
				_fishDif.Text = $"Dif: ???"; // Set the fish's difficulty
				_fishRar.Text = $"Rar: ???"; // Set the fish's rarity
				_fishDesc.Text = (string)fish["info"]; // Set the fish's description
			}

			var fishRunes = new Godot.Collections.Array();
			var fishAttacks = (Godot.Collections.Array)fish["attacks"];

			foreach (string attack in fishAttacks) { // For each attack that the fish can perform...
				foreach (int rune in _runeData.RunesDict.Keys) { // For each rune...
					var dictRune = (Godot.Collections.Dictionary)_runeData.RunesDict[rune]; // Store that rune
					var runeAttacks = (Godot.Collections.Array)dictRune["attacks"];  // Get the attacks that the rune defends against
					if (runeAttacks.Contains(attack) && !fishRunes.Contains(rune)) fishRunes.Add(rune); // If the rune defends against the fish attack, store it
				}
			}

			// Clear all current rune reprersentations
			foreach (Node child in _runesCont.GetChildren()) {
				child.Free();
			}

			foreach (string rune in fishRunes) { // For each rune...
				var newRune = _runeObj.Instantiate(); // Create a new rune object
				TextureRect runeImg = (TextureRect)newRune.GetChild(0);

				var runeInv = (Godot.Collections.Array)_playerData.PlayerInventory["runes"]; // Get the player's runes
				if (runeInv.Contains(rune)) runeImg.Modulate = new Color(0.91f, 0.86f, 0.35f); // If the player has the rune, display its image
				else runeImg.Modulate = new Color(0, 0, 0); // Otherwise, display the default image
				_runesCont.AddChild(newRune); // Add the rune object to the rune container
			}

		} else if (!_fishData.FishDict.ContainsKey(FishId)) {
			Visible = false;
		}
	}
}
