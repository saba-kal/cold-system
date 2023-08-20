using Godot;

public partial class UnitMoveComponent : NavigationAgent3D
{
    [Export] private float _moveSpeed = 200;
    [Export] private float _turnSpeed = 10;
    [Export] private AnimationPlayer _animationPlayer;

    private Vector3 _duplicateMoveTarget;

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
            var characterBody = GetParent<CharacterBody3D>();
            if (GetParent<Unit>().UnitNumber == 1)
            {
                //GD.PrintT(characterBody.GlobalPosition, _duplicateMoveTarget);
            }
            if (!characterBody.GlobalPosition.IsEqualApprox(_duplicateMoveTarget))
            {
                //SetMoveTarget(_duplicateMoveTarget);
            }
            return;
        }

        Velocity = GetVelocity((float)delta);
        FaceTowardsVelocity(Velocity.Normalized());
        _animationPlayer?.Play("RunForward");


        var characterBody2 = GetParent<CharacterBody3D>();
        characterBody2.Velocity = Velocity;
        characterBody2.MoveAndSlide();
    }

    public void SetMoveTarget(Vector3 movementTarget)
    {
        GD.Print(movementTarget);
        TargetPosition = movementTarget;
        _duplicateMoveTarget = movementTarget;
    }

    private async void ActorSetup()
    {
        await ToSignal(GetTree(), "physics_frame");
        SetMoveTarget(GetParent<Node3D>().GlobalPosition);
    }

    private Vector3 GetVelocity(float delta)
    {
        var currentAgentPosition = GetParent<Node3D>().GlobalPosition;
        var nextPathPosition = GetNextPathPosition();
        var desiredDirection = (nextPathPosition - currentAgentPosition).Normalized();
        var currentDirection = Velocity.Normalized();
        var direction = currentDirection.Lerp(desiredDirection, delta * _turnSpeed).Normalized();
        return direction * _moveSpeed * delta;
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
