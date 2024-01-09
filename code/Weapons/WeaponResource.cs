using Sandbox;
using Editor;
using System;

[GameResource("Weapon Resource", "weapon", "All the required data for a weapon to be used")]
public partial class WeaponResource : GameResource
{
	public string Name { get; set; }

	public PrefabFile WeaponPrefab { get; set; }

	public bool UsesAmmo { get; set; } = true;

	[Category("Ammo")] public AmmoType Ammo { get; set; }
	[Category( "Ammo" )] public int StartingAmmo { get; set; }
	[Category( "Ammo" )] public int weaponSlot { get; set; }
}
