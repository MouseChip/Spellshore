using Godot;
using System;

public partial class FindexAreaButton : Button
{
	[Export] private FindexMenu _findexMenu;
	[Export] private string _desAreaFilter;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnPress;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPress() {
		var firstFishId = (int)_findexMenu.FirstLocationFish[_desAreaFilter];
		_findexMenu.CurFishId = firstFishId;
	}
}
