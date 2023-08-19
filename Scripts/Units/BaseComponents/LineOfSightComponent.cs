using Godot;
using Godot.Collections;

public partial class LineOfSightComponent : Node3D
{
    [Export] private Array<Marker3D> _startPositions;

    private RayCast3D _rayCast;

    public override void _Ready()
    {
        _rayCast = this.GetChildNode<RayCast3D>();
    }

    public bool LineOfSightIsFree(Vector3 targetPosition)
    {
        foreach (var marker in _startPositions)
        {
            var distanceToTarget = marker.GlobalPosition.DistanceTo(targetPosition);
            var from = marker.GlobalPosition;
            var to = marker.GlobalPosition + marker.GlobalTransform.Basis.Z * distanceToTarget;
            _rayCast.GlobalPosition = from;
            _rayCast.TargetPosition = _rayCast.ToLocal(to);
            if (!_rayCast.IsColliding())
            {
                return true;
            }
        }

        return false;
    }
}
