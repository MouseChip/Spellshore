using Godot;

public partial class CentreGrid : Control
{
	private int _cellSize = 80; // Size of each cell
    private int _totalWidth = 254; // Total width of the grid (including horizontal separator values)
	private int _horSeparator = 7; // Horizontal separator value

    public override void _Ready()
    {
        ChildEnteredTree += ArrangeChildren; // Arrange children when a new child is added
    }

    private void ArrangeChildren(Node node) {
        var children = new Godot.Collections.Array<Control>();
        foreach (Control child in GetChildren()) { // For every child to be arranged...
            children.Add(child); // Add the child
        }

        int childCount = children.Count; // Count the children
        if (childCount == 0) return; // If there are no children, return

        float totalChildrenWidth = childCount * _cellSize;  // The total width of the children, found by multiplying the width of a child by the number of children
        float startX = (_totalWidth - totalChildrenWidth) / 2.0f; // Find the starting position from the centre by taking the distance between the two widths and halving it (i.e. creating equal margins on either side of the grid)

        // Position each child
        for (int i = 0; i < children.Count; i++) { // For each child
            var child = children[i]; // Get the child
            float x = startX + (i * _cellSize) + (_horSeparator * i); // Calculate the x position by moving it forward from startX by the number of previous children added (with an additional separator between them)
            child.Position = new Vector2(x, 0);
        }
	}
}
