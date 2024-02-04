using Sandbox;
using Sandbox.Citizen;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public sealed class EnemyStateComponent : Component
{
	public enum EnemyState
	{
		E_IDLE,
		E_SEARCHING,
		E_RANGED,
		E_MELEE
	}

	[Property] EnemyState State { get; set; }

	int RegenPathRate = 10;

	int SinceRegen = 0;

	[Property] GameObject Target { get;set; }

	[Property] CharacterController CC { get; set; }

	[Property] float MoveSpeed { get; set; }

	[Property] float AttackRate { get; set; }

	TimeSince LastAttack { get; set; }

	CancellationTokenSource AttackTokenSource;

	[Property] GameObject ProjectilePrefab { get; set; }

	[Property] public Vector3 Gravity { get; set; } = new Vector3( 0, 0, 1200 );

	[Property] CitizenAnimationHelper AnimHelper { get; set; }

	[Property] NavMeshAgent NavAgent { get; set; }

	Vector3 WishVelocity { get; set; }

	protected override void OnEnabled()
	{
		base.OnEnabled();
		LastAttack = AttackRate;

		AnimHelper.Handedness = CitizenAnimationHelper.Hand.Both;
		AnimHelper.HoldType = CitizenAnimationHelper.HoldTypes.Pistol;
	}
	protected override void OnFixedUpdate()
	{
		switch (State)
		{
			case EnemyState.E_IDLE:
				StateIdle();
				break;
			case EnemyState.E_SEARCHING:
				StateFind();
				break;
			case EnemyState.E_RANGED:
				StateAttack();
				break;
		}

		//Log.Info( State );

		if ( CC.IsOnGround )
		{
			CC.Velocity = CC.Velocity.WithZ( 0 );
			CC.Accelerate( WishVelocity );
			CC.ApplyFriction( 4.0f );
		}
		else
		{
			CC.Velocity -= Gravity * Time.Delta * 0.5f;
			CC.Accelerate( WishVelocity.ClampLength( 50 ) );
			CC.ApplyFriction( 0.1f );
		}

		CC.Move();

		if ( !CC.IsOnGround )
		{
			CC.Velocity -= Gravity * Time.Delta * 0.5f;
		}
		else
		{
			CC.Velocity = CC.Velocity.WithZ( 0 );
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		AnimHelper.WithVelocity( CC.Velocity );
	}

	void StateIdle()
	{
		if ( Target == null )
			Target = Scene.Components.Get<PlayerController>(FindMode.InDescendants).GameObject;



		var playerForward = (Target.Transform.Position - Transform.Position).Normal;

		var startPos = Vector3.Up * 56.0f + Transform.Position;
		var endPos = playerForward * 1000.0f + startPos;

		if (Vector3.Dot( Transform.Rotation.Forward, playerForward ) > 0.25f)
		{
			Gizmo.Draw.SolidSphere( startPos, 10.0f );
			Gizmo.Draw.SolidSphere( endPos, 10.0f );

			var tr = Scene.Trace.Ray( startPos, endPos ).WithoutTags("trigger", "enemy")
				.Run();

			if ( tr.Hit && tr.GameObject.Tags.Has( "playerhitbox" ) )
			{
				State = EnemyState.E_SEARCHING;
				NavAgent.MoveTo( Target.Transform.Position );
			}
		}
	}

	void StateFind()
	{
		// Reset to idle if we don't have any targets
		if ( Target == null )
		{
			State = EnemyState.E_IDLE;
			return;
		}
		

		if ((Target.Transform.Position - Transform.Position).Length < 500 && CanAttack())
		{
			State = EnemyState.E_RANGED;
		}

		Transform.Rotation = Rotation.LookAt( (Target.Transform.Position.WithZ(0) - Transform.Position.WithZ(0)).Normal );
	}

	void StateAttack()
	{
		if ( CanAttack() )
		{
			Log.Info( "attack" );
			if ( AttackTokenSource != null )
				AttackTokenSource.Cancel();
			AttackTokenSource = new CancellationTokenSource();

			_ = AttackFunc();

			LastAttack = 0;
			WishVelocity = 0;
		}
	}

	bool CanAttack()
	{
		return (LastAttack >= AttackRate);
	}

	async Task AttackFunc()
	{
		await Task.DelaySeconds( 0.25f );
		if ( Enabled )
		{
			Sound.Play( "grunt.pain" );
			var projectile = ProjectilePrefab.Clone();
			projectile.Transform.Position = GetProjectileSpawn();
			projectile.Transform.Rotation = Rotation.LookAt( GetProjectileNormal() );

			State = EnemyState.E_SEARCHING;
		}
	}

	Vector3 GetProjectileSpawn()
	{
		Vector3 pos = new Vector3();

		pos = Transform.Position;
		pos += Transform.Rotation.Forward * 20.0f;
		pos += Vector3.Up * 52.0f;


		return pos;
	}

	Vector3 GetProjectileNormal()
	{
		Vector3 forward = new Vector3();
		CharacterController characterController;
		if ( Target.Components.TryGet( out characterController ) )
		{
			forward = ((Target.Transform.Position + (characterController.Velocity)/2 ) - Transform.Position);
		}
		else
		{
			forward = (Target.Transform.Position - Transform.Position);
		}


		return forward;
	}
}
