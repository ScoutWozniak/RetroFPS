using Sandbox;
using System.Collections.Generic;
using System;
public sealed class ButtonComponent : Component, IUsable
{
	[Property] public List<GameObject> ToTrigger { get; set; }
	[Property] bool TriggerOnce { get; set; } = true;

	[Property] public Action OnPressed { get; set; }


	bool Used = false;
	public void OnUse(bool value, GameObject go )
	{
		if ( TriggerOnce && !Used || !TriggerOnce )
		{
			//OnPressed?.Invoke();
			Used = true;
			Sound.Play( "button.pressed", Transform.Position );
			TriggerAll();
		}
	}

	public void OnToggled(bool value)
	{ }

	protected override void OnUpdate()
	{

	}

	public void TriggerAll()
	{
		foreach ( GameObject g in ToTrigger )
		{
			foreach ( var comp in g.Components.GetAll<IUsable>() )
			{
				comp.OnUse( true, g );
			}
		}
	}

}
