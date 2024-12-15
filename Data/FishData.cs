using Godot;
using System;

public partial class FishData : Resource
{
	public int HookedFish;

	public Godot.Collections.Dictionary FishDict = new Godot.Collections.Dictionary {
		{0, new Godot.Collections.Dictionary{
			{"name", "testFish1"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"all"}},
			{"climate", new Godot.Collections.Array{"sunny", "rainy"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 50},
			{"difficulty", 50},
			{"baseLength", 8},
			{"lenVar", 2},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"bait", new Godot.Collections.Array{"all"}},
			{"caught", 0},
		}},

		{1, new Godot.Collections.Dictionary{
			{"name", "testFish2"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"saltwater"}},
			{"climate", new Godot.Collections.Array{"sunny", "cloudy"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 20},
			{"difficulty", 10},
			{"baseLength", 60},
			{"lenVar", 20},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"bait", new Godot.Collections.Array{"worm", "squid"}},
			{"caught", 0},
		}},

		{2, new Godot.Collections.Dictionary{
			{"name", "testFish3"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"all"}},
			{"climate", new Godot.Collections.Array{"sunny"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 70},
			{"difficulty", 80},
			{"baseLength", 300},
			{"lenVar", 50},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"bait", new Godot.Collections.Array{"worm"}},
			{"caught", 0},
		}}
	};
}
