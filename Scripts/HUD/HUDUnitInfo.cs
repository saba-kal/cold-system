using Godot;


public partial class HUDUnitInfo : Control
{
    public Unit Unit { get; set; }

    [Export] private ProgressBar _healthBar;
    [Export] private Label _unitNumberLabel;
    [Export] private Control _attackModeIcon;
    [Export] private Control _moveModeIcon;

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
        Unit.ModeChanged += OnModeChanged;
        UpdateModeIcons();
    }

    private void OnDamageTaken(Unit _)
    {
        _healthBar.Value = Unit.GetCurrentHealth();
    }

    private void OnModeChanged(Unit _)
    {
        UpdateModeIcons();
    }

    private void UpdateModeIcons()
    {
        _attackModeIcon.Visible = Unit.IsInAttackMode();
        _moveModeIcon.Visible = !Unit.IsInAttackMode();
    }
}