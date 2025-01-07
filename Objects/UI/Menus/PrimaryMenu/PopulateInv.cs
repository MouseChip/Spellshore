using Godot;
using System;

public partial class PopulateInv : HBoxContainer
{
	[Signal] public delegate void InvPopulatedEventHandler();

	[Export] private HBoxContainer _itemCont;
	[Export] private String _invType;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private ToolData _toolData = GD.Load<ToolData>("res://Data/ToolData.tres"); // Load the tool data

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VisibilityChanged += PopulateInvContent;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void PopulateInvContent() {
		if (Visible) { // If this tab is open...
			foreach (String item in (Godot.Collections.Array)_playerData.PlayerInventory[_invType]) { // For each of a specific type of item in the player's inventory...
				ToolButton rodItem = (ToolButton)GD.Load<PackedScene>("res://Objects/UI/Menus/ToolButton.tscn").Instantiate(); // Create a new rod item
				rodItem.ToolId = item;
				var tool = (Godot.Collections.Dictionary)_toolData.ToolDict[rodItem.ToolId];
				rodItem.Name = (String)tool["name"];

				// Check if an item has already been added, and don't add it again if it is already present
				foreach (var child in _itemCont.GetChildren()) {
					if (child.Name == rodItem.Name) return;
				}

				_itemCont.AddChild(rodItem);
			}

			EmitSignal(SignalName.InvPopulated);
		}
	}
}
