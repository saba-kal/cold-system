using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class TurretAimComponent : Node3D
{
    public bool TargetIsInSight { get; private set; }

    [Export] private float _range = 10f;
    [Export(PropertyHint.Enum, Constants.UNIT_GROUPS)] private string _aimTarget;
    [Export] private Node3D _turretNode;
    [Export] private Array<BaseWeapon> _gunNodes;

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

        AimAtTarget(closestTarget, (float)delta);
        TargetIsInSight = GunsArePointedAtEnemies();
    }

    private List<Unit> GetTargetUnits()
    {
        return GetTree().GetNodesInGroup(_aimTarget).Cast<Unit>().ToList();
    }

    private Unit GetClosestTarget(List<Unit> potentialTargets)
    {
        Unit closestTarget = null;
        var minDistanceSqr = float.MaxValue;
        var rangeSqr = _range * _range;
        foreach (var target in potentialTargets)
        {
            var distanceSqr = target.GetUnitGlobalCenter().DistanceSquaredTo(GlobalPosition);
            if (distanceSqr < minDistanceSqr &&
                distanceSqr < rangeSqr &&
                LineOfSightIsFree(target.GetUnitGlobalCenter()))
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

    private void AimAtTarget(Unit target, float delta)
    {
        var desiredTransform = _turretNode.GlobalTransform.LookingAt(target.GetUnitGlobalCenter(), Vector3.Up);

        // IDK why I need to turn the turret 180 degrees, but I do. Otherwise, it faces the opposite direction.
        desiredTransform = desiredTransform.RotatedLocal(Vector3.Up, Mathf.Pi);

        var scale = _turretNode.Scale;
        _turretNode.GlobalTransform = _turretNode.GlobalTransform.InterpolateWith(desiredTransform, delta * 20);
        _turretNode.Scale = scale; //Setting global transform resets scale. So we need to set it back to it's original value.

        // Revert X and Z rotations since we only want to rotate by Y.
        _turretNode.Rotation = new Vector3(0, _turretNode.Rotation.Y, 0);

        foreach (var gunNode in _gunNodes)
        {
            gunNode.LookAt(target.GetUnitGlobalCenter(), Vector3.Up, true);
            if (gunNode.Scale.X > 0)
            {
                gunNode.Rotation = new Vector3(gunNode.Rotation.X, 0, 0);
            }
            else
            {
                // The gun is mirrored to the other side of the mech. So we need to flip it back by rotating 180 degrees.
                gunNode.Rotation = new Vector3(gunNode.Rotation.X - Mathf.Pi, 0, 0);
            }
        }
    }

    private bool GunsArePointedAtEnemies()
    {
        foreach (var gun in _gunNodes)
        {
            if (gun.IsPointedAtTarget())
            {
                return true;
            }
        }
        return false;
    }
}
