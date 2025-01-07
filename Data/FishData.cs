using Godot;
using System;

public partial class FishData : Resource
{
	public int HookedFish;

	public Godot.Collections.Dictionary FishDict = new Godot.Collections.Dictionary {
		{1, new Godot.Collections.Dictionary{
			{"name", "testFish1"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"all"}},
			{"climate", new Godot.Collections.Array{"sunny", "rainy"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 50},
			{"difficulty", 50},
			{"baseLength", 8},
			{"lenVar", 2},
			{"pb", 8},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"bait", new Godot.Collections.Array{"all"}},
			{"caught", 3},
			{"description", "test fish 1 description, such as location, source, climate, hours, bait, and more"},
			{"info", "test fish 1 info, such as location, climate, hours, and bait."}
		}},

		{2, new Godot.Collections.Dictionary{
			{"name", "testFish2"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"saltwater"}},
			{"climate", new Godot.Collections.Array{"sunny", "cloudy"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 20},
			{"difficulty", 10},
			{"baseLength", 60},
			{"lenVar", 20},
			{"pb", 40},
			{"attacks", new Godot.Collections.Array{"splash"}},
			{"bait", new Godot.Collections.Array{"worm", "squid"}},
			{"caught", 1},
			{"description", "test fish 2 description, such as location, source, climate, hours, bait, and more"},
			{"info", "test fish 2 info, such as location, climate, hours, and bait."}
		}},

		{3, new Godot.Collections.Dictionary{
			{"name", "testFish3"},
			{"scene", new Godot.Collections.Array{"testing_scene_1"}},
			{"source", new Godot.Collections.Array{"all"}},
			{"climate", new Godot.Collections.Array{"sunny"}},
			{"hours", new Godot.Collections.Array{1, 5, 12, 17}},
			{"rarity", 70},
			{"difficulty", 80},
			{"baseLength", 300},
			{"lenVar", 50},
			{"pb", 320},
			{"attacks", new Godot.Collections.Array{"slap"}},
			{"bait", new Godot.Collections.Array{"worm"}},
			{"caught", 0},
			{"description", "test fish 3 description, such as location, source, climate, hours, bait, and more"},
			{"info", "test fish 3 info, such as location, climate, hours, and bait."}
		}}
	};
}
