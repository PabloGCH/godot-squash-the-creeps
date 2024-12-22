using System;
using Godot;

public partial class Mob : CharacterBody3D
{
    [Export]
    public int MinSpeed { get; set; } = 10;

    [Export]
    public int MaxSpeed { get; set; } = 18;

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void Initialize(Vector3 startPosition, Vector3 playerPosisition)
    {
        // Rotates mob to face the player
        LookAtFromPosition(startPosition, playerPosisition, Vector3.Up);
        // Randomizes direction by a range of -45 to 45 degrees
        RotateY((float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4));
        // Calculates a random speed
        int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
        // Sets the mob's velocity
        Velocity = Vector3.Forward * randomSpeed;
        // Rotates the mob's velocity to match the mob's rotation
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
    }

    public void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }
}
