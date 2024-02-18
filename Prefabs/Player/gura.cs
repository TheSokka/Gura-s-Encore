using Godot;
using System;

public partial class gura : CharacterBody3D {
    
    //Position Debugging
    private Label3D _label3D;
    
    private Node3D _visual;
    private Vector3 _visualRotationDegrees;
    private float _mouseSensitivity = .2f;

    [Export] private float _walkSpeed = 5.0f;
    [Export] private float _jumpVelocity = 4.5f;


    // This variable for grabbing the camera in scene
    private Camera3D _camera;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    private Vector3 _forwardMovRotation;
    [Export] public float angularVelocity = 5;
    private Vector3 _targetDirection;
    [Export] private double _groundDeceleration = 5;
    [Export] private float _acceleration = 1;

    public override void _Ready() {
        _visual = GetNode<Node3D>("Visual");
        _label3D = GetNode<Label3D>("Label3D");
        _camera = GetTree().Root.GetNode<Camera3D>($"{GetTree().CurrentScene.Name}/MainCamera/Camera3D");
    }

    public override void _PhysicsProcess(double delta) {
        Vector3 velocity = Velocity;
        GD.Print( $"{GlobalPosition}");
        var camHRotation = _camera.GlobalTransform.Basis.GetEuler().Y;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            velocity.Y = _jumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector3 direction = new Vector3(Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft"), 0,
            Input.GetActionStrength("MoveBackward") - Input.GetActionStrength("MoveForward")).Rotated(Vector3.Up, camHRotation).Normalized();
        
        

        if (direction != Vector3.Zero) {
            _targetDirection = direction;
            // Apply the new basis to the _visual
            velocity.X = (float)Mathf.Lerp(Velocity.X, direction.X * _walkSpeed, delta * _acceleration);
            velocity.Z = (float)Mathf.Lerp(Velocity.Z, direction.Z * _walkSpeed, delta * _acceleration);
        }
        else {

            velocity.X = (float)Mathf.Lerp(Velocity.X, 0, delta * _groundDeceleration);
            velocity.Z =  (float)Mathf.Lerp(Velocity.Z, 0, delta * _groundDeceleration);

        }
        

        var targetAngle = Mathf.Atan2(_targetDirection.X, _targetDirection.Z);

        _visual.Rotation = new Vector3(_visual.Rotation.X,
            (float)Mathf.LerpAngle(_visual.Rotation.Y, targetAngle, delta * angularVelocity), _visual.Rotation.Z);

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseInput) { }
    }
}