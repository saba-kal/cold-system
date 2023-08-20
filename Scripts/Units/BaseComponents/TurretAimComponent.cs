using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class TurretAimComponent : Node3D
{
    public bool TargetIsInSight { get; private set; }

    [Export] private float _range = 10f;
    [Export] private float _turretRotationSpeed = 20f;
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
            ResetRotation((float)delta);
            return;
        }

        var closestTarget = GetClosestTarget(targetUnits);
        if (closestTarget == null)
        {
            TargetIsInSight = false;
            ResetRotation((float)delta);
            return;
        }

        AimAtTarget(closestTarget, (float)delta);
        TargetIsInSight = GunsArePointedAtEnemies(closestTarget);
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
        _turretNode.GlobalTransform = _turretNode.GlobalTransform.InterpolateWith(desiredTransform, Mathf.Clamp(delta * _turretRotationSpeed, 0, 1));
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

    private void ResetRotation(float delta)
    {
        _turretNode.Rotation = _turretNode.Rotation.Lerp(Vector3.Zero, Mathf.Clamp(delta * _turretRotationSpeed, 0, 1));
        foreach (var gunNode in _gunNodes)
        {
            if (gunNode.Scale.X > 0)
            {
                gunNode.Rotation = Vector3.Zero;
            }
            else
            {
                // The gun is mirrored to the other side of the mech. So we need to flip it back by rotating 180 degrees.
                gunNode.Rotation = new Vector3(-Mathf.Pi, 0, 0);
            }
        }
    }

    private bool GunsArePointedAtEnemies(Unit unit)
    {
        foreach (var gun in _gunNodes)
        {
            if (gun.IsPointedAtTarget(unit.GetUnitGlobalCenter()))
            {
                return true;
            }
        }
        return false;
    }
}
