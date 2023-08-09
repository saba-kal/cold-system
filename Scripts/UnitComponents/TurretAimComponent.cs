using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class TurretAimComponent : Node3D
{
    public bool TargetIsInSight { get; private set; }

    [Export] private float _range = 10f;
    [Export(PropertyHint.Enum, Constants.UNIT_GROUPS)] private string _aimTarget;
    [Export] private Node3D _turretNode;
    [Export] private Node3D _gunNode;

    private LineOfSightComponent _lineOfSightComponent;

    public override void _Ready()
    {
        _lineOfSightComponent = this.GetNeighborNode<LineOfSightComponent>();
    }

    public override void _Process(double delta)
    {
        var targetUnits = GetTargetUnits();
        if (targetUnits.Count == 0)
        {
            TargetIsInSight = false;
            return;
        }

        var closestTarget = GetClosestTarget(targetUnits);
        if (closestTarget == null)
        {
            TargetIsInSight = false;
            return;
        }

        AimAtTarget(closestTarget);
        TargetIsInSight = true;
    }

    private List<Node3D> GetTargetUnits()
    {
        return GetTree().GetNodesInGroup(_aimTarget).Cast<Node3D>().ToList();
    }

    private Node3D GetClosestTarget(List<Node3D> potentialTargets)
    {
        Node3D closestTarget = null;
        var minDistanceSqr = float.MaxValue;
        var rangeSqr = _range * _range;
        foreach (var target in potentialTargets)
        {
            var distanceSqr = target.GlobalPosition.DistanceSquaredTo(GlobalPosition);
            if (distanceSqr < minDistanceSqr &&
                distanceSqr < rangeSqr &&
                LineOfSightIsFree(target.GlobalPosition))
            {
                closestTarget = target;
                minDistanceSqr = distanceSqr;
            }
        }

        return closestTarget;
    }

    private bool LineOfSightIsFree(Vector3 targetPosition)
    {
        if (_lineOfSightComponent == null)
        {
            return true;
        }

        return _lineOfSightComponent.LineOfSightIsFree(targetPosition);
    }

    private void AimAtTarget(Node3D target)
    {
        _turretNode.LookAt(target.GlobalPosition, Vector3.Up, true);
        _turretNode.Rotation = new Vector3(0, _turretNode.Rotation.Y, 0);
        _gunNode.LookAt(target.GlobalPosition, Vector3.Up, true);
        _gunNode.Rotation = new Vector3(_gunNode.Rotation.X, 0, 0);
    }
}
