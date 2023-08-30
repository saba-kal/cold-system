using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerController : Node3D
{
    private List<Unit> _units = new List<Unit>();
    private bool _mouseIsPressed = false;
    private Camera3D _camera;
    private GroupFormationManager _formationManager;

    public override void _Ready()
    {
        _camera = GetViewport().GetCamera3D();
        _formationManager = this.GetChildNode<GroupFormationManager>();
        var unitNumber = 1;
        foreach (var unit in this.GetChildren<Unit>())
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
                _units[i].IsSelected = Input.IsActionPressed($"select_unit_{i + 1}") || Input.IsActionPressed("select_all_units");
            }
        }
        _formationManager.SelectedUnitCount = _units.Count(u => u.IsSelected);

        var mousePosition = GetMousePosition();
        _formationManager.FormationPosition = mousePosition ?? Vector3.Zero;
        _formationManager.Visible = mousePosition != null;
        if (mousePosition != null && Input.IsActionJustPressed("select_position"))
        {
            var formationPositions = _formationManager.GetFormationPositions();
            var i = 0;
            foreach (var unit in GetSelectedUnits())
            {
                unit.SetMoveTarget(formationPositions[i]);
                i++;
            }
        }

        if (Input.IsActionJustPressed("toggle_unit_mode"))
        {
            foreach (var unit in GetSelectedUnits())
            {
                unit.ToggleMode();
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
