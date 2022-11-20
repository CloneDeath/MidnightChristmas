using Godot;

namespace MidnightChristmas; 

public partial class Santa : CharacterBody3D
{
	public const float Speed = 1.0f;
	public const float JumpVelocity = 4.5f;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;

		//velocity = AddGravity(delta, velocity);
		//velocity = HandleJump(velocity);
		velocity = HandleInput(velocity);

		Velocity = velocity;
		MoveAndSlide();
		
		var rotation = Input.GetAxis("look_right", "look_left") * delta;
		RotateY((float)rotation);
	}

	private Vector3 AddGravity(double delta, Vector3 velocity) {
		if (!IsOnFloor()) velocity.y -= gravity * (float)delta;
		return velocity;
	}

	private Vector3 HandleInput(Vector3 velocity) {
		var inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		var direction = (Transform.basis * new Vector3(inputDir.x, 0, inputDir.y)).Normalized();
		if (direction != Vector3.Zero) {
			velocity.x = direction.x * Speed;
			velocity.z = direction.z * Speed;
		}
		else {
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
			velocity.z = Mathf.MoveToward(Velocity.z, 0, Speed);
		}

		return velocity;
	}

	private Vector3 HandleJump(Vector3 velocity) {
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) velocity.y = JumpVelocity;
		return velocity;
	}
}