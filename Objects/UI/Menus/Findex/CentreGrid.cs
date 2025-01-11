using Godot;

public partial class CentreGrid : Control
{
	[Export] private int _cellSize; // Size of each cell
    [Export] private int _totalWidth; // Total width of the grid (including horizontal separator values)
	[Export] private int _horSeparator; // Horizontal separator value

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

        float totalChildrenWidth = childCount * (_cellSize + _horSeparator);  // The total width of the children, found by multiplying the width of a child by the number of children

        CustomMinimumSize = new Vector2(totalChildrenWidth, Size.Y); // Set the minimum size of the grid to fit children exactly

        // Calculate the starting position to center children within the visible area
        float startX = Mathf.Max((_totalWidth - totalChildrenWidth) / 2.0f, 0); // Find the start position, either by ensuring there are equal margins on both sides of the grid or by setting the start position to 0 if the grid is too small

        // Position each child
        for (int i = 0; i < children.Count; i++) { // For each child
            var child = children[i]; // Get the child
            float x = startX + (i * _cellSize) + (_horSeparator * i); // Calculate the x position by moving it forward from startX by the number of previous children added (with an additional separator between them)
            child.Position = new Vector2(x, 0);
        }
	}
}
