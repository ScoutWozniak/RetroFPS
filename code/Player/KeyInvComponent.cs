using Sandbox;
using System.Collections.Generic;

public sealed class KeyInvComponent : Component
{
	Dictionary<KeyTypes, bool> HasKey;
	protected override void OnStart()
	{
		base.OnStart();
		HasKey = new Dictionary<KeyTypes, bool>();

		HasKey.Add(KeyTypes.PurpleKey, false);
		HasKey.Add(KeyTypes.RedKey, false);
	}
	
	public bool HasKeyType(KeyTypes key)
	{
		return HasKey.ContainsKey(key) && HasKey[key] == true;
	}

	public void UpdateKey(KeyTypes key, bool value)
	{
		HasKey[key] = value;
	}
}

public enum KeyTypes
{
	None,
	PurpleKey,
	RedKey
} 
