using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export] public int PlayerSpeed = 100;
	
	[Export] private Sprite2D _playerBase;
	[Export] private Sprite2D _playerRobe;
	[Export] private Sprite2D _playerHat;
	[Export] private Node _playerPaws;
	
	public int SpdMagnifier = 1;
	
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data

    public override void _Ready()
    {
        CustomiseAvatar();
		_playerData.AvatarChanged += CustomiseAvatar;
    }

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
    
	public void GetInput()
	{
		if (!_playerData.IsCasting && !_playerData.IsFishing) { // If the player isn't casting or fishing...
			Vector2 inputDir = Input.GetVector("playerLeft","playerRight","playerUp","playerDown");
			if (inputDir != Vector2.Zero) _playerData.PlayerDir = inputDir; // If there is input, log the direction
			
			if ((Input.IsActionPressed("playerSprint") && !_playerData.AutoRun) || ((!Input.IsActionPressed("playerSprint") && _playerData.AutoRun))) SpdMagnifier = 2;
			else SpdMagnifier = 1;

			Velocity = inputDir * PlayerSpeed * SpdMagnifier;
		} else {
			Velocity = Vector2.Zero;
		}
	}

	private void CustomiseAvatar() {
		_playerBase.Modulate = _playerData.CatColour;
		foreach (var child in _playerPaws.GetChildren()) {
			if (child is Sprite2D) {
				((Sprite2D)child).Modulate = _playerData.CatColour;
			}
		}
		_playerRobe.Modulate = _playerData.RobeColour;
		_playerHat.Texture = GD.Load<Texture2D>($"res://Assets/Graphics/Avatar/Clothing/Hats/{_playerData.HatType}.png");

		if (_playerData.HatType == "WizardHat") {
			_playerHat.Modulate = _playerData.RobeColour;
		} else {
			_playerHat.Modulate = new Color(1, 1, 1);
		}
	}
}
