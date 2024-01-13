using Sandbox;

public sealed class RepulsorSpell : Component, ISpell
{
	[Property] GameObject RepulsorPrefab { get; set; }

	[Property] float PersonalIntensity { get; set; }
	public void CastSpell()
	{
		Vector3 ProjPos = Transform.Position + Transform.Rotation.Forward * 100.0f;
		var tr = Scene.Trace.Ray(Transform.Position, ProjPos).WithoutTags("playerhitbox").Run();
		if(tr.Hit)
		{ ProjPos = tr.HitPosition; }	
		var go = RepulsorPrefab.Clone( ProjPos );
		go.Transform.Rotation = Transform.Rotation;

		if (GameObject.Components.TryGet<CharacterController>(out var cc, FindMode.InAncestors)) { 
			// Launch the player only if they aren't on the ground
			if (!cc.IsOnGround)
			{
				cc.Velocity -= Transform.Rotation.Forward * PersonalIntensity;
			}

		}
	}

	protected override void OnUpdate()
	{

	}
}
