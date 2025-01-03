using Godot;
using System;

public partial class PlayerData : Resource
{
    public Vector2 PlayerDir;
    public Godot.Collections.Dictionary PlayerInventory = new Godot.Collections.Dictionary {
        {"rods", new Godot.Collections.Array{"r0", "r1", "r2"}},
        {"wands", new Godot.Collections.Array{"w0", "w1"}},
        {"bait", new Godot.Collections.Array{"b0", "b1", "b2", "b3", "b4"}},
        {"fish", new Godot.Collections.Array()}
    };
    public String PlayerHemisphere = "south";
    public bool IsCasting = false;
    public bool IsFishing = false;
    public bool AutoRun = false;
}
