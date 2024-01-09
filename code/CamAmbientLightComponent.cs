using Sandbox;

public sealed class CamAmbientLightComponent : Component
{
	protected override void OnEnabled()
	{
		base.OnEnabled();
		Scene.SceneWorld.AmbientLightColor = Color.Blue;
	}
}
