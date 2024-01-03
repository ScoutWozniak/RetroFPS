using Sandbox;

public sealed class KeyPickupComponent : Component, IItemPickup
{
	[Property] KeyTypes KeyType { get;set; }
	public void OnPickup(GameObject go)
	{
		//var weapon = SceneUtility.Instantiate( WeaponPrefab );
		//weapon.SetParent(Scene.Components.Get<PlayerWeaponManagerComponent>(FindMode.EverythingInDescendants).GameObject, false);
		go.Components.Get<KeyInvComponent>( FindMode.InParent ).UpdateKey(KeyType, true);
		Sound.Play( "quakeitem.pickup" );
	}
}
