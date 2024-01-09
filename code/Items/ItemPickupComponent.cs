using Sandbox;

public sealed class ItemPickupComponent : Component, Component.ITriggerListener
{
	[Property] bool Rotate { get; set; }
	public void OnTriggerEnter( Collider other )
	{
		//var weapon = SceneUtility.Instantiate( WeaponPrefab );
		//weapon.SetParent(Scene.Components.Get<PlayerWeaponManagerComponent>(FindMode.EverythingInDescendants).GameObject, false);
		foreach ( var comp in GameObject.Components.GetAll<IItemPickup>() )
		{
			comp.OnPickup(other.GameObject);
		}

		GameObject.Destroy();
	}

	public void OnTriggerExit( Collider other ) {}

	protected override void OnUpdate()
	{
		if (Rotate)
			Transform.Rotation = Transform.Rotation * new Angles( 0.0f, 1.0f, 0.0f ).ToRotation();
	}
}


public interface IItemPickup
{
	public void OnPickup(GameObject go);
}
public sealed class WeaponPickupComponent : Component, IItemPickup
{
	[Property] GameObject WeaponPrefab { get; set; }
	[Property] WeaponResource WeaponData { get; set; }
	public void OnPickup(GameObject go)
	{
		//var weapon = SceneUtility.Instantiate( WeaponPrefab );
		//weapon.SetParent(Scene.Components.Get<PlayerWeaponManagerComponent>(FindMode.EverythingInDescendants).GameObject, false);
		go.Components.Get<PlayerWeaponManagerComponent>(  ).AddWeapon( WeaponData );
		Sound.Play( "quakeitem.pickup" );
	}
}

