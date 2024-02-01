using Sandbox;
using System;
using System.Collections.Generic;

public sealed class PlayerWeaponManagerComponent : Component
{
	GameObject[] weapons = new GameObject[9];
	WeaponResource[] weaponResources = new WeaponResource[9];

	[Property] int ActiveWeapon { get; set; }

	public int[] AmmoAmmount { get; set; }

	[Property] List<WeaponResource> GiveOnStart { get; set; }

	[Property] GameObject ThrowablePrefab { get; set; }

	WeaponData WeaponFireComponent { 
		get { 
			if ( weapons[ActiveWeapon] != null )
				return weapons[ActiveWeapon].Components.Get<WeaponData>();
			return null;
		}

	}


	protected override void OnStart()
	{
		base.OnStart();
		AmmoAmmount = new int[Enum.GetNames( typeof( AmmoType ) ).Length];

		foreach(var weapon in GiveOnStart)
		{
			AddWeapon(weapon );
		}
	}
	protected override void OnUpdate()
	{
		// Weapon Switching
		for (int i = 0; i < 9; i++)
		{
			if (Input.Pressed("Slot" + (i + 1).ToString())) {
				SetActiveWeapon( i );
			}
		}
	}

	public void AddWeapon(WeaponResource weaponResource)
	{
		if ( weapons[weaponResource.weaponSlot] == null )
		{
			var prefab = SceneUtility.GetPrefabScene( weaponResource.WeaponPrefab );
			var go = SceneUtility.Instantiate( prefab );
			go.SetParent( GameObject, false );
			go.Transform.LocalPosition = Vector3.Zero;
			go.Components.Get<WeaponData>().SetupStats( weaponResource );

			weapons[weaponResource.weaponSlot] = go;
			SetActiveWeapon(weaponResource.weaponSlot);
			weaponResources[weaponResource.weaponSlot] = weaponResource;
		}
		AddAmmo( weaponResource.Ammo, weaponResource.StartingAmmo );

		Scene.Components.Get<PlayerHudComponent>( FindMode.EverythingInSelfAndDescendants ).AddText( $"You picked up a {weaponResource.Name}!" );
	}

	public void SetActiveWeapon(int slot)
	{
		if ( weapons[slot] != null && slot != ActiveWeapon )
		{
			weapons[slot].Enabled = true;
			if ( weapons[ActiveWeapon] != null )
				weapons[ActiveWeapon].Enabled = false;
			//Log.Info( ActiveWeapon );
			ActiveWeapon = slot;
		}
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		// Throw the weapon - We can't throw the knife!
		if (Input.Pressed("Drop") && ActiveWeapon != 0)
		{
			ThrowActiveWeapon();
		}
	}

	public int GetActiveAmmo()
	{
		if ( WeaponFireComponent != null && AmmoAmmount[(int)WeaponFireComponent.Ammo] != 0 )
			return AmmoAmmount[(int)WeaponFireComponent.Ammo];
		else
			return 0;
	}

	public void AddAmmo( AmmoType type, int ammount )
	{
		AmmoAmmount[(int)type] += ammount;
	}

	void ThrowActiveWeapon()
	{
		
		weapons[ActiveWeapon].Destroy();
		weapons[ActiveWeapon] = null;
		SetActiveWeapon( 0 );

		// SPAWN HACK 
		var go = ThrowablePrefab.Clone( Scene.Camera.Transform.Position );
		go.Components.Get<Rigidbody>().Velocity =  Scene.Camera.Transform.Rotation.Forward * 750.0f;
	}
}

public enum AmmoType
{
	None,
	Pistol,
	Rifle
}
