using Godot;
using System;
using System.Reflection.PortableExecutable;

public abstract partial class SubState : Node {
    public SubState nextSubState;

    public gura player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public abstract void StateProcess();

    public abstract void StateInput();

    public abstract void Enter();

    public abstract void Exit();
}