using Sandbox;

public sealed class ProjectileComponent : Component, Component.ITriggerListener
{
	[Property] float Speed { get; set; }

	[Property] float Damage { get; set; }
	public void OnTriggerEnter( Collider other )
	{
		HealthComponent health;
		if (other.GameObject.Components.TryGet( out health))
		{
			health.Hurt( Damage );
			GameObject.Destroy();
		}
	}

	public void OnTriggerExit( Collider other )
	{
		throw new System.NotImplementedException();
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		var movement = Transform.Rotation.Forward * Speed;
		Transform.Position += movement;
	}
}
