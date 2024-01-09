using Sandbox;
using System;
using System.Collections.Generic;
public interface IWeaponFireEvent
{
	public void OnFire();
}
public sealed class HitscanFireEvent : Component, IWeaponFireEvent
{
	[Property] float Range { get; set; }

	[Property] GameObject ParticleEffect { get; set; }

	[Property] TagSet Ignore { get; set; }

	[Property] SoundEvent HitSound { get; set; }
	[Property] SoundEvent FireSound { get; set; }

	[Property] float Damage { get; set; } = 10.0f;

	public void OnFire()
	{
		var cam = Scene.Components.Get<CameraComponent>( FindMode.EverythingInDescendants );
		if ( cam != null )
		{
			FireBullet( cam.Transform.Position, cam.Transform.Position + cam.Transform.Rotation.Forward * Range );
			if (FireSound != null)
			{
				Sound.Play( FireSound, Transform.Position );
			}
		}
	}

	void FireBullet( Vector3 startPos, Vector3 endPos )
	{
		var tr = Scene.Trace.Ray( startPos, endPos ).WithoutTags( "playerhitbox", "trigger" ).UseHitboxes().Run();
		if ( tr.Hit )
		{
			//SpawnHitParticles( tr.HitPosition, tr.Normal );
			if ( tr.GameObject != null )
			{
				var healthComp = tr.GameObject.Components.Get<HealthComponent>( FindMode.EnabledInSelfAndChildren );
				if ( healthComp != null )
				{
					healthComp.Hurt( Damage );
				}
			}
			SpawnParticles( tr.HitPosition, tr.Normal );

			if (HitSound != null)
				Sound.Play( HitSound, tr.HitPosition );
		}
	}

	void SpawnParticles(Vector3 pos, Vector3 normal)
	{
		if ( ParticleEffect == null )
			return;

		var particle = SceneUtility.Instantiate( ParticleEffect );
		particle.Transform.Position = pos + normal * 2.0f;
		particle.Transform.Rotation = Rotation.LookAt( normal );
	}
}
