using Godot;
using System;

public partial class ToolButton : Button
{
	[Signal] public delegate void ToolPressedEventHandler();

	[Export] private RichTextLabel _toolTitle;

	public String ToolId;

	private ToolData _toolData = GD.Load<ToolData>("res://Data/ToolData.tres"); // Load the player data
	private Godot.Collections.Dictionary _storedTool;

    public override void _Ready()
    {
		_storedTool = (Godot.Collections.Dictionary)_toolData.ToolDict[ToolId];
		_toolTitle = GetNode<RichTextLabel>("ButtonDisplay/ToolTitle");
        _toolTitle.Text = $"[center]{(String)_storedTool["name"]}[/center]";

		Pressed += OnPress;
    }

	private void OnPress() {
		EmitSignal(SignalName.ToolPressed);
	}
}
