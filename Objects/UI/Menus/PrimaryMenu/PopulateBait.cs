using Godot;
using System;

public partial class PopulateBait : VBoxContainer
{
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private ToolData _toolData = GD.Load<ToolData>("res://Data/ToolData.tres"); // Load the tool data

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VisibilityChanged += PopulateBaitContent;
	}

	private void PopulateBaitContent() {
		if (Visible) { // If this tab is open...
			foreach (String item in (Godot.Collections.Array)_playerData.PlayerInventory["bait"]) { // For each of a specific type of item in the player's inventory...
				BaitButton baitItem = (BaitButton)GD.Load<PackedScene>("res://Objects/UI/Menus/BaitButton.tscn").Instantiate(); // Create a new rod item
				baitItem.BaitId = item;
				var tool = (Godot.Collections.Dictionary)_toolData.BaitDict[baitItem.BaitId];
				baitItem.Name = (String)tool["name"];

				// Check if an item has already been added, and don't add it again if it is already present
				foreach (var child in GetChildren()) {
					if (child.Name == baitItem.Name) return;
				}

				AddChild(baitItem);
			}
		}
	}
}
