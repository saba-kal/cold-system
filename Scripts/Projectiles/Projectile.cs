using Godot;
using System.Collections.Generic;

public partial class Projectile : Area3D
{
    [Export] private PackedScene _onDeathEffect;
    [Export] private Area3D _explosionArea;

    private Vector3 _direction = Vector3.Forward;
    private float _damage = 1f;
    private float _speed = 30f;
    private string _targetGroup = Constants.ENEMY_UNIT_GROUP;
    private string _ignoreGroup = Constants.PLAYER_UNIT_GROUP;
    private List<Unit> _unitsInExplosionArea = new List<Unit>();

    public void Init(
        Vector3 direction,
        float damage,
        float speed,
        string targetGroup,
        string ignoreGroup)
    {
        _direction = direction.Normalized();
        _damage = damage;
        _speed = speed;
        _targetGroup = targetGroup;
        _ignoreGroup = ignoreGroup;
    }

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        if (_explosionArea != null)
        {
            _explosionArea.BodyEntered += OnExplosionAreaBodyEntered;
            _explosionArea.BodyExited += OnExplosionAreaBodyExited;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += _direction * _speed * (float)delta;
        LookAt(GlobalPosition + _direction);
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup(_ignoreGroup))
        {
            return;
        }

        // Damage the target.
        Unit targetUnit = null;
        if (body.IsInGroup(_targetGroup) && body is Unit)
        {
            targetUnit = (Unit)body;
            targetUnit.TakeDamage(_damage);
        }

        // Damage any other targets caught in the explosion area.
        foreach (var nearbyUnit in _unitsInExplosionArea)
        {
            if (targetUnit != nearbyUnit)
            {
                nearbyUnit.TakeDamage(_damage);
            }
        }

        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (_onDeathEffect != null)
        {
            var effect = _onDeathEffect.Instantiate() as Node3D;
            effect.Position = GlobalPosition;
            GetTree().Root.AddChild(effect);
        }
        QueueFree();
    }

    private void OnExplosionAreaBodyEntered(Node3D body)
    {
        if (body.IsInGroup(_targetGroup) &&
            body is Unit unit)
        {
            _unitsInExplosionArea.Add(unit);
        }
    }

    private void OnExplosionAreaBodyExited(Node3D body)
    {
        if (body.IsInGroup(_targetGroup) &&
            body is Unit unit)
        {
            _unitsInExplosionArea.Remove(unit);
        }
    }
}
