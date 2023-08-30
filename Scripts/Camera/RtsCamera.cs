using Godot;

public partial class RtsCamera : Node3D
{
    [Export] private float _panSpeed = 25;
    [Export] private float _panEdgeSize = 40;
    [Export] private float _rotationSpeed = 1;
    [Export] private float _dragSpeed = 0.02f;
    [Export] private float _zoomSpeed = 5;
    [Export] private float _zoomAmount = 2;
    [Export] private float _minZoomHeight = 4;
    [Export] private float _maxZoomHeight = 55;
    [Export] private Curve _zoomRotationCurve;

    private Camera3D _camera;
    private Node3D _pivot;
    private float _targetPivotHeight;
    private float _targetRotation;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Pivot/Camera3D");
        _pivot = GetNode<Node3D>("Pivot");
        _targetPivotHeight = _pivot.Position.Y;
    }

    public override void _Process(double delta)
    {
        HandleCameraRotation(delta);
        HandleCameraPan(delta);
        HandleCameraZoom(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotionEvent && mouseMotionEvent.ButtonMask == MouseButtonMask.Left)
        {
            //Add 1 so values can be between 1-2.
            var zoomWeight = (_targetPivotHeight - _minZoomHeight) / (_maxZoomHeight - _minZoomHeight) + 1;
            var rotationWeight = (_camera.RotationDegrees.X - _zoomRotationCurve.MinValue) / (_zoomRotationCurve.MaxValue - _zoomRotationCurve.MinValue) + 1;
            var translation = new Vector3(
                    -mouseMotionEvent.Relative.X * zoomWeight * _dragSpeed,
                    0,
                    -mouseMotionEvent.Relative.Y * zoomWeight * rotationWeight * _dragSpeed
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
        if (Input.IsActionPressed("camera_pan_right") || viewPortSize.X - viewPortMousePos.X < _panEdgeSize)
        {
            translation.X = 1;
        }
        if (Input.IsActionPressed("camera_pan_left") || viewPortMousePos.X < _panEdgeSize)
        {
            translation.X = -1;
        }
        if (Input.IsActionPressed("camera_pan_down") || viewPortSize.Y - viewPortMousePos.Y < _panEdgeSize)
        {
            translation.Z = 1;
        }
        if (Input.IsActionPressed("camera_pan_up") || viewPortMousePos.Y < _panEdgeSize)
        {
            translation.Z = -1;
        }
        Translate(translation.Normalized() * _panSpeed * (float)delta);
    }

    private void HandleCameraZoom(double delta)
    {
        var zoomAmount = 0f;
        if (Input.IsActionJustPressed("camera_zoom_in"))
        {
            zoomAmount = -1;
        }
        if (Input.IsActionJustPressed("camera_zoom_out"))
        {
            zoomAmount = 1;
        }

        _targetPivotHeight = Mathf.Clamp(_targetPivotHeight + zoomAmount * _zoomAmount, _minZoomHeight, _maxZoomHeight);

        var targetPivotPosition = new Vector3(_pivot.Position.X, _targetPivotHeight, _pivot.Position.Z);

        _pivot.Position = _pivot.Position.Lerp(targetPivotPosition, (float)delta * _zoomSpeed);

        var zoomWeight = (_targetPivotHeight - _minZoomHeight) / (_maxZoomHeight - _minZoomHeight);
        var targetRotation = new Vector3(_zoomRotationCurve.SampleBaked(zoomWeight), 0, 0);

        _camera.RotationDegrees = _camera.RotationDegrees.Lerp(targetRotation, (float)delta * _zoomSpeed);
    }
}
