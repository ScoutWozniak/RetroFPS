using Sandbox;
using Sandbox.Utility;
using System.Threading;
using System.Threading.Tasks;

public sealed class DoorComponent : Component, IUsable
{
	[Property] Vector3 MoveDir { get;set; }

	[Property] float MoveSpeed { get;set; }

	[Property] float MoveLength { get;set; }

	[Property] bool Opened { get;set; }

	[Property] KeyTypes KeyNeeded { get;set; }

	Vector3 OriginalPos { get;set; }

	[Property] bool CloseAfterOpen { get;set; }
	[Property] float CloseAfter { get;set; }

	// HACK
	int curOpen { get; set; }

	public void OnUse(bool value, GameObject go )
	{
		if(value)
			OpenDoor();
		else
			CloseDoor();
	}

	void OpenDoor()
	{
		if ( HasKey() )
		{
			Opened = true;
			_ = LerpPosition( MoveSpeed, OriginalPos + MoveDir * MoveLength, Easing.EaseOut );
			curOpen++;
		}
	}

	void CloseDoor()
	{
		Opened = false;
		_ = LerpPosition( MoveSpeed, OriginalPos, Easing.EaseOut );
	}

	protected override void OnEnabled()
	{
		base.OnEnabled();
		OriginalPos = Transform.Position;
	}

	async Task LerpPosition(float seconds, Vector3 to, Easing.Function easer)
	{
		TimeSince timeSince = 0;
		Vector3 from = Transform.Position;

		while (timeSince < seconds)
		{
			var pos = Vector3.Lerp( from, to, easer( timeSince / seconds ) );
			Transform.Position = pos;
			
			await Task.Frame();
		}

		if (CloseAfterOpen)
			_ = CloseAfterSec(curOpen);
	}

	async Task CloseAfterSec(int _curOpen)
	{
		await Task.DelaySeconds( CloseAfter );
		if (Opened && curOpen == _curOpen )
		{
			_ = LerpPosition( MoveSpeed, OriginalPos, Easing.EaseOut );
		}
	}

	bool HasKey()
	{
		if ( KeyNeeded != KeyTypes.None && !Scene.Components.Get<KeyInvComponent>( FindMode.InDescendants ).HasKeyType( KeyNeeded ) )
		{
			Scene.Components.Get<PlayerHudComponent>( FindMode.InDescendants ).AddText( "You need the " + KeyTypes.PurpleKey.ToString() );
			return false;
		}
		return true;
	}
}
