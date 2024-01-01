using Sandbox;

public sealed class KeyPickupComponent : Component, Component.ITriggerListener
{
	[Property] KeyTypes KeyType { get;set; }
	public void OnTriggerEnter( Collider other )
	{
		//var weapon = SceneUtility.Instantiate( WeaponPrefab );
		//weapon.SetParent(Scene.Components.Get<PlayerWeaponManagerComponent>(FindMode.EverythingInDescendants).GameObject, false);
		Scene.Components.Get<KeyInvComponent>( FindMode.EverythingInDescendants ).UpdateKey(KeyType, true);
		Sound.Play( "quakeitem.pickup" );
		GameObject.Destroy();
	}

	public void OnTriggerExit( Collider other )
	{

	}

	protected override void OnUpdate()
	{
		Transform.Rotation = Transform.Rotation * new Angles( 0.0f, 1.0f, 0.0f ).ToRotation();
	}
}
