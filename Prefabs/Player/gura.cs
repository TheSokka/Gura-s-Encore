using Godot;
using System;

public partial class gura : CharacterBody3D {
	private Node3D _visual;
	private Vector3 _visualRotationDegrees;
    private float _mouseSensitivity = .2f;
	
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	
	// This variable for grabbing the camera in scene
	private Camera3D _camera;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private Vector3 _forwardMovRotation;
	[Export] public float angularVelocity = 5;
	private Vector3 _targetDirection;

	public override void _Ready() {
		_visual = GetNode<Node3D>("Visual");
		_camera = GetTree().Root.GetNode<Camera3D>($"{GetTree().CurrentScene.Name}/MainCamera/Camera3D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		var camHRotation = _camera.GlobalTransform.Basis.GetEuler().Y;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Rotated(Vector3.Up, camHRotation).Normalized();
		
		var lerpFactor = (float) delta * Speed;
		
		if (direction != Vector3.Zero) {
			_targetDirection = direction.Normalized();

			// Apply the new basis to the _visual
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		var targetAngle = Mathf.Atan2(_targetDirection.X, _targetDirection.Z);

		_visual.Rotation = new Vector3(_visual.Rotation.X, (float)Mathf.LerpAngle(_visual.Rotation.Y, targetAngle, delta * angularVelocity) , _visual.Rotation.Z);

		Velocity = velocity;
		MoveAndSlide();
	}
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseInput) {
		}
	}
}
