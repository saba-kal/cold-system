using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerController : Node3D
{
    private List<Unit> _units = new List<Unit>();
    private bool _mouseIsPressed = false;
    private Camera3D _camera;

    public override void _Ready()
    {
        _camera = GetViewport().GetCamera3D();
        var unitNumber = 1;
        foreach (Unit unit in GetChildren())
        {
            unit.UnitNumber = unitNumber;
            _units.Add(unit);
            unitNumber++;
        }
    }

    public override void _Process(double delta)
    {
        for (var i = 0; i < _units.Count; i++)
        {
            if (IsInstanceValid(_units[i]))
            {
                _units[i].IsSelected = Input.IsActionPressed($"select_unit_{i + 1}");
            }
        }

        if (Input.IsActionPressed("select_position"))
        {
            var mousePosition = GetMousePosition();
            if (mousePosition != null)
            {
                foreach (var unit in GetSelectedUnits())
                {
                    unit.SetMoveTarget(mousePosition.Value);
                }
            }
        }
    }

    private Vector3? GetMousePosition()
    {
        var worldSpace = GetWorld3D().DirectSpaceState;
        var mousePosition = GetViewport().GetMousePosition();
        var from = _camera.ProjectRayOrigin(mousePosition);
        var to = from + _camera.ProjectRayNormal(mousePosition) * 1000;
        var query = PhysicsRayQueryParameters3D.Create(from, to, Constants.GROUND_COLLISION_LAYER);
        var result = worldSpace.IntersectRay(query);
        if (result?.Count > 0)
        {
            return (Vector3)result["position"];
        }

        return null;
    }

    private List<Unit> GetSelectedUnits()
    {
        return _units.Where(u => IsInstanceValid(u) && u.IsSelected).ToList();
    }
}
