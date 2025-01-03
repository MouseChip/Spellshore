using Godot;
using System;

public partial class SoundSlider : Node2D
{
	[Export] private HSlider _volumeSlider;
	[Export] private Button _defButton;
	[Export] private Button _muteButton;

	private float _volume;
	private float _defaultVal = 0.5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_defButton.Pressed += () => _volumeSlider.Value = _defaultVal;
		_muteButton.Pressed += () => _volumeSlider.Value = 0;
		_volumeSlider.ValueChanged += OnValChanged;

		_volume = (float)_volumeSlider.Value;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnValChanged(double value) {
		_volume = (float)value;
		GD.Print(_volume);
	}
}
