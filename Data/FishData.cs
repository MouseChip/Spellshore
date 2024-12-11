using Godot;
using System;

public partial class FishData : Resource
{
	public Godot.Collections.Dictionary fishDict = new Godot.Collections.Dictionary {
		{0, new Godot.Collections.Dictionary{
			{"name", "testFish1"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"all"}},
			{"climate", new Godot.Collections.Array{"sunny", "rainy"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 50},
			{"difficulty", 50},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"bait", new Godot.Collections.Array{"all"}},
			{"caught", 0},
		}}
	};
}
