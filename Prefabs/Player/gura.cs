using Godot;
using System;

public partial class gura : CharacterBody3D {
	private Node3D _visual;
	private Vector3 _visualRotationDegrees;
    private float _mouseSensitivity = .2f;
	
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	private Vector3 _forwardMovRotation;

	public override void _Ready() {
		_visual = GetNode<Node3D>("Visual");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		
		float lerpFactor = (float) delta * Speed;
		
		if (direction != Vector3.Zero) {
			var targetDirection = direction.Normalized();
			var targetAngle = Mathf.Atan2(targetDirection.X, targetDirection.Z);

			var newAngle = Mathf.LerpAngle(Rotation.Y, Mathf.Atan2(velocity.X, velocity.Y), 10f * delta);

			_visual.Rotation = new Vector3(_visual.Rotation.X, (float)newAngle, _visual.Rotation.Z);
			// Apply the new basis to the _visual
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseInput) {
			_visual.RotateY(Mathf.DegToRad(-mouseInput.Relative.X * _mouseSensitivity));
			RotateY(Mathf.DegToRad(mouseInput.Relative.X * _mouseSensitivity));
		}
	}
}
