using Godot;
using System;

public partial class BaitButton : Button
{
	[Export] private Label _baitTitle;

	public String BaitId;

	private ToolData _toolData = GD.Load<ToolData>("res://Data/ToolData.tres"); // Load the tool data

	public override void _Ready()
    {
		var _storedBait = (Godot.Collections.Dictionary)_toolData.BaitDict[BaitId];
		_baitTitle = GetNode<Label>("BaitName");
        _baitTitle.Text = $"{(String)_storedBait["name"]}";
    }
}
