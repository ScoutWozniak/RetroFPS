using Sandbox;

public sealed class HealthComponent : Component
{
	[Property] public int Health { get; set; } = 100;
	[Property] public int Armour { get; set; } = 0;
	[Property] SoundEvent HurtSound { get; set; }

	public void Hurt(int damage)
	{
		Health -= damage;
	}
}
