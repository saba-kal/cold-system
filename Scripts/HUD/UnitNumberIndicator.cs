using Godot;

public partial class UnitNumberIndicator : Node3D
{
    [Export] private Label _unitNumberLabel;

    public void SetUnitNumber(int unitNumber)
    {
        _unitNumberLabel.Text = unitNumber.ToString();
    }
}
