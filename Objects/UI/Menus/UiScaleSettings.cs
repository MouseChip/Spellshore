using Godot;
using System;

public partial class UiScaleSettings : Node2D
{
	[Export] private HSlider _scaleSlider;
	[Export] private BasicHud _basicHud;
	[Export] private Button _defButton;

	private float _defaultVal = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_defButton.Pressed += SetDefVal;
		_scaleSlider.DragEnded += OnScaleChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetDefVal() {
		_basicHud.ScaleMag = _defaultVal;
		_scaleSlider.Value = _defaultVal;
	}

	private void OnScaleChanged(bool valueChanged) {
		if (valueChanged) _basicHud.ScaleMag = (float)_scaleSlider.Value;
	}
}
