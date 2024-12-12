using Godot;
using System;

public partial class TestSceneData : Resource
{
	[Export] public Godot.Collections.Array PotentialFish = new Godot.Collections.Array{};
	[Export] public String Weather = "sunny";
}
