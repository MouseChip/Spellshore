using Godot;
using System;

public partial class Bobber : RigidBody2D
{
    [Signal] public delegate void BobberWaterEventHandler();
    [Signal] public delegate void ReelInEventHandler();

    public String bobberSource;

	private PlayerData _playerData = GD.Load<PlayerData>("res://Objects/Player/playerData.tres"); // Load the player data
	private FishingRod _fishingRod;
	public Vector2 _bobberOrigin;
	private Vector2 _launchDirection = Vector2.Zero;
    private float _launchHeight = 50f; // Default launch height for arc
    private float _northLaunchHeight = 1.5f; // Adjustment for the launch height when casting north
    private float _southLaunchHeight = 2f; // Adjustment for the launch height when casting south
	private float _bobberDist = 0f; // Maximum distance to travel
    private float _distTravelled = 0f;
    private float _bobberSpeed = 200f;

    private bool _checkWater = false;

    public override void _Ready()
    {
		_fishingRod = GetParent<FishingRod>();
        _bobberOrigin = Position;
    }

    public void LaunchBobber()
    {
		_launchDirection = _playerData.playerDir.Normalized(); // Normalise the player direction to ensure that the bobber moves in the correct direction
        _distTravelled = 0f;
		_bobberDist = MapFloat(_fishingRod.castLevel, -1, 1, 0, _fishingRod.maxPower); // Calculate the desired distance by mapping the cast power
        _launchHeight = MapFloat(_fishingRod.castLevel, -1, 1, 0, 50);
        
        _checkWater = true;
    }

    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_distTravelled < _bobberDist && _playerData.isFishing) { // If the elapsed time is less than the total travel duration...
            float dirMoveIncrement = _bobberSpeed * (float)delta; // This will increase the position by the speed relative to the framerate
            _distTravelled += dirMoveIncrement; // Increase the distance travelled so far by the above amount
            Vector2 dirMovement = _launchDirection * _distTravelled; // Set the movement along the particular direction to the distance travelled (so that this will increase as time passes)
            
            float distProgress = _distTravelled / _bobberDist; // Get the progress of movement from 0.0 to 1.0

            // Arc trajectory
            // Find the current height of the arc by multiplying pi by the percentage of movement completed
            // in order to get the base y-value (amplitude) of the sine wave at a given point along the journey.
            // Multiply by -1 so that the bobber travels upwards.
            // Multiply by the base launch height so that the maximum amplitude (the peak) is at this value.
            float verticalOffset = Mathf.Sin(Mathf.Pi * distProgress) * -1 * _launchHeight;
            if (_launchDirection.Y < 0) { // If the player is facing north...
                verticalOffset *= _northLaunchHeight; // Increase the amplitude of the sine wave such that the bobber will overshoot its cast and will need to fall to reach its position
            }
            else if (_launchDirection.Y > 0) { // If the player is facing south...
                verticalOffset *= _southLaunchHeight; // Increase the amplitude of the sine wave such that the bobber will overshoot its cast at the start
            }
        
            Position = _bobberOrigin + dirMovement + new Vector2(0, verticalOffset); // Update the bobber's position by adding movement along the constant axis as well as the arc

        } else if (_distTravelled >= _bobberDist) { // Otherwise, if the bobber has finished travelling...
            if (_checkWater && _playerData.isFishing) {
                var spaceState = GetWorld2D().DirectSpaceState; // Get a reference to the worldspace
                var query = new PhysicsPointQueryParameters2D(); // Create a new raycast query that is to be set to a single point
                query.Position = GlobalPosition; // Set the position of the query to the bobber's position
                var results = spaceState.IntersectPoint(query); // Perform the raycast query and store any collisions in the form of an array of dictionaries (representing each collision object)
                if (results.Count > 0) { // If there was at least one collision...
                    for (int i = 0; i < results.Count; i++) { // For each colliding body...
                        if (!GetTree().GetNodesInGroup("waterLayer").Contains((Node)results[i]["collider"])) { // If the colliding body is not the water layer (found by going through the fishing rod, the player, the current scene, and then getting the water layer as a child of TileMapLayers as a child of the current scene)...
                            _playerData.isFishing = false;
                            _checkWater = false;
                            return;
                        } else {
                            var colliderNode = (Node)results[i]["collider"];
                            var colliderGroups = colliderNode.GetGroups();
                            for (int j = 0; j < colliderGroups.Count; j++) {
                                if (colliderGroups[j] == "saltwater" || colliderGroups[j] == "freshwater") bobberSource = colliderGroups[j]; // Store the type of water source
                            }

                            EmitSignal(SignalName.BobberWater);
                            _checkWater = false;
                        }
                    }
                }
            }
        }

        if (!_playerData.isFishing) {  // If the player is not fishing...
			Position = new Vector2(15, -15); // Set the bobber at the desired position
		}
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton) {
            if (mouseButton.ButtonIndex == MouseButton.Left) {
                if (mouseButton.IsPressed()) { // If the left mouse buttons is clicked...
					if (_playerData.isFishing) { // If the player was fishing...
                        _playerData.isCasting = false;
                        _playerData.isFishing = false; // End the fishing action
                        EmitSignal(SignalName.ReelIn); // Signal a reel in
                    }
                }
                if (mouseButton.IsReleased()) {
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
