using Sandbox;

public sealed class ThrowableWeapon : Component, Component.ICollisionListener
{
	//int Bounces = 0;
	[Property] GameObject DropPrefab { get; set; }
	public void OnCollisionStart( Collision other )
	{
		Log.Info( "test" );
		if (other.Other.GameObject.Components.TryGet<HealthComponent>(out var health))
		{
			health.Hurt( 35.0f );
		}
		if (DropPrefab != null)
			DropPrefab.Clone(Transform.Position);

		GameObject.Destroy();
	}

	public void OnCollisionStop( CollisionStop other )
	{
		Log.Info( "test" );
	}

	public void OnCollisionUpdate( Collision other )
	{
		
	}

	protected override void OnUpdate()
	{
	}
}
