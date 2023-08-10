using Godot;

public partial class Unit : CharacterBody3D
{
    private UnitMoveComponent _moveComponent;
    private SelectableUnitComponent _selectableComponent;
    private Marker3D _unitCenter;

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

    public override void _Ready()
    {
        _moveComponent = this.GetChildNode<UnitMoveComponent>();
        _selectableComponent = this.GetChildNode<SelectableUnitComponent>();
        _unitCenter = GetNodeOrNull<Marker3D>("UnitCenter");
    }

    public void SetMoveTarget(Vector3 moveTarget)
    {
        _moveComponent.SetMoveTarget(moveTarget);
    }

    public Vector3 GetUnitGlobalCenter()
    {
        return _unitCenter?.GlobalPosition ?? GlobalPosition;
    }

    public void TakeDamage(float damage)
    {
        //TODO: implement damage.
    }
}
