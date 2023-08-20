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
        _rayCast.TargetPosition = _rayCast.ToLocal(targetPosition);
        return !_rayCast.IsColliding();
    }
}
