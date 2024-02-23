using System.Linq;
using Godot;

namespace GurasEncore.Scenes.Characters.Player;

public partial class StateMachine : Node {
    private State _currentState;
    
    [ExportCategory("Export Vars")]
    [Export] private AnimationTree _animTree = new();
    [Export] private gura _player;

    private State[] _states;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _states = new State[GetChildren().Count];
        foreach (var child in GetChildren()) {
            if (child is State state) {
                state.player = _player;

                _states.Append(child);
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        if (_currentState.nextState != null)
            SwitchStates(_currentState.nextState);

        _currentState.StateProcess();
    }


    private void SwitchStates(State nextState) {
        if (_currentState != null) {
            _currentState.Exit();
            _currentState = nextState;
            _currentState.nextState = null;
        }

        _currentState = nextState;
        _currentState.Enter();
    }
}