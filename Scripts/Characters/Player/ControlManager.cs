using Godot;
using System;
using System.Linq;

public partial class ControlManager : Node {
    private gura _player;
    private bool wasWindowOutOfFocus = true; // Assume the window was out of focus when the game starts

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        _player = (gura)GetTree().GetNodesInGroup("Player").FirstOrDefault();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        bool isWindowFocused = DisplayServer.WindowIsFocused();
        if (isWindowFocused && DisplayServer.MouseGetMode() != DisplayServer.MouseMode.Captured &&
            wasWindowOutOfFocus) {
            // If the window has just regained focus, capture the mouse only if it was out of focus recently
            DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
            wasWindowOutOfFocus = false; // reset this flag as we just handled it
        }
        else if (!isWindowFocused && DisplayServer.MouseGetMode() == DisplayServer.MouseMode.Captured) {
            // If the window loses focus, free the mouse and remember that window was out of focus recently
            DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
            wasWindowOutOfFocus = true;
        }
    }
}