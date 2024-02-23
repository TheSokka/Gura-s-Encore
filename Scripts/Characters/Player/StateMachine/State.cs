using Godot;
using System;

public abstract partial class State : Node {
    [Export] private bool _canMove = true;

    public State nextState;

    public gura player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public abstract void StateProcess();

    public abstract void StateInput();

    public abstract void Enter();

    public abstract void Exit();
}