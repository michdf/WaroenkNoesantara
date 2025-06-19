using Godot;

public partial class Player : CharacterBody3D
{
    private const float LerpValue = 0.15f;
    
    private Vector3 _snapVector = Vector3.Down;
    private float _speed;
    
    [ExportGroup("Movement variables")]
    [Export] public float WalkSpeed { get; set; } = 5.0f;
    [Export] public float RunSpeed { get; set; } = 10.0f;
    [Export] public float JumpStrength { get; set; } = 15.0f;
    [Export] public float Gravity { get; set; } = 50.0f;
    
    private const float AnimationBlend = 7.0f;
    
    private Node3D _playerMesh;
    private Node3D _springArmPivot;
    private AnimationTree _animator;
    
    public override void _Ready()
    {
        _playerMesh = GetNode<Node3D>("Mesh");
        _springArmPivot = GetNode<Node3D>("SpringArmPivot");
        _animator = GetNode<AnimationTree>("AnimationTree");
    }
    
    public override void _PhysicsProcess(double delta)
    {
        var moveDirection = Vector3.Zero;
        moveDirection.X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        moveDirection.Z = Input.GetActionStrength("move_backwards") - Input.GetActionStrength("move_forwards");
        moveDirection = moveDirection.Rotated(Vector3.Up, _springArmPivot.Rotation.Y);
        
        if (Input.IsActionPressed("run"))
        {
            _speed = RunSpeed;
        }
        else
        {
            _speed = WalkSpeed;
        }
        
        Velocity = new Vector3(moveDirection.X * _speed, Velocity.Y - Gravity * (float)delta, moveDirection.Z * _speed);
        
        if (moveDirection != Vector3.Zero)
        {
            _playerMesh.Rotation = new Vector3(_playerMesh.Rotation.X, 
                Mathf.LerpAngle(_playerMesh.Rotation.Y, Mathf.Atan2(Velocity.X, Velocity.Z), LerpValue), 
                _playerMesh.Rotation.Z);
        }
        
        bool justLanded = IsOnFloor() && _snapVector == Vector3.Zero;
        bool isJumping = IsOnFloor() && Input.IsActionJustPressed("jump");
        
        if (isJumping)
        {
            Velocity = new Vector3(Velocity.X, JumpStrength, Velocity.Z);
            _snapVector = Vector3.Zero;
        }
        else if (justLanded)
        {
            _snapVector = Vector3.Down;
        }
        
        ApplyFloorSnap();
        MoveAndSlide();
        // Animate((float)delta);
    }
    
//     private void Animate(float delta)
//     {
//         if (IsOnFloor())
//         {
//             _animator.Set("parameters/ground_air_transition/transition_request", "grounded");
            
//             if (Velocity.Length() > 0)
//             {
//                 if (_speed == RunSpeed)
//                 {
//                     _animator.Set("parameters/iwr_blend/blend_amount", 
//                         Mathf.Lerp(_animator.Get("parameters/iwr_blend/blend_amount").AsSingle(), 1.0f, delta * AnimationBlend));
//                 }
//                 else
//                 {
//                     _animator.Set("parameters/iwr_blend/blend_amount", 
//                         Mathf.Lerp(_animator.Get("parameters/iwr_blend/blend_amount").AsSingle(), 0.0f, delta * AnimationBlend));
//                 }
//             }
//             else
//             {
//                 _animator.Set("parameters/iwr_blend/blend_amount", 
//                     Mathf.Lerp(_animator.Get("parameters/iwr_blend/blend_amount").AsSingle(), -1.0f, delta * AnimationBlend));
//             }
//         }
//         else
//         {
//             _animator.Set("parameters/ground_air_transition/transition_request", "air");
//         }
//     }
}
