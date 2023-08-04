using Godot;

public partial class Unit : CharacterBody3D
{
    [Export] private UnitMoveComponent _moveComponent;
    [Export] private SelectableUnitComponent _selectableComponent;

    public void SetMoveTarget(Vector3 moveTarget)
    {
        _moveComponent.SetMoveTarget(moveTarget);
    }

    public bool IsSelected
    {
        get => _selectableComponent?.IsSelected ?? false;
        set
        {
            if (_selectableComponent != null)
            {
                _selectableComponent.IsSelected = value;
            }
        }
    }
}
