using Godot;

public abstract partial class BaseWeapon : Node3D
{
    [Export(PropertyHint.Enum, Constants.UNIT_GROUPS)] protected string _targetGroup = Constants.ENEMY_UNIT_GROUP;
    [Export] private RayCast3D _lineOfSightRay;
    [Export] private Node3D _externalWeaponModel;

    private AudioStreamPlayer3D _soundEffectPlayer;

    public bool Enabled { get; set; }

    public override void _Ready()
    {
        _soundEffectPlayer = this.GetChildNode<AudioStreamPlayer3D>();
        _lineOfSightRay.CollisionMask = UnitUtils.GetUnitGroupCollisionLayer(_targetGroup);
    }

    public override void _Process(double delta)
    {
        if (_externalWeaponModel != null)
        {
            _externalWeaponModel.Rotation = Rotation;
        }
    }

    public void PlaySoundEffect()
    {
        _soundEffectPlayer?.Play();
    }

    public bool IsPointedAtTarget(Vector3 target)
    {
        if (_lineOfSightRay == null)
        {
            return true;
        }

        return _lineOfSightRay.IsColliding();
    }
}
