using Sandbox;
using System.Collections.Generic;
public sealed class ButtonComponent : Component, IUsable
{
	[Property] List<GameObject> ToTrigger { get; set; }

	public void OnUse()
	{
		foreach(GameObject go in ToTrigger)
		{
			foreach(var comp in go.Components.GetAll<IUsable>()) { 
				comp.OnUse();
			}
		}
	}

	protected override void OnUpdate()
	{

	}


}
