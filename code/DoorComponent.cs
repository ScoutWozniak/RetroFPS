using Sandbox;
using Sandbox.Utility;
using System.Threading.Tasks;

public sealed class DoorComponent : Component, IUsable
{
	[Property] Vector3 MoveDir { get;set; }

	[Property] float MoveSpeed { get;set; }

	[Property] float MoveLength { get;set; }

	[Property] bool Opened { get;set; }

	[Property] KeyTypes KeyNeeded { get;set; }

	public void OnUse()
	{
		//Transform.Position = Transform.Position + MoveDir * MoveLength;
		if ( KeyNeeded != KeyTypes.None && !Scene.Components.Get<KeyInvComponent>( FindMode.InDescendants ).HasKeyType( KeyNeeded ) )
		{
			Scene.Components.Get<PlayerHudComponent>( FindMode.InDescendants ).AddText( "You need the " + KeyTypes.PurpleKey.ToString() );
			return;
		}
		Opened = true;
		_ = LerpPosition( MoveSpeed, Transform.Position + MoveDir * MoveLength, Easing.EaseOut );
	}

	protected override void OnUpdate()
	{

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
	}
}
