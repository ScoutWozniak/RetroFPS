using Sandbox;
using System.Threading.Tasks;

public sealed class WeaponFireComponent : Component
{
	public enum InputListenerType
	{
		Pressed,
		Down,
		Released
	}
	[Property] AmmoType Ammo { get; set; }
	[Property] int AmmoUsage { get; set; }

	[Property] SoundEvent FireSound { get; set; }

	[Property] public InputListenerType InputType { get; set; } = InputListenerType.Down;

	[Property] float FireDelay { get; set; } = 1.0f;

	[Property] ViewmodelComponent Viewmodel { get; set; }

	[Property] GameObject HitParticles { get; set; }

	[Property] GameObject FireLight { get; set; }

	TimeSince SinceFire { get; set; }
	protected override void OnUpdate()
	{
		if (GetFireType("attack1") && SinceFire >= FireDelay)
		{
			var ammoInv = GameObject.Components.Get<PlayerWeaponManagerComponent>( FindMode.EverythingInSelfAndAncestors );
			if ( ammoInv.AmmoAmmount[(int)Ammo] > 0 )
			{
				ammoInv.AmmoAmmount[(int)Ammo] -= AmmoUsage;
				Fire();
				SinceFire = 0;
			}
		}
	}

	void Fire()
	{
		var cam = Scene.Components.Get<CameraComponent>( FindMode.EverythingInDescendants );
		if (cam != null )
		{
			FireBullet( cam.Transform.Position, cam.Transform.Position + cam.Transform.Rotation.Forward * 1000.0f );
			FireEffects();


		}
	}

	void FireBullet(Vector3 startPos, Vector3 endPos)
	{
		var tr = Scene.Trace.Ray( startPos, endPos ).WithoutTags("playerhitbox").UseHitboxes().Run();
		if (tr.Hit)
		{
			SpawnHitParticles( tr.HitPosition, tr.Normal );
			if ( tr.GameObject != null )
			{
				var healthComp = tr.GameObject.Components.Get<HealthComponent>( FindMode.EnabledInSelfAndChildren );
				if ( healthComp != null )
				{
					healthComp.Hurt( 10 );
				}
			}
			
		}
	}

	void FireEffects()
	{
		Sound.Play( FireSound, Transform.Position );
		Viewmodel.FireEffects();
		FireLight.Enabled = true;
		Scene.Components.Get<ScreenShakeComponent>( FindMode.InDescendants ).AddShake(0.25f,0.1f);
	}

	bool GetFireType(string action)
	{
		if ( InputType == InputListenerType.Pressed )
			return Input.Pressed( action );
		if ( InputType == InputListenerType.Down )
			return Input.Down( action );

		return Input.Released( action );
	}

	void SpawnHitParticles(Vector3 pos, Vector3 forward)
	{
		
		var particles = SceneUtility.Instantiate( HitParticles, pos + forward * 2.0f );
		particles.Transform.Rotation = Rotation.LookAt( forward );
		Log.Info( particles );

		var light = SceneUtility.Instantiate( FireLight, Transform.Position );
		Log.Info( light );
	}
}
