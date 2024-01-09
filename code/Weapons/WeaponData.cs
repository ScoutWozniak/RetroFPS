using Sandbox;
using System.Threading.Tasks;

public sealed class WeaponData : Component
{
	public enum InputListenerType
	{
		Pressed,
		Down,
		Released
	}
	public AmmoType Ammo { get; set; }
	int AmmoUsage { get; set; }

	bool NeedsAmmo { get; set; }

	[Property] SoundEvent FireSound { get; set; }

	[Property] public InputListenerType InputType { get; set; } = InputListenerType.Down;

	[Property] float FireDelay { get; set; } = 1.0f;

	[Property] ViewmodelComponent Viewmodel { get; set; }

	TimeSince SinceFire { get; set; }
	protected override void OnUpdate()
	{
		if (GetFireType("attack1") && SinceFire >= FireDelay)
		{
			
			if ( CanFire() )
			{
				UseAmmo();
				foreach (var fireEffect in Components.GetAll<IWeaponFireEvent>())
				{
					fireEffect.OnFire();
				}

				Viewmodel.FireEffects();
			}

			SinceFire = 0;
		}
	}

	protected override void OnStart()
	{
		base.OnStart();
		SinceFire = FireDelay; // HACK
	}

	bool GetFireType(string action)
	{
		if ( InputType == InputListenerType.Pressed )
			return Input.Pressed( action );
		if ( InputType == InputListenerType.Down )
			return Input.Down( action );

		return Input.Released( action );
	}

	bool CanFire()
	{
		var ammoInv = GameObject.Components.Get<PlayerWeaponManagerComponent>( FindMode.EverythingInSelfAndAncestors );

		return (ammoInv.AmmoAmmount[(int)Ammo] > 0 && NeedsAmmo) || !NeedsAmmo;
	}

	void UseAmmo()
	{
		if ( !NeedsAmmo )
			return;
		var ammoInv = GameObject.Components.Get<PlayerWeaponManagerComponent>( FindMode.EverythingInSelfAndAncestors );
		ammoInv.AmmoAmmount[(int)Ammo] -= 1;
	}

	public void SetupStats(WeaponResource weapon)
	{
		Ammo = weapon.Ammo;
		NeedsAmmo = weapon.UsesAmmo;
	}
}
