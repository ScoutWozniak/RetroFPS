using Sandbox;
using System.Collections.Generic;

public sealed class UsableComponent : Component
{
	public void OnUse(GameObject go)
	{
		foreach(var comp in GameObject.Components.GetAll<IUsable>( FindMode.EverythingInSelf ))
		{
			comp.OnUse(true, go);
		}
	}
}

public sealed class TriggerComponent : Component, Component.ITriggerListener
{
	[Property] List<GameObject> ToTrigger { get; set; }

	[Property] bool TriggerOnce { get; set; }
	[Property] TriggerStyle Style { get; set; }

	enum TriggerStyle
	{
		OnEnter,
		OnExit,
		Both
	}

	bool Used { get; set; }
	public void OnTriggerEnter( Collider other )
	{
		if ( CheckNotUsed( other.Tags ) && (Style == TriggerStyle.OnEnter || Style == TriggerStyle.Both ))
		{
			Trigger(true, other.GameObject);
		}
	}

	void Trigger(bool value, GameObject gObject)
	{
		if ( ToTrigger.Count <= 0 )
			return;
		foreach ( GameObject go in ToTrigger )
		{
			foreach ( var comp in go.Components.GetAll<IUsable>() )
			{
				comp.OnUse( value, gObject );
			}
		}

		Used = true;
	}

	bool CheckNotUsed(ITagSet tags) { return ((TriggerOnce && !Used || !TriggerOnce) && tags.Has("playerhitbox")); }

	public void OnTriggerExit( Collider other ) {
		if ( CheckNotUsed(other.Tags) && (Style == TriggerStyle.OnExit || Style == TriggerStyle.Both) )
		{
			Trigger(false, other.GameObject);
		}
	}
}

public interface IUsable
{
	public void OnUse(bool value, GameObject go);
}

public sealed class LightToggleComponent : Component, IUsable
{
	public void OnUse(bool value, GameObject go )
	{
		var light = Components.Get<PointLight>(FindMode.EverythingInSelf);
		if ( light != null )
		{
			light.Enabled = !light.Enabled;
			Log.Info( light.Enabled );
		}
	}
}

public sealed class TextPrintComponent : Component, IUsable
{
	PlayerHudComponent PlayerHud;
	[Property] string Text { get; set;  }
	[Property] bool PrintOnce { get; set; }


	bool Printed { get; set; }
	protected override void OnEnabled()
	{
		base.OnEnabled();
		PlayerHud = Scene.Components.Get<PlayerHudComponent>( FindMode.InDescendants );
	}
	public void OnUse(bool value, GameObject go)
	{
		if ( PrintOnce && !Printed || !PrintOnce )
		{
			PlayerHud.AddText( Text, true );
			Printed = true;
		}
	}
}

public sealed class HurtObjectComponent : Component, IUsable
{
	public void OnUse( bool value, GameObject go )
	{
		go.Components.Get<HealthComponent>( FindMode.EverythingInSelfAndParent ).Hurt(10);
	}
}
