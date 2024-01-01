using Sandbox;

public sealed class ViewmodelComponent : Component
{
	[Property] string FireGraphValue { get; set; }
	protected override void OnStart()
	{
		base.OnStart();
	}

	protected override void OnEnabled()
	{
		base.OnEnabled();
	}

	public void FireEffects()
	{
		GameObject.Components.Get<SkinnedModelRenderer>().Set( FireGraphValue, true );
	}
}
