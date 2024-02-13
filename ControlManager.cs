using Godot;
using System;
using System.Linq;

public partial class ControlManager : Node {

	private gura _player;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_player = (gura)GetTree().GetNodesInGroup("Player").FirstOrDefault();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
