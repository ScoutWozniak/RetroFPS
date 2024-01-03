using Sandbox;

public sealed class EnemyStateComponent : Component
{
	public enum EnemyState
	{
		E_IDLE,
		E_SEARCHING,
		E_RANGED,
		E_MELEE
	}

	[Property] EnemyState State { get; set; }
	protected override void OnUpdate()
	{

	}
}
