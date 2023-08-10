using Godot;
using Godot.Collections;

public partial class LineOfSightComponent : Node3D
{
    [Export] private Array<Marker3D> _startPositions;

    public bool LineOfSightIsFree(Vector3 targetPosition)
    {
        var worldSpace = GetWorld3D().DirectSpaceState;
        foreach (var marker in _startPositions)
        {
            var query = PhysicsRayQueryParameters3D.Create(marker.Position, targetPosition, Constants.GROUND_COLLISION_LAYER);
            var result = worldSpace.IntersectRay(query);
            if (result?.Count > 0)
            {
                return false;
            }
        }

        return true;
    }
}
