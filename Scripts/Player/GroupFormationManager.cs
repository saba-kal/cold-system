using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class GroupFormationManager : Node3D
{
    public int SelectedUnitCount { get; set; }
    public Vector3 FormationPosition { get; set; }

    [Export] private float circleFormationRadius = 4f;
    [Export] private float heightAdjustRayLength = 20f;

    private List<Node3D> _moveTargetIndicators;

    public override void _Ready()
    {
        _moveTargetIndicators = GetChildren().Cast<Node3D>().ToList();
    }

    public override void _Process(double delta)
    {
        GlobalPosition = FormationPosition;
        HideUnselectedUnitMoveIndicators();
        PlaceIndicatorsInCircle();
        AdjustIndicatorHeights();
    }


    public List<Vector3> GetFormationPositions()
    {
        return _moveTargetIndicators.Select(m => m.GlobalPosition).ToList();
    }


    private void HideUnselectedUnitMoveIndicators()
    {
        for (var i = 0; i < _moveTargetIndicators.Count; i++)
        {
            _moveTargetIndicators[i].Visible = i < SelectedUnitCount;
        }
    }

    private void PlaceIndicatorsInCircle()
    {
        if (SelectedUnitCount == 1)
        {
            _moveTargetIndicators[0].Position = Vector3.Zero;
            return;
        }

        var adjustedRadius = circleFormationRadius * SelectedUnitCount / 5f;

        if (SelectedUnitCount == 2)
        {
            _moveTargetIndicators[0].Position = new Vector3(adjustedRadius, 0, 0);
            _moveTargetIndicators[1].Position = new Vector3(-adjustedRadius, 0, 0);
            return;
        }

        for (var i = 0; i < SelectedUnitCount; i++)
        {
            var circlePosition = i / (float)SelectedUnitCount;
            var x = Mathf.Sin(circlePosition * Mathf.Pi * 2.0f) * adjustedRadius;
            var z = Mathf.Cos(circlePosition * Mathf.Pi * 2.0f) * adjustedRadius;
            _moveTargetIndicators[i].Position = new Vector3(x, 0, z);
        }
    }

    private void AdjustIndicatorHeights()
    {
        if (SelectedUnitCount == 1)
        {
            return;
        }

        var worldSpace = GetWorld3D().DirectSpaceState;
        foreach (var indicator in _moveTargetIndicators)
        {
            var from = indicator.GlobalPosition + new Vector3(0, heightAdjustRayLength, 0);
            var to = from - new Vector3(0, heightAdjustRayLength * 2, 0);
            var query = PhysicsRayQueryParameters3D.Create(from, to, Constants.GROUND_COLLISION_LAYER);
            var result = worldSpace.IntersectRay(query);
            if (result?.Count > 0)
            {
                indicator.GlobalPosition = (Vector3)result["position"];
            }
        }
    }
}
