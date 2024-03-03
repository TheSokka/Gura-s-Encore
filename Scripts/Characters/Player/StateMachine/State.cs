using Godot;
using System;
using System.Linq;

public abstract partial class State : Node {
    [Export] private bool _canMove = true;

    public State nextState;
    [Export] public SubState currentSubState;
    public gura player;
    public SubState[] subStates;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        player = GetOwner<gura>();
        subStates = GetParent().GetChildren().OfType<SubState>().ToArray();
        foreach (var child in GetChildren()) {
            if (child is SubState subState) {
                subState.currentState = null;
                subStates.Append(child);
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public abstract void StateProcess();

    public abstract void StateInput();

    public abstract void Enter();

    public abstract void Exit();
}