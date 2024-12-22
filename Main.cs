using System;
using Godot;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    private void OnMobTimerTimeOut()
    {
        // Instances the mob scene
        Mob mob = MobScene.Instantiate<Mob>();

        // Gets the location of the spawn path
        var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");

        // Gets a random spawn location from the path
        mobSpawnLocation.ProgressRatio = GD.Randf();

        // Gets the player position
        Vector3 playerPosition = GetNode<Player>("Player").Position;

        // Initializes the mob with it's spawn location and the player location
        mob.Initialize(mobSpawnLocation.Position, playerPosition);

        // Spawns the mob by adding it to the main scene
        AddChild(mob);
        // Conects the mob's squashed signal to score label
        mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;
    }

    private void OnPlayerDeath() {
      GetNode<Timer>("MobTimer").Stop();
    }
}
