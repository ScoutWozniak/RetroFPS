using Sandbox;

public sealed class Repulsor : Component, Component.ITriggerListener
{
	public void OnTriggerEnter( Collider other )
	{
		var dif = other.Transform.Position;
		if(other.GameObject.Components.TryGet<CharacterController>(out var cc) && !other.Tags.Has("playerhitbox"))
		{
			Log.Info( "test" );
			cc.Velocity = Transform.Rotation.Forward * 750.0f + Vector3.Up * 200.0f;
		}

		if ( other.GameObject.Components.TryGet<Rigidbody>( out var rb ) )
		{
			rb.Velocity += Transform.Rotation.Forward * 1000.0f;
		}

		if ( other.GameObject.Components.TryGet<Rigidbody>( out var proj ) )
		{
			rb.Transform.Rotation = Rotation.LookAt( -rb.Transform.Rotation.Forward );
		}
	}

	public void OnTriggerExit( Collider other ) {}
}
