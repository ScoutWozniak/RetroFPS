using Sandbox;

public sealed class Repulsor : Component, Component.ITriggerListener
{
	public void OnTriggerEnter( Collider other )
	{
		if(other.GameObject.Components.TryGet<CharacterController>(out var cc) && !other.Tags.Has("playerhitbox"))
		{
			var dif = cc.Transform.Position - Transform.Position;

			cc.Velocity += dif.Normal * 1000.0f;
		}

		if ( other.GameObject.Components.TryGet<Rigidbody>( out var rb ) )
		{
			var dif = rb.Transform.Position - Transform.Position;

			rb.Velocity += dif.Normal * 1000.0f;
		}
	}

	public void OnTriggerExit( Collider other ) {}
}
