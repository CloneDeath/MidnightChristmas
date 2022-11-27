using System;
using Godot;
using MidnightChristmas.House.Cookie;

namespace MidnightChristmas; 

public partial class Santa : CharacterBody3D
{
	public const float Speed = 1.0f;
	public const float JumpVelocity = 4.5f;
	public const double TimeToTurnAround = 0.5f;

	public static float Gravity => ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public bool IsTurningAround => TurnAroundTime <= TimeToTurnAround;
	public double TurnAroundTime = TimeToTurnAround + 1;
	public double TurnAroundDirection = 1;

	private Label CookieCount => GetNode<Label>("CanvasLayer/CookieCount");

	public override void _Process(double delta) {
		base._Process(delta);

		CookieCount.Text = Cookie.Count.ToString();
	}

	public override void _PhysicsProcess(double delta) {
		TurnAroundTime += delta;
		if (!IsTurningAround && Input.IsActionJustPressed("turn_around")) {
			TurnAroundTime = 0;
			TurnAroundDirection *= -1;
		}
		if (IsTurningAround) RotateY((float)(TurnAroundDirection * delta * Mathf.Pi / TimeToTurnAround));
		
		ProcessInputs(delta);
	}

	private void ProcessInputs(double delta) {
		var velocity = Velocity;

		velocity = AddGravity(delta, velocity);
		if (!IsTurningAround) velocity = HandleJump(velocity);
		if (!IsTurningAround) velocity = HandleInput(velocity);

		Velocity = velocity;
		MoveAndSlide();

		var rotation = Input.GetAxis("look_right", "look_left") * delta;
		if (!IsTurningAround) {
			if (Math.Abs(rotation) > 0) TurnAroundDirection = Math.Sign(rotation);
			RotateY((float)rotation);
		}
	}

	private Vector3 AddGravity(double delta, Vector3 velocity) {
		if (!IsOnFloor()) velocity.y -= Gravity * (float)delta;
		return velocity;
	}

	private Vector3 HandleJump(Vector3 velocity) {
		if (Input.IsActionJustPressed("move_jump") && IsOnFloor()) velocity.y = JumpVelocity;
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
}