using Godot;
using System;

public partial class ToolDetails : HBoxContainer
{
	[Export] private HBoxContainer _invItems;
	[Export] private RichTextLabel _toolInfo;
	[Export] private HBoxContainer _toolStats;
	[Export] private PopulateInv _invParent;

	private ToolData _toolData = GD.Load<ToolData>("res://Data/toolData.tres"); // Load the player data

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_invParent.InvPopulated += GetInvButs;
	}

	private void SetItemDesc(String ToolId) {
		var _storedTool = (Godot.Collections.Dictionary)_toolData.ToolDict[ToolId]; // Get the stored tool
		_toolInfo.Text = (String)_storedTool["desc"]; // Set the description text

		foreach (RichTextLabel stat in _toolStats.GetChildren()) { // For each stat...
			stat.Text = $"{stat.Name}: {_storedTool[stat.Name.ToString().ToLower()]}"; // Set the stat value
		}
	}

	private void GetInvButs() {
		foreach (ToolButton item in _invItems.GetChildren()) { // For each tool in the inventory...
			item.ToolPressed += () => SetItemDesc(item.ToolId); // Set the tool description when clicked
		}
	}
}
