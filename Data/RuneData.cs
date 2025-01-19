using Godot;
using System;

public partial class RuneData : Resource
{
	public Godot.Collections.Dictionary RunesDict = new Godot.Collections.Dictionary {
		{1, new Godot.Collections.Dictionary{
			{"name", "uruz"},
			{"template", new Godot.Collections.Array{new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0.6f), new Vector2(1, 1)}},
			{"attacks", new Godot.Collections.Array{"splash"}},
			{"desc", "This is a test rune."}
		}},
		{2, new Godot.Collections.Dictionary{
			{"name", "thurisaz"},
			{"template", new Godot.Collections.Array{new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0.5f), new Vector2(0, 1)}},
			{"attacks", new Godot.Collections.Array{"slap"}},
			{"desc", "This is a test rune."}
		}},
		{3, new Godot.Collections.Dictionary{
			{"name", "raido"},
			{"template", new Godot.Collections.Array{new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0.25f), new Vector2(0, 0.5f), new Vector2(1, 1)}},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"desc", "This is a test rune."}
		}},
		{4, new Godot.Collections.Dictionary{
			{"name", "kenaz"},
			{"template", new Godot.Collections.Array{new Vector2(1, 0), new Vector2(0, 0.5f), new Vector2(1, 1)}},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"desc", "This is a test rune."}
		}},
		{5, new Godot.Collections.Dictionary{
			{"name", "laguz"},
			{"template", new Godot.Collections.Array{new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0.5f)}},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"desc", "This is a test rune."}
		}},
	};
}
