using Godot;
using System;

public partial class PlayerData : Resource
{
    [Signal] public delegate void AvatarChangedEventHandler();

    public Color RobeColour { 
        get => _robeColour;
        set {
            _robeColour = value;
            EmitSignal(SignalName.AvatarChanged);
        }
    }
    public Color CatColour { 
        get => _catColour;
        set {
            _catColour = value;
            EmitSignal(SignalName.AvatarChanged);
        }
    }
    public string HatType { 
        get => _hatType;
        set {
            _hatType = value;
            EmitSignal(SignalName.AvatarChanged);
        }
    }

    private Color _robeColour = new Color(1, 1, 1);
    private Color _catColour = new Color(1, 1, 1);
    private string _hatType = "WizardHat";

    public Vector2 PlayerDir;
    public Godot.Collections.Dictionary PlayerInventory = new Godot.Collections.Dictionary {
        {"rods", new Godot.Collections.Array{1, 2, 3}},
        {"wands", new Godot.Collections.Array{1, 2}},
        {"bait", new Godot.Collections.Array{1, 2, 3, 4, 5}},
        {"fish", new Godot.Collections.Array()},
        {"runes", new Godot.Collections.Array{1, 2, 3, 4, 5}}
    };
    public String PlayerHemisphere = "south";
    public int PlayerHealth = 100;
    public int EquippedRod = 1;
    public int EquippedWand = 1;
    public bool IsCasting = false;
    public bool IsFishing = false;
    public bool AutoRun = false;
}
