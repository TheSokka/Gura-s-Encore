using Godot;
using System;

public partial class PGrounded : State
{
    
    private SubState _currentSubState;
    
    
    
    public override void StateProcess() {
        _currentSubState.StateProcess();
    }

    public override void StateInput() {
        throw new NotImplementedException();
    }

    public override void Enter() {
        throw new NotImplementedException();
    }

    public override void Exit() {
        throw new NotImplementedException();
    }
}
