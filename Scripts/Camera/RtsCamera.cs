using Godot;

public partial class RtsCamera : Node3D
{
    [Export] private float _panSpeed = 25;
    [Export] private float _panEdgeSize = 40;
    [Export] private float _rotationSpeed = 1;
    [Export] private float _dragSpeed = 30;

    private Camera3D _camera;
    private Node3D _pivot;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Pivot/Camera3D");
        _pivot = GetNode<Node3D>("Pivot");
    }

    public override void _Process(double delta)
    {
        HandleCameraRotation(delta);
        HandleCameraPan(delta);
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent && mouseEvent.ButtonMask == MouseButtonMask.Middle)
        {
            var viewPortSize = GetViewport().GetVisibleRect().Size;
            var translation = new Vector3(
                    -mouseEvent.Relative.X / viewPortSize.X * _dragSpeed,
                    0,
                    -mouseEvent.Relative.Y / viewPortSize.Y * _dragSpeed
                );
            Translate(translation);
        }
    }

    private void HandleCameraRotation(double delta)
    {
        var rotation = 0f;
        if (Input.IsActionPressed("camera_rotate_left"))
        {
            rotation = -1;
        }
        if (Input.IsActionPressed("camera_rotate_right"))
        {
            rotation = 1;
        }
        Rotate(Vector3.Up, rotation * _rotationSpeed * (float)delta);
    }

    private void HandleCameraPan(double delta)
    {
        var viewPortMousePos = GetViewport().GetMousePosition();
        var viewPortSize = GetViewport().GetVisibleRect().Size;
        var translation = new Vector3();
        if (viewPortSize.X - viewPortMousePos.X < _panEdgeSize)
        {
            translation.X = 1;
        }
        if (viewPortMousePos.X < _panEdgeSize)
        {
            translation.X = -1;
        }
        if (viewPortSize.Y - viewPortMousePos.Y < _panEdgeSize)
        {
            translation.Z = 1;
        }
        if (viewPortMousePos.Y < _panEdgeSize)
        {
            translation.Z = -1;
        }
        Translate(translation.Normalized() * _panSpeed * (float)delta);
    }
}
