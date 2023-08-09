using Godot;

public static class NodeExtensions
{
    public static T GetNeighborNode<T>(this Node node)
    {
        foreach (var neighborNode in node.GetParent().GetChildren())
        {
            if (neighborNode is T targetNode)
            {
                return targetNode;
            }
        }

        return default;
    }
}