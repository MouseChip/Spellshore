using Godot;
using System;

public partial class RuneData : Resource
{
	public Godot.Collections.Dictionary RunesDict = new Godot.Collections.Dictionary {
		{"u0", new Godot.Collections.Dictionary{
			{"name", "testRune1"},
			{"attacks", new Godot.Collections.Array{"splash"}}
		}},
		{"u1", new Godot.Collections.Dictionary{
			{"name", "testRune2"},
			{"attacks", new Godot.Collections.Array{"slap"}}
		}},
		{"u2", new Godot.Collections.Dictionary{
			{"name", "testRune3"},
			{"attacks", new Godot.Collections.Array{"splash, slap"}}
		}},
	};
}
