using Godot;
using System;

public partial class PGrounded : State {


    public override void StateProcess() {
        if (currentSubState.nextSubState != null) {
            SwitchSubStates(currentSubState.nextSubState);
        }
        currentSubState.SubStateProcess();
    }

    public override void StateInput() {
        currentSubState.SubStateInput();
    }

    public override void Enter() {
        
    }

    public override void Exit() {
        
    }

    private void SwitchSubStates(SubState nextSubState) {
        if (currentSubState != null) {
            currentSubState.ExitSubState();
            currentSubState = nextSubState;
            currentSubState.nextSubState = null;
        }

        currentSubState = nextSubState;
        currentSubState.EnterSubState();
    }
}