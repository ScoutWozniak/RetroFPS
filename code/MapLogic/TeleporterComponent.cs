using System;
using System.Collections.Generic;
using Sandbox;


public sealed class TeleporterComponent : Component, IUsable
{
	[Property] bool SetRotation { get; set; } = false;
	[Property] bool RedirectVel { get; set; }
	[Property] float RedictMult { get; set; }
	[Property] bool RemoveVel { get; set; }
	public void OnUse( bool value, GameObject go )
	{
		Log.Info( go );
		go.Transform.Position = Transform.Position;
		var player = go.Components.Get<PlayerController>();
		if ( SetRotation )
		{
			//Special check for player!
			
			if ( player != null )
			{
				Log.Info( player.EyeAngles );
				player.SetEyeAngles( Transform.Rotation.Angles() );

			}
			go.Transform.Rotation = Transform.Rotation;
		}

		if ( RedirectVel && player != null )
			RedirectVelocity( player );

		if (RemoveVel && player != null)
			RemoveVelocity(player );

		Sound.Play( "tele.port", Transform.Position );
	}

	void RedirectVelocity(PlayerController player)
	{
		var cc = player.GameObject.Components.Get<CharacterController>();
		var velLength = cc.Velocity.Length;
		cc.Velocity = Transform.Rotation.Forward * velLength * RedictMult;
	}

	void RemoveVelocity(PlayerController player)
	{
		var cc = player.GameObject.Components.Get<CharacterController>();
		cc.Velocity = Vector3.Zero;
	}
}
