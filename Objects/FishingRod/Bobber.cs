using Godot;
using System;

public partial class Bobber : RigidBody2D
{
	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private FishingRod _fishingRod;
	public Vector2 _bobberOrigin;
	private Vector2 _launchDirection = Vector2.Zero;
    private float _launchHeight = 50f; // Height of the arch at the midpoint (in pixels)
    private float _travelDuration = 0.5f; // Duration (in seconds) to reach max distance
	private float _bobberDist = 0f; // Maximum distance to travel
    private float _elapsedTime = 0f; // Time since launch

    public override void _Ready()
    {
		_fishingRod = GetParent<FishingRod>();
        _bobberOrigin = Position;
    }

    public void LaunchBobber()
    {
        _elapsedTime = 0f; // Reset the time

		_launchDirection = _playerData.playerDir.Normalized(); // Normalise the player direction to ensure that the bobber moves in the correct direction
		_bobberDist = MapFloat(_fishingRod.castLevel, -1, 1, 0, _fishingRod.maxPower); // Calculate the desired distance by mapping the cast power
    }

    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_elapsedTime < _travelDuration && _playerData.isFishing) { // If the elapsed time is less than the total travel duration...
            _elapsedTime += (float)delta; // Increase the timer

            float _distTravelled = _elapsedTime / _travelDuration; // Calculate the proportion of the journey completed
            float _verticalOffset = Mathf.Sin((float)Math.PI * _distTravelled) * _launchHeight; // Set the vertical position to a sine wave determined by the proportion of the journey completed, with an amplitude that reaches the desired peak
            
			// _bobberOrigin is the starting position and is used as a reference for the bobber's current position
			// By adding the _launchDirection, which is also a vector, the two vectors will essentially combine such that a new direction (the correct direction) is created
			// By multiplying by the scalar _bobberDistance value, the vector is extended to the full distance
			// By multiplying by the scalar _distTravelled, the vector is shortened so that the position is set to where it needs to be at the given time
			// By adding the vertical offset, the y position is increased such that it follows the path of a sine wave, creating the arch pattern
			Position = _bobberOrigin + _launchDirection * _bobberDist * _distTravelled + new Vector2(0, -_verticalOffset); // Set the position to the original position with the added distance and sine wave offset in the correct direction

        } else if (!_playerData.isFishing) {  // Otherwise, if the player is not fishing...
			Position = new Vector2(15, -15); // Set the bobber at the desired position
		}
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent) {
            if (mouseEvent.ButtonIndex == MouseButton.Left) {
                if (mouseEvent.IsReleased()) {
					if (_playerData.isCasting) {
						_playerData.isCasting = false;
						_playerData.isFishing = true;
						LaunchBobber(); // If the mouse is released, launched the bobber
					}
                }
            }
        }
    }

	public static float MapFloat(float value, float fromLow, float fromHigh, float toLow, float toHigh) {
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}
}
