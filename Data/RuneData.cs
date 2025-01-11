using Godot;
using System;

public partial class RuneData : Resource
{
	public Godot.Collections.Dictionary RunesDict = new Godot.Collections.Dictionary {
		{1, new Godot.Collections.Dictionary{
			{"name", "testRune1"},
			{"attacks", new Godot.Collections.Array{"splash"}},
			{"desc", "This is a test rune."}
		}},
		{2, new Godot.Collections.Dictionary{
			{"name", "testRune2"},
			{"attacks", new Godot.Collections.Array{"slap"}},
			{"desc", "This is a test rune."}
		}},
		{3, new Godot.Collections.Dictionary{
			{"name", "testRune3"},
			{"attacks", new Godot.Collections.Array{"splash", "slap"}},
			{"desc", "This is a test rune."}
		}},
	};
}
