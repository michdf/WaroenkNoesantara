using Godot;

public partial class SpringArmPivot : Node3D
{
	[ExportGroup("FOV")]
	[Export] public bool ChangeFovOnRun { get; set; }
	[Export] public float NormalFov { get; set; } = 75.0f;
	[Export] public float RunFov { get; set; } = 90.0f;

	[ExportGroup("Zoom")]
	[Export] public float MinZoom { get; set; } = 2.0f;
	[Export] public float MaxZoom { get; set; } = 10.0f;
	[Export] public float ZoomSpeed { get; set; } = 0.5f;

	private const float CameraBlend = 0.05f;

	private SpringArm3D _springArm;
	private Camera3D _camera;
	private bool _isRightMousePressed = false;

	public override void _Ready()
	{
		_springArm = GetNode<SpringArm3D>("SpringArm3D");
		_camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Track right mouse button state
		if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Right)
		{
			_isRightMousePressed = mouseButton.Pressed;
		}

		// Handle mouse scroll for zoom
		if (@event is InputEventMouseButton scrollEvent)
		{
			if (scrollEvent.ButtonIndex == MouseButton.WheelUp && scrollEvent.Pressed)
			{
				_springArm.SpringLength = Mathf.Clamp(_springArm.SpringLength - ZoomSpeed, MinZoom, MaxZoom);
			}
			else if (scrollEvent.ButtonIndex == MouseButton.WheelDown && scrollEvent.Pressed)
			{
				_springArm.SpringLength = Mathf.Clamp(_springArm.SpringLength + ZoomSpeed, MinZoom, MaxZoom);
			}
		}

		// Only process mouse movement when right mouse button is held
		if (@event is InputEventMouseMotion mouseMotion && _isRightMousePressed)
		{
			RotateY(-mouseMotion.Relative.X * 0.005f);
			_springArm.RotateX(-mouseMotion.Relative.Y * 0.005f);
			_springArm.Rotation = new Vector3(
				Mathf.Clamp(_springArm.Rotation.X, -Mathf.Pi / 4, Mathf.Pi / 4),
				_springArm.Rotation.Y,
				_springArm.Rotation.Z
			);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Only change FOV when right mouse button is held
		if (ChangeFovOnRun && _isRightMousePressed)
		{
			var ownerCharacter = Owner as CharacterBody3D;
			if (ownerCharacter?.IsOnFloor() == true)
			{
				if (Input.IsActionPressed("run"))
				{
					_camera.Fov = Mathf.Lerp(_camera.Fov, RunFov, CameraBlend);
				}
				else
				{
					_camera.Fov = Mathf.Lerp(_camera.Fov, NormalFov, CameraBlend);
				}
			}
			else
			{
				_camera.Fov = Mathf.Lerp(_camera.Fov, NormalFov, CameraBlend);
			}
		}
	}
}
