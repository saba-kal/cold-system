using Godot;

public partial class UnitMoveComponent : NavigationAgent3D
{
    [Export] private float _moveSpeed = 5;
    [Export] private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        // These values need to be adjusted for the actor's speed and the navigation layout.
        //PathDesiredDistance = 1;
        //TargetDesiredDistance = 1;

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

        UpdateVelocity();
        FaceTowardsVelocity();
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

    private void UpdateVelocity()
    {
        var currentAgentPosition = GetParent<Node3D>().GlobalPosition;
        var nextPathPosition = GetNextPathPosition();
        Velocity = (nextPathPosition - currentAgentPosition).Normalized() * _moveSpeed;
    }

    private void FaceTowardsVelocity()
    {
        var nextPathPosition = GetNextPathPosition();
        var unitNode = GetParent<Node3D>();
        var faceDirection = new Vector3(nextPathPosition.X, unitNode.GlobalPosition.Y, nextPathPosition.Z);
        if (unitNode.GlobalTransform.Origin.IsEqualApprox(faceDirection))
        {
            return;
        }

        unitNode.LookAt(faceDirection, Vector3.Up, true);
    }

    private void OnNavigationAgentVelocityComputed(Vector3 safeVelocity)
    {
        var characterBody = GetParent<CharacterBody3D>();
        characterBody.Velocity = safeVelocity;
        characterBody.MoveAndSlide();
    }
}
