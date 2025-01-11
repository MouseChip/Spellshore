using Godot;
using System;

public partial class SpellbindRuneCont : VBoxContainer
{
	[Export] private PlayerData _playerData;
	[Export] private FishData _fishData;
	[Export] private RuneData _runeData;
	[Export] private RichTextLabel _runeName;
	[Export] private TextureRect _runePhoto;
	[Export] private RichTextLabel _runeDesc;
	[Export] private Control _fishCont;
	[Export] private PackedScene _fishObj;

	public int RuneId = 1;

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
		if (Visible && _runeData.RunesDict.ContainsKey(RuneId)) {
			var rune = (Godot.Collections.Dictionary)_runeData.RunesDict[RuneId];
			var playerInv = (Godot.Collections.Dictionary)_playerData.PlayerInventory;
			var foundRunes = (Godot.Collections.Array)playerInv["runes"];
			_isDiscovered = foundRunes.Contains(RuneId) ? true : false; // If the player has the rune in their inventory, then the rune has been discovered

			if (_isDiscovered) { // If the rune has been discovered...
				_runeName.Text = $"[center]{(string)rune["name"]}[/center]"; // Set the rune name
				_runePhoto.Modulate = new Color(0, 1, 0); // Set the rune photo to the rune's sprite
				_runeDesc.Text = $"[center]{(string)rune["desc"]}[/center]"; // Set the rune's description
			} else { // Otherwise, if the rune has not been discovered...
				var runeIdText = RuneId.ToString();
				while (runeIdText.Length < 3) {
					runeIdText = "0" + runeIdText;
				}

				_runeName.Text = $"[center]Rune {runeIdText}[/center]"; // Set the rune name to the rune's id
				_runePhoto.Modulate = new Color(0, 0, 1); // Set the rune photo to the default image
				_runeDesc.Text = "[center]It is unknown what exactly this rune does[/center]"; // Use the rune's default description
			}

			// Find all fish that can be attacked with this rune
			var runeFish = new Godot.Collections.Array();
			foreach (string attack in (Godot.Collections.Array)rune["attacks"]) {
				foreach (var fishId in _fishData.FishDict.Keys) {
					var fish = (Godot.Collections.Dictionary)_fishData.FishDict[fishId];
					var fishAttacks = (Godot.Collections.Array)fish["attacks"];
					if (fishAttacks.Contains(attack) && !runeFish.Contains(fishId)) runeFish.Add(fishId);
				}
			}

			// Clear all current rune reprersentations
			foreach (Node child in _fishCont.GetChildren()) {
				child.Free();
			}

			foreach (var fishId in runeFish) {
				var newFish = _fishObj.Instantiate();
				TextureRect fishImg = (TextureRect)newFish.GetChild(0);

				var fish = (Godot.Collections.Dictionary)_fishData.FishDict[fishId];
				if ((int)fish["caught"] > 0) fishImg.Modulate = new Color(0.91f, 0.86f, 0.35f); // If the player has caught  the fish, display its image
				else fishImg.Modulate = new Color(0, 0, 0); // Otherwise, display the default image
				_fishCont.AddChild(newFish); // Add the fish object to the fish container
			}

		} else if (!_runeData.RunesDict.ContainsKey(RuneId)) {
			Visible = false;
		}
	}
}
