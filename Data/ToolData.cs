using Godot;
using System;

public partial class ToolData : Resource
{   
    public String CurRod;
    public String CurWand;

    public Godot.Collections.Dictionary ToolDict = new Godot.Collections.Dictionary{
        {"r0", new Godot.Collections.Dictionary{
			{"name", "Basic Rod"},
            {"desc", "Ol' faithful. Can't get much simpler than a piece of driftwood with string attached. That's pretty much what a rod is, right?"},
            {"luck", 5},
            {"power", 3},
            {"cost", 0}
		}},
        {"r1", new Godot.Collections.Dictionary{
			{"name", "Stone Rod"},
            {"desc", "A little more durable and powerful than the basic rod. Not necessarily as attractive to fish, though. Not too much more useful."},
            {"luck", 3},
            {"power", 5},
            {"cost", 15}
		}},
        {"r2", new Godot.Collections.Dictionary{
			{"name", "Bronze Rod"},
            {"desc", "Obviously the next step in technological innovation. More powerful than the two simplest rods and more attractive to fish."},
            {"luck", 10},
            {"power", 5},
            {"cost", 25}
		}},
        {"r3", new Godot.Collections.Dictionary{
			{"name", "Iron Rod"},
            {"desc", "A moderately powerful rod that attracts moderately uncommon fish. Quite a bit more expensive, though."},
            {"luck", 25},
            {"power", 8},
            {"cost", 55}
		}},
        {"w0", new Godot.Collections.Dictionary{
			{"name", "Basic Wand"},
            {"desc", "Ol' faithful. Can't get much simpler than a piece of driftwood imbued with the spirits of mystical beings and magical powers."},
            {"intelligence", 15},
            {"power", 5},
            {"cost", 0}
		}},
        {"w1", new Godot.Collections.Dictionary{
			{"name", "Maple Wand"},
            {"desc", "Slightly more powerful than the basic wand, though not much more intelligent."},
            {"intelligence", 18},
            {"power", 8},
            {"cost", 15}
		}},
        {"w2", new Godot.Collections.Dictionary{
			{"name", "Foo Wand"},
            {"desc", "I kinda just needed at least one more wand and was running out of ideas, these aren't even gonna stick around. Shut up."},
            {"intelligence", 10},
            {"power", 5},
            {"cost", 25}
		}}
    };

    public Godot.Collections.Dictionary BaitDict = new Godot.Collections.Dictionary{
        {"b0", new Godot.Collections.Dictionary{
            {"name", "Basic Bait"},
            {"rarity", 5}
        }},
        {"b1", new Godot.Collections.Dictionary{
            {"name", "Better Bait"},
            {"rarity", 15}
        }},
        {"b2", new Godot.Collections.Dictionary{
            {"name", "Wild Bait"},
            {"rarity", 25}
        }},
        {"b3", new Godot.Collections.Dictionary{
            {"name", "Magic Bait"},
            {"rarity", 35}
        }},
        {"b4", new Godot.Collections.Dictionary{
            {"name", "Super Bait"},
            {"rarity", 45}
        }},
        {"b5", new Godot.Collections.Dictionary{
            {"name", "Double Bait"},
            {"rarity", 55}
        }}
    };
}