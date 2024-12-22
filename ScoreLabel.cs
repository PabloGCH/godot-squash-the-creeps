using System;
using Godot;

public partial class ScoreLabel : Label
{
    private int _score = 0;

    public void OnMobSquashed()
    {
        GD.Print("score increased");
        _score++;
        Text = "Score: " + _score;
    }
}
