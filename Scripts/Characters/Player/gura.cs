using Godot;
using System;

public partial class gura : CharacterBody3D {
    //Position Debugging
    private Label3D _label3D;

    //Visual Nodes
    private Node3D _visualRot;
    private Node3D _visual;
    private RayCast3D _groundRayCast;

    [ExportCategory("Physics")] [ExportGroup("Locomotion")] [ExportSubgroup("General")] [Export]
    private float _mass = 1;

    [Export] private float _jumpVelocity = 4.5f;
    [ExportSubgroup("Floor")] [Export] private float _walkSpeed = 5.0f;
    [Export] private float _groundAcceleration = 5;
    [Export] private double _groundDeceleration = 5;
    [ExportSubgroup("Rotation")] [Export] private float _angularVelocity = 5;


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    private float _engineGravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    private Vector3 _forwardMovRotation;
    private Vector3 _targetDirection;


    // This variable for grabbing the camera in scene
    private Camera3D _camera;

    public override void _Ready() {
        _visualRot = GetNode<Node3D>("Visual/VisualRotate");
        _visual = GetNode<Node3D>("Visual");
        _label3D = GetNode<Label3D>("Label3D");
        _camera = GetTree().Root.GetNode<Camera3D>($"{GetTree().CurrentScene.Name}/MainCamera/Camera3D");
        _groundRayCast = GetNode<RayCast3D>("Rays/GroundRayCast");
    }

    public override void _PhysicsProcess(double delta) {
        Vector3 velocity = Velocity;
        GD.Print($"{GlobalPosition}");
        var camHRotation = _camera.GlobalTransform.Basis.GetEuler().Y;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= _engineGravity * _mass * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            velocity.Y = _jumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector3 direction = new Vector3(Input.GetActionStrength("MoveRight") - Input.GetActionStrength("MoveLeft"), 0,
                Input.GetActionStrength("MoveBackward") - Input.GetActionStrength("MoveForward"))
            .Rotated(Vector3.Up, camHRotation).Normalized();


        if (direction != Vector3.Zero) {
            _targetDirection = direction;
            // Apply the new basis to the _visual
            velocity.X = (float)Mathf.Lerp(Velocity.X, direction.X * _walkSpeed, delta * _groundAcceleration);
            velocity.Z = (float)Mathf.Lerp(Velocity.Z, direction.Z * _walkSpeed, delta * _groundAcceleration);
        }
        else {
            velocity.X = (float)Mathf.Lerp(Velocity.X, 0, delta * _groundDeceleration);
            velocity.Z = (float)Mathf.Lerp(Velocity.Z, 0, delta * _groundDeceleration);
        }


        var targetAngle = Mathf.Atan2(_targetDirection.X, _targetDirection.Z);

        _visualRot.Rotation = new Vector3(_visualRot.Rotation.X,
            (float)Mathf.LerpAngle(_visualRot.Rotation.Y, targetAngle, delta * _angularVelocity),
            _visualRot.Rotation.Z);

        Velocity = velocity;

        MoveAndSlide();

        RotateOntoSlope(delta);
    }

    private void RotateOntoSlope(double delta) {
        var targetSlopeRot = new Vector3 {
            X = _groundRayCast.GetCollisionNormal().Z,
            Y = 0,
            Z = -_groundRayCast.GetCollisionNormal().X
        };

        _visual.Rotation = _groundRayCast.IsColliding()
            ? _visual.Rotation.Lerp(targetSlopeRot,
                (float)delta * _angularVelocity)
            : _visual.Rotation.Lerp(Vector3.Zero,
                (float)delta * _angularVelocity);
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion mouseInput) {
            
            //Throw Attack
            
        }
    }
}