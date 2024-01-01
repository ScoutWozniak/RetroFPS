using Sandbox;
using System;

public sealed class PlayerWeaponManagerComponent : Component
{
	GameObject[] weapons = new GameObject[9];
	WeaponResource[] weaponResources = new WeaponResource[9];

	[Property] int ActiveWeapon { get; set; }

	public int[] AmmoAmmount { get; set; }


	protected override void OnStart()
	{
		base.OnStart();
		AmmoAmmount = new int[Enum.GetNames( typeof( AmmoType ) ).Length];
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

			weapons[weaponResource.weaponSlot] = go;
			SetActiveWeapon(weaponResource.weaponSlot);
			Log.Info( ActiveWeapon );
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
			Log.Info( ActiveWeapon );
			ActiveWeapon = slot;
		}
	}

	public int GetActiveAmmo()
	{
		if ( weaponResources[ActiveWeapon] != null && AmmoAmmount[(int)weaponResources[ActiveWeapon].Ammo] != 0 )
			return AmmoAmmount[(int)weaponResources[ActiveWeapon].Ammo];
		else
			return 0;
	}

	public void AddAmmo( AmmoType type, int ammount )
	{
		AmmoAmmount[(int)type] += ammount;
	}
}

public enum AmmoType
{
	Pistol,
	Rifle
}
