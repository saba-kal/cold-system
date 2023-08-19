using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class HUDManager : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    private List<Unit> GetPlayerUnits()
    {
        return GetTree().GetNodesInGroup(Constants.PLAYER_UNIT_GROUP).Cast<Unit>().ToList();
    }
}
