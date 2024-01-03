using Sandbox;

public sealed class HealthComponent : Component
{
	[Property] public int Health { get; set; } = 100;
	[Property] public int Armour { get; set; } = 0;
	[Property] SoundEvent HurtSound { get; set; }
	[Property] bool RemoveOnDeath { get; set; } = true;

	public void Hurt(int damage)
	{
		Health -= damage;
		

		if (Health <= 0)
		{
			foreach(var comp in GameObject.Components.GetAll<IDeathEvent>())
			{
				comp.OnDeath();
			}
			if (RemoveOnDeath)
				GameObject.Destroy();
		}
		else
		{
			if ( HurtSound != null )
			{
				Sound.Play( HurtSound, Transform.Position );
			}
		}
	}
}
