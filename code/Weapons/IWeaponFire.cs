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
		var tr = Scene.Trace.Ray( startPos, endPos ).WithoutTags( "playerhitbox", "trigger", "enemycollider" ).UseHitboxes().Run();
		if ( tr.Hit )
		{
			GameObject hitObject = tr.GameObject;

			Log.Info( hitObject );
			
			if ( hitObject != null )
			{
				var healthComp = hitObject.Components.Get<HealthComponent>();
				if ( healthComp != null )
				{
					healthComp.Hurt( Damage );
				}

				// Impulse the rigidbodies
				if (hitObject.Components.TryGet<Rigidbody>(out var rb))
				{
					Log.Info( "hey" );
					rb.Velocity -= tr.Normal * 50.0f;
				}
			}

			SpawnParticles( tr.HitPosition, tr.Normal, (tr.Hitbox != null) );

			if (HitSound != null)
				Sound.Play( HitSound, tr.HitPosition );
		}
	}

	void SpawnParticles(Vector3 pos, Vector3 normal, bool hitFlesh)
	{
		if ( ParticleEffect == null )
			return;

		var particle = ParticleEffect.Clone();
		particle.Transform.Position = pos + normal * 2.0f;
		particle.Transform.Rotation = Rotation.LookAt( normal );

		if (hitFlesh)
			particle.Components.Get<ParticleEffect>().Tint = Color.Red;
	}
}
