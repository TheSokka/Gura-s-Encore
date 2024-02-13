using Godot;
using System;

public partial class MainCamera : Node {
	private Camera3D _camera3D;
	private float _mouseSensitivity = .2f;
	private Vector3 _cameraRotationDegrees;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_camera3D = GetNode<Camera3D>("Camera3D");
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Captured);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseInput) {
			GD.Print(_cameraRotationDegrees);
			_cameraRotationDegrees.X += Mathf.DegToRad(-mouseInput.Relative.Y * _mouseSensitivity);
			_cameraRotationDegrees.Y += Mathf.DegToRad(mouseInput.Relative.X * _mouseSensitivity);
			_cameraRotationDegrees.X = Mathf.Clamp(_cameraRotationDegrees.X, -1.11f, 1.11f);

			_camera3D.Call("set_third_person_rotation_degrees", _cameraRotationDegrees);

		}
	}
}
