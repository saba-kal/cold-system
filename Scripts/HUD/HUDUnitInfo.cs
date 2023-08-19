using Godot;


public partial class HUDUnitInfo : Control
{
    public Unit Unit { get; set; }

    [Export] private ProgressBar _healthBar;
    [Export] private Label _unitNumberLabel;

    public override void _Ready()
    {
        if (Unit == null)
        {
            return;
        }

        _healthBar.MaxValue = Unit.GetMaxHealth();
        _healthBar.Value = Unit.GetCurrentHealth();
        _unitNumberLabel.Text = Unit.UnitNumber.ToString();
        Unit.DamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(Unit _)
    {
        _healthBar.Value = Unit.GetCurrentHealth();
    }
}