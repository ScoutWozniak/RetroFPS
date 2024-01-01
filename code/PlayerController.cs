using Sandbox;
using Sandbox.Citizen;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime;

public class PlayerController : Component, INetworkSerializable
{
	[Property] public Vector3 Gravity { get; set; } = new Vector3( 0, 0, 800 );

	public Vector3 WishVelocity { get; private set; }
	[Property] public GameObject Eye { get; set; }

	[Property] float HeadbobSpeed { get; set; } = 5.0f;
	[Property] float HeadbobAmmount { get; set; } = 5.0f;

	[Category( "Movement" )]
	[Property] float RunSpeed { get; set; } = 320.0f;

	[Category( "Movement" )]
	[Property] float WalkSpeed { get; set; } = 110.0f;

	public Angles EyeAngles;
	public bool IsRunning;

	protected override void OnEnabled()
	{
		base.OnEnabled();

		if ( IsProxy )
			return;

		var cam = Scene.GetAllComponents<CameraComponent>().FirstOrDefault();
		if ( cam is not null )
		{
			EyeAngles = cam.Transform.Rotation.Angles();
			EyeAngles.roll = 0;
		}
	}

	protected override void OnUpdate()
	{
		// Eye input
		if ( !IsProxy )
		{
			EyeAngles.pitch += Input.MouseDelta.y * 0.1f;
			EyeAngles.pitch = Math.Clamp( EyeAngles.pitch, -89.0f, 89.0f );
			EyeAngles.yaw -= Input.MouseDelta.x * 0.1f;
			//EyeAngles.roll = ((Input.Down( "Right" ) ? 1 : 0) - (Input.Down( "Left" ) ? 1 : 0)) * 2.0f;

			var cam = Scene.GetAllComponents<CameraComponent>().FirstOrDefault();

			var lookDir = EyeAngles.ToRotation();


			cam.Transform.Position = Eye.Transform.Position;
			cam.Transform.Rotation = lookDir * Eye.Transform.LocalRotation;

			// Headbob!
			if (WishVelocity.Length > 0)
			{
				//cam.Transform.Position += Vector3.Up * MathF.Sin( Time.Now * HeadbobSpeed ) * HeadbobAmmount;
			}



			IsRunning = Input.Down( "Run" );

			if ( Input.Pressed( "use" ) )
			{
				var tr = Scene.Trace.Ray(cam.Transform.Position, cam.Transform.Position + cam.Transform.Rotation.Forward * 100.0f).WithoutTags("playerhitbox").Run();
				if (tr.Hit && tr.GameObject != null)
				{
					Log.Info( tr.GameObject );
					var use = tr.GameObject.Components.Get<UsableComponent>();
					if ( use != null)
					{
						use.OnUse();
					}
				}
			}
		}
		

		
	}
	public void OnJump( float floatValue, string dataString, object[] objects, Vector3 position )
	{
		//AnimationHelper?.TriggerJump();
	}

	float fJumps;

	protected override void OnFixedUpdate()
	{
		if ( IsProxy )
			return;

		BuildWishVelocity();

		var cc = GameObject.Components.Get<CharacterController>();

		if ( cc.IsOnGround && Input.Down( "Jump" ) )
		{
			float flGroundFactor = 1.0f;
			float flMul = 268.3281572999747f * 1.2f;
			//if ( Duck.IsActive )
			//	flMul *= 0.8f;

			cc.Punch( Vector3.Up * flMul * flGroundFactor );
			//	cc.IsOnGround = false;

			OnJump( fJumps, "Hello", new object[] { Time.Now.ToString(), 43.0f }, Vector3.Random );

			fJumps += 1.0f;

		}

		if ( cc.IsOnGround )
		{
			cc.Velocity = cc.Velocity.WithZ( 0 );
			cc.Accelerate( WishVelocity );
			cc.ApplyFriction( 4.0f );
		}
		else
		{
			cc.Velocity -= Gravity * Time.Delta * 0.5f;
			cc.Accelerate( WishVelocity.ClampLength( 50 ) );
			cc.ApplyFriction( 0.1f );
		}

		cc.Move();

		if ( !cc.IsOnGround )
		{
			cc.Velocity -= Gravity * Time.Delta * 0.5f;
		}
		else
		{
			cc.Velocity = cc.Velocity.WithZ( 0 );
		}
	}

	public void BuildWishVelocity()
	{
		var rot = EyeAngles.ToRotation();

		WishVelocity = 0;

		if ( Input.Down( "Forward" ) ) WishVelocity += rot.Forward;
		if ( Input.Down( "Backward" ) ) WishVelocity += rot.Backward;
		if ( Input.Down( "Left" ) ) WishVelocity += rot.Left;
		if ( Input.Down( "Right" ) ) WishVelocity += rot.Right;

		WishVelocity = WishVelocity.WithZ( 0 );

		if ( !WishVelocity.IsNearZeroLength ) WishVelocity = WishVelocity.Normal;

		if ( Input.Down( "Run" ) ) WishVelocity *= WalkSpeed;
		else WishVelocity *= RunSpeed;
	}

	public void Write( ref ByteStream stream )
	{
		stream.Write( IsRunning );
		stream.Write( EyeAngles );
	}

	public void Read( ByteStream stream )
	{
		IsRunning = stream.Read<bool>();
		EyeAngles = stream.Read<Angles>();
	}
}
