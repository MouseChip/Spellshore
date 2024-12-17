using Godot;
using System;

public partial class PlayerData : Resource
{
    public Vector2 PlayerDir;
    public Godot.Collections.Array PlayerInventory = new Godot.Collections.Array();
    public String PlayerHemisphere = "south";
    public bool IsCasting = false;
    public bool IsFishing = false;
}
