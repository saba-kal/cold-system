using Godot;

public partial class SelectableUnitComponent : Node3D
{
    [Export] private bool _defaultIsSelected = false;
    [Export] private Node3D _selectedIndicator;

    private UnitNumberIndicator _unitNumberIndicator;
    private int _unitNumber;

    public override void _Ready()
    {
        _selectedIndicator.Visible = _defaultIsSelected;
        _unitNumberIndicator = this.GetChildNode<UnitNumberIndicator>();
    }
    public bool IsSelected { get => _selectedIndicator.Visible; set => _selectedIndicator.Visible = value; }
    public int UnitNumber
    {
        get => _unitNumber;
        set
        {
            _unitNumber = value;
            _unitNumberIndicator?.SetUnitNumber(value);
        }
    }
}
