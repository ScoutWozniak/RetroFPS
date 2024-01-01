using Sandbox;

public sealed class WeaponPickupComponent : Component, Component.ITriggerListener
{
	[Property] GameObject WeaponPrefab { get; set; }
	[Property] WeaponResource WeaponData { get; set; }
	public void OnTriggerEnter( Collider other )
	{
		//var weapon = SceneUtility.Instantiate( WeaponPrefab );
		//weapon.SetParent(Scene.Components.Get<PlayerWeaponManagerComponent>(FindMode.EverythingInDescendants).GameObject, false);
		Scene.Components.Get<PlayerWeaponManagerComponent>( FindMode.EverythingInDescendants ).AddWeapon( WeaponData );
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
