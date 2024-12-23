using System;
using Godot;

public partial class Mob : CharacterBody3D
{
    [Export]
    public int MinSpeed { get; set; } = 10;

    [Export]
    public int MaxSpeed { get; set; } = 18;

    [Signal]
    public delegate void SquashedEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void Initialize(Vector3 startPosition, Vector3 playerPosisition)
    {
        // Rotates mob to face the player
        Vector3 targetPosition = new Vector3(playerPosisition.X, 0, playerPosisition.Z);
        LookAtFromPosition(startPosition, targetPosition, Vector3.Up);
        // Randomizes direction by a range of -45 to 45 degrees
        RotateY((float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4));
        // Calculates a random speed
        int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
        // Sets the mob's velocity
        Velocity = Vector3.Forward * randomSpeed;
        // Rotates the mob's velocity to match the mob's rotation
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
        GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = randomSpeed / MinSpeed;
    }

    public void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }

    public void Squash()
    {
        EmitSignal(SignalName.Squashed);
        QueueFree();
    }
}
