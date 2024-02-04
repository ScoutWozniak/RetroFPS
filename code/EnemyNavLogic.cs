using Sandbox;

public sealed class EnemyNavLogic : Component
{
	[Property] NavMeshAgent NavAgent;
	protected override void OnUpdate()
	{
		NavAgent.MoveTo( Vector3.Zero );
	}
}
