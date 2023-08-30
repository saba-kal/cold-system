using Godot;

public partial class Unit : CharacterBody3D
{
    [Signal] public delegate void DeathEventHandler(Unit unit);
    [Signal] public delegate void DamageTakenEventHandler(Unit unit);
    [Signal] public delegate void ModeChangedEventHandler(Unit unit);

    private UnitMoveComponent _moveComponent;
    private SelectableUnitComponent _selectableComponent;
    private HealthComponent _healthComponent;
    private UnitModeComponent _modeComponent;
    private Marker3D _unitCenter;

    public override void _Ready()
    {
        _moveComponent = this.GetChildNode<UnitMoveComponent>();
        _selectableComponent = this.GetChildNode<SelectableUnitComponent>();
        _healthComponent = this.GetChildNode<HealthComponent>();
        _modeComponent = this.GetChildNode<UnitModeComponent>();
        _unitCenter = GetNodeOrNull<Marker3D>("UnitCenter");
        _healthComponent.HealthLost += OnHealthLost;
        _healthComponent.DamageTaken += OnDamageTaken;
        if (_modeComponent != null)
        {
            _modeComponent.ModeChanged += OnModeChanged;
        }
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
        _healthComponent.TakeDamage(damage);
    }

    public float GetCurrentHealth()
    {
        return _healthComponent.GetCurrentHealth();
    }

    public float GetMaxHealth()
    {
        return _healthComponent.GetMaxHealth();
    }

    public void ToggleMode()
    {
        _modeComponent?.ToggleMode();
    }

    public bool IsInAttackMode()
    {
        return _modeComponent?.IsInAttackMode ?? true;
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

    public int UnitNumber
    {
        get => _selectableComponent?.UnitNumber ?? 0;
        set
        {
            if (_selectableComponent != null)
            {
                _selectableComponent.UnitNumber = value;
            }
        }
    }

    private void OnHealthLost()
    {
        EmitSignal(SignalName.Death, this);
        QueueFree();
    }

    private void OnDamageTaken()
    {
        EmitSignal(SignalName.DamageTaken, this);
    }

    private void OnModeChanged()
    {
        EmitSignal(SignalName.ModeChanged, this);
    }
}
