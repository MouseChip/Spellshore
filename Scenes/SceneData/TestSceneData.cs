using Godot;
using System;

public partial class TestSceneData : Resource
{
	[Export] public Godot.Collections.Array potentialFish = new Godot.Collections.Array{};
	[Export] public String weather = "sunny";
}
