using Godot;
using System;

public partial class AttackData : Resource
{
    public Godot.Collections.Dictionary AttackDict = new Godot.Collections.Dictionary{
        {"splash", new Godot.Collections.Dictionary{
            {"time", 5},
            {"damage", 25},
            {"runes", new Godot.Collections.Array{1, 2}},
            {"fish", new Godot.Collections.Array{1, 2, 3}}
        }},
        {"slap", new Godot.Collections.Dictionary{
            {"time", 5},
            {"damage", 25},
            {"runes", new Godot.Collections.Array{1, 2}},
            {"fish", new Godot.Collections.Array{1, 2, 3}}
        }}
    };
}
