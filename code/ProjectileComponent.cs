using Sandbox;

public sealed class ProjectileComponent : Component, Component.ITriggerListener
{
	[Property] float Speed { get; set; }

	[Property] float Damage { get; set; }
	public void OnTriggerEnter( Collider other )
	{
		HealthComponent health;
		if (other.GameObject.Components.TryGet( out health, FindMode.EverythingInSelfAndDescendants))
		{
			health.Hurt( Damage );
			Log.Info( health.Health );
			GameObject.Destroy();
		}
	}

	public void OnTriggerExit( Collider other )
	{

	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		var movement = Transform.Rotation.Forward * Speed;
		Transform.Position += movement;
	}
}
