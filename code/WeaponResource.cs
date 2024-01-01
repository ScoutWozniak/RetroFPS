using Sandbox;
using Editor;
using System;

[GameResource("Weapon Resource", "weapon", "All the required data for a weapon to be used")]
public partial class WeaponResource : GameResource
{
	public string Name { get; set; }

	public PrefabFile WeaponPrefab { get; set; }

	public AmmoType Ammo { get; set; }
	public int StartingAmmo { get; set; }

	public int weaponSlot { get; set; }
}
