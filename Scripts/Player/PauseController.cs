using Godot;

public partial class PauseController : Node3D
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause"))
        {
            GetTree().Paused = !GetTree().Paused;
        }
    }
}
