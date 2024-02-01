using Sandbox;

public sealed class CamAmbientLightComponent : Component
{
	protected override void OnEnabled()
	{
		base.OnEnabled();
		var cam = Components.Get<CameraComponent>();
		
	}
}
