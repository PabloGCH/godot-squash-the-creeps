using System;
using Godot;

public partial class Player : CharacterBody3D
{
    private Vector3 _targetVelocity = Vector3.Zero;

    [Export]
    public int Speed { get; set; } = 14;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Vector3 _velocity = Vector3.Zero;


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
          _targetVelocity.X -= FallAcceleration * (float)delta;
        }
        
        // Moves the character

        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
