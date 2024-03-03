using Godot;
using System;
using System.Reflection.PortableExecutable;

public abstract partial class SubState : Node {
    public SubState nextSubState;
    public State currentState;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public abstract void SubStateProcess();

    public abstract void SubStateInput();

    public abstract void EnterSubState();

    public abstract void ExitSubState();
}