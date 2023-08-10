using Godot;

public partial class SelectableUnitComponent : Node3D
{
    [Export] private bool _defaultIsSelected = false;
    [Export] private Node3D _selectedIndicator;

    public bool IsSelected { get => _selectedIndicator.Visible; set => _selectedIndicator.Visible = value; }

    public override void _Ready()
    {
        _selectedIndicator.Visible = _defaultIsSelected;
    }
}
