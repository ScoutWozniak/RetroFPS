using Sandbox;
using System;
using System.Collections.Generic;

public sealed class AmmoInvComponent : Component
{

	public int[] AmmoAmmount { get;set; }
	protected override void OnStart()
	{
		base.OnStart();
		AmmoAmmount = new int[Enum.GetNames( typeof(AmmoType) ).Length];
	}
	public void AddAmmo(AmmoType type, int ammount)
	{
		AmmoAmmount[(int)type] += ammount;
	}
}
