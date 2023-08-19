using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class UnitInfoManager : Control
{
    [Export] private PackedScene _unitInfoScene;

    public override void _Ready()
    {
        ClearUnitInfo();
        foreach (var playerUnit in GetPlayerUnits())
        {
            var unitInfo = _unitInfoScene.Instantiate<HUDUnitInfo>();
            unitInfo.Unit = playerUnit;
            AddChild(unitInfo);
        }
    }

    private void ClearUnitInfo()
    {
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }
    }

    private List<Unit> GetPlayerUnits()
    {
        return GetTree().GetNodesInGroup(Constants.PLAYER_UNIT_GROUP).Cast<Unit>().ToList();
    }
}
