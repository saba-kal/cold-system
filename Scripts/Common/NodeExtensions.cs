using Godot;
using System.Collections.Generic;

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

    public static T GetChildNode<T>(this Node node)
    {
        foreach (var childNode in node.GetChildren())
        {
            if (childNode is T targetNode)
            {
                return targetNode;
            }
        }

        return default;
    }

    public static List<T> GetAllChildren<T>(this Node node)
    {
        var children = new List<T>();
        foreach (var childNode in node.GetChildren())
        {
            if (childNode is T targetNode)
            {
                children.Add(targetNode);
            }
            children.AddRange(GetAllChildren<T>(childNode));
        }
        return children;
    }
}