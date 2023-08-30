using Godot;


public partial class UnitModeComponent : Node3D
{
    [Signal] public delegate void ModeChangedEventHandler();

    public bool IsInAttackMode { get; private set; } = true;

    [Export] private float _attackModeSpeedModifier = 0.5f;
    [Export] private float _moveModeSpeedModifier = 1f;
    [Export] private AnimationPlayer _animationPlayer;

    private float _baseSpeed;
    private UnitMoveComponent _unitMoveComponent;
    private WeaponManager _weaponManager;

    public override void _Ready()
    {
        _unitMoveComponent = this.GetNeighborNode<UnitMoveComponent>();
        _weaponManager = this.GetNeighborNode<WeaponManager>();
        _baseSpeed = _unitMoveComponent.GetSpeed();
        ToggleWeapons();
        UpdateSpeed();
    }

    public void ToggleMode()
    {
        IsInAttackMode = !IsInAttackMode;
        ToggleWeapons();
        UpdateSpeed();
        EmitSignal(SignalName.ModeChanged);
    }

    private void ToggleWeapons()
    {
        _weaponManager.EnableWeapons = IsInAttackMode;
    }

    private void UpdateSpeed()
    {
        var speedModifier = IsInAttackMode ? _attackModeSpeedModifier : _moveModeSpeedModifier;
        _unitMoveComponent.SetSpeed(_baseSpeed * speedModifier);
        if (_animationPlayer != null)
        {
            _animationPlayer.SpeedScale = speedModifier;
        }
    }
}