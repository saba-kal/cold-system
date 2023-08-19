using Godot;

public partial class UnitMoveComponent : NavigationAgent3D
{
    [Export] private float _moveSpeed = 200;
    [Export] private float _turnSpeed = 10;
    [Export] private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        CallDeferred("ActorSetup");
        VelocityComputed += OnNavigationAgentVelocityComputed;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsNavigationFinished())
        {
            _animationPlayer?.Play("Stand");
            return;
        }

        var direction = UpdateVelocity((float)delta);
        FaceTowardsVelocity(direction);
        _animationPlayer?.Play("RunForward");
    }

    public void SetMoveTarget(Vector3 movementTarget)
    {
        TargetPosition = movementTarget;
    }

    private async void ActorSetup()
    {
        await ToSignal(GetTree(), "physics_frame");
        SetMoveTarget(GetParent<Node3D>().GlobalPosition);
    }

    private Vector3 UpdateVelocity(float delta)
    {
        var currentAgentPosition = GetParent<Node3D>().GlobalPosition;
        var nextPathPosition = GetNextPathPosition();
        var desiredDirection = (nextPathPosition - currentAgentPosition).Normalized();
        var currentDirection = Velocity.Normalized();
        var direction = currentDirection.Lerp(desiredDirection, delta * _turnSpeed).Normalized();
        Velocity = direction * _moveSpeed * delta;
        return direction;
    }

    private void FaceTowardsVelocity(Vector3 direction)
    {
        var unitNode = GetParent<Node3D>();
        if (unitNode.GlobalTransform.Origin.IsEqualApprox(direction))
        {
            return;
        }

        unitNode.LookAt(unitNode.GlobalPosition + direction, Vector3.Up, true);
    }

    private void OnNavigationAgentVelocityComputed(Vector3 safeVelocity)
    {
        var characterBody = GetParent<CharacterBody3D>();
        characterBody.Velocity = safeVelocity;
        characterBody.MoveAndSlide();
    }
}
