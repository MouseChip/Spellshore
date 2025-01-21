using Godot;
using System;

public partial class PlayerData : Resource
{
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
