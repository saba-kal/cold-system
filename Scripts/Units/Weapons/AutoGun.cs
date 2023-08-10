using Godot;

public partial class AutoGun : BaseWeapon
{
    [Export] public float _fireRate = 10;
    [Export] private float _projectileDamage = 1;
    [Export] private float _projectileSpeed = 30;
    [Export] private PackedScene _projectileScene;
    [Export] private Marker3D _projectileSpawnPoint;

    private float _timeSinceLastShot = 0;
    private float _timeBetweenShots = 1;

    public float FireRate => _fireRate;

    public override void _Ready()
    {
        base._Ready();
        _timeBetweenShots = 1f / _fireRate;
    }

    public override void _Process(double delta)
    {
        if (ReadyToFire())
        {
            SpawnProjectile();
            PlaySoundEffect();
            _timeSinceLastShot = 0;
        }
        else
        {
            _timeSinceLastShot += (float)delta;
        }
    }

    public bool ReadyToFire()
    {
        return Enabled && _timeSinceLastShot >= _timeBetweenShots;
    }

    public void DelayShot(float delayAmount)
    {
        _timeSinceLastShot = _timeBetweenShots - delayAmount;
    }

    private void SpawnProjectile()
    {
        var projectile = _projectileScene.Instantiate<Projectile>();
        projectile.Init(
            GlobalTransform.Basis.Z,
            _projectileDamage,
            _projectileSpeed,
            _targetGroup,
            UnitUtils.GetOppositeUnitGroup(_targetGroup));
        GetTree().Root.AddChild(projectile);
        projectile.GlobalPosition = _projectileSpawnPoint.GlobalPosition;
    }
}
