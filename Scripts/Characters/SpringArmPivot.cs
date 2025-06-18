using Godot;

public partial class SpringArmPivot : Node3D
{
	[ExportGroup("FOV")]
	[Export] public bool ChangeFovOnRun { get; set; }
	[Export] public float NormalFov { get; set; } = 75.0f;
	[Export] public float RunFov { get; set; } = 90.0f;

	private const float CameraBlend = 0.05f;

	private SpringArm3D _springArm;
	private Camera3D _camera;

	public override void _Ready()
	{
		_springArm = GetNode<SpringArm3D>("SpringArm3D");
		_camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
		
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
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
		if (ChangeFovOnRun)
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
