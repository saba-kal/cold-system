using Godot;
using System.Collections.Generic;

public partial class WeaponManager : Node3D
{
    public bool EnableWeapons { get; set; } = true;

    private List<BaseWeapon> _weapons;
    private TurretAimComponent _turretAimComponent;

    private bool _previousEnabled = false;

    public override void _Ready()
    {
        _weapons = GetParent().GetAllChildren<BaseWeapon>();
        _turretAimComponent = this.GetNeighborNode<TurretAimComponent>();
        OffsetWeaponsFireRates();
    }

    public override void _Process(double delta)
    {
        if (!EnableWeapons)
        {
            _previousEnabled = false;
            return;
        }

        var weaponsEnabled = _turretAimComponent?.TargetIsInSight ?? false;
        if (weaponsEnabled == true && _previousEnabled == false)
        {
            OffsetWeaponsFireRates();
        }
        foreach (var weapon in _weapons)
        {
            weapon.Enabled = weaponsEnabled;
        }
        _previousEnabled = weaponsEnabled;
    }

    private void OffsetWeaponsFireRates()
    {
        for (var i = 1; i < _weapons.Count; i++)
        {
            if (_weapons[i] is AutoGun autoGun)
            {
                // This will make it so that the guns don't fire at the same time.
                autoGun.DelayShot((1f / autoGun.FireRate) * i / 2);
            }
        }
    }
}
