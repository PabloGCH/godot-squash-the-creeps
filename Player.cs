using System;
using Godot;

public partial class Player : CharacterBody3D
{
    private Vector3 _targetVelocity = Vector3.Zero;

    [Export]
    public int Speed { get; set; } = 14;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    [Export]
    public int JumpImpulse { get; set; } = 20;

    [Export]
    public int BounceImpulse { get; set; } = 16;

    [Signal]
    public delegate void DeathEventHandler();

    private Vector3 _velocity = Vector3.Zero;

    private void Die()
    {
        EmitSignal(SignalName.Death);
        QueueFree();
    }

    private void MobDetectorCollision(Node3D body) {
      Die();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 direction = Vector3.Zero;

        //Calculates direction
        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1.0f;
        }

        // Normalizes direction and rotates the player to face the direction
        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        // Calculates ground velocity

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        // Calculates air velocity

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
        }

        // Bounce after squashing a mob

        for (int index = 0; index < GetSlideCollisionCount(); index++)
        {
            // Gets the collision
            KinematicCollision3D collision = GetSlideCollision(index);
            // Checks if the collision was with a mob
            if (collision.GetCollider() is Mob mob)
            {
                // Check if it's a top collision
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
                {
                    mob.Squash();
                    _targetVelocity.Y = BounceImpulse;
                    // Breaks the loop to avoid bouncing multiple times
                    break;
                }
            }
        }

        // Moves the character
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
