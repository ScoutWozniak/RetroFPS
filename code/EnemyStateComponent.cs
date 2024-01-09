using Sandbox;
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

	protected override void OnEnabled()
	{
		base.OnEnabled();
		LastAttack = AttackRate;

		// Temp
		if (Target == null)
		{

		}
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

	void StateIdle()
	{
		var rotForward = Transform.Rotation.Forward;
		var playerForward = (Target.Transform.Position - Transform.Position).Normal;

		if(Vector3.Dot( rotForward, playerForward ) > 0)
		{
			State = EnemyState.E_SEARCHING;
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

		var forward = (Target.Transform.Position - Transform.Position).Normal;
		CC.Velocity = forward * MoveSpeed;
		

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
			var projectile = SceneUtility.Instantiate( ProjectilePrefab );
			projectile.Transform.Position = GetProjectileSpawn();
			projectile.Transform.Rotation = Rotation.LookAt( GetProjectileNormal() );

			State = EnemyState.E_IDLE;
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
