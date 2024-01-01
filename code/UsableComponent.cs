using Sandbox;

public sealed class UsableComponent : Component
{
	public void OnUse()
	{
		foreach(var comp in GameObject.Components.GetAll<IUsable>( FindMode.EverythingInSelf ))
		{
			comp.OnUse();
		}
	}
}

public interface IUsable
{
	public void OnUse();
}
