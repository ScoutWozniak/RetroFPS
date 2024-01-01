using Sandbox;
using System.Threading.Tasks;

public sealed class DestroyAfterComponent : Component
{
	[Property] float Seconds { get; set; }
	protected override void OnEnabled()
	{
		base.OnEnabled();
		_ = DestroyAfter( Seconds );
	}

	async Task DestroyAfter(float seconds)
	{
		await Task.DelaySeconds(seconds);
		GameObject.Destroy();
	}
}
