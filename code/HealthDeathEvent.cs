using Sandbox;
using System.Collections.Generic;

public sealed class DeathSoundEvent : Component, IDeathEvent
{
	[Property] SoundEvent DeathSound { get; set; }

	public void OnDeath()
	{
		if (DeathSound != null)
			Sound.Play( DeathSound, Transform.Position );
	}
}

public sealed class DeathExplosionEvent : Component, IDeathEvent
{
	[Property] GameObject Prefab { get; set; }
	[Property] int SpawnAmmount { get; set; }

	public void OnDeath()
	{
		if ( Prefab != null )
		{
			for (int i = 0; i < SpawnAmmount; i++)
			{
				var go = Prefab.Clone( Transform.Position );
				go.Enabled = true;
				go.Transform.Position += Vector3.Up * 50.0f;
			}
		}
	}
}

public sealed class DeathTriggerEvent : Component, IDeathEvent
{
	[Property] List<GameObject> ToTrigger { get; set; }
	public void OnDeath()
	{
		foreach ( GameObject g in ToTrigger )
		{
			foreach ( var comp in g.Components.GetAll<IUsable>() )
			{
				comp.OnUse( true, GameObject );
			}
		}
	}
}

public interface IDeathEvent
{
	public void OnDeath();
}
