using Sandbox;

public sealed class ViewmodelComponent : Component
{
	[Property] string FireGraphValue { get; set; }
	[Property] SkinnedModelRenderer Renderer { get; set; }
	protected override void OnUpdate()
	{
		base.OnUpdate();
		Transform.Position = Scene.Components.Get<CameraComponent>(FindMode.EverythingInDescendants).Transform.Position;
		Transform.Rotation = Scene.Components.Get<CameraComponent>(FindMode.EverythingInDescendants).Transform.Rotation;
	}

	public void FireEffects()
	{
		if (FireGraphValue != "")
			Renderer.Set( FireGraphValue, true );
	}
}
