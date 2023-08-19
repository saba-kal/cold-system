using Godot;

public partial class HealthComponent : Node3D
{
    [Signal] public delegate void HealthLostEventHandler();
    [Signal] public delegate void DamageTakenEventHandler();

    [Export] private float _maxHealth = 30f;

    private float _currentHealth;

    public override void _Ready()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        if (_currentHealth <= 0)
        {
            EmitSignal(SignalName.HealthLost);
        }
        EmitSignal(SignalName.DamageTaken);
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
}
