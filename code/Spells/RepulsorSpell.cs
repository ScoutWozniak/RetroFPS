using System.IO;
using Sandbox;

public sealed class RepulsorSpell : Component, ISpell
{
	[Property] GameObject RepulsorPrefab { get; set; }

	[Property] public float castCooldown { get; set; }
 	public float currentCooldown {get; set;}
	[Property] float PersonalIntensity { get; set; }

	public float cooldownPercentage => MathX.Clamp(1f - (currentCooldown / castCooldown), 0f, 1f);
	
	public void CastSpell()
	{
		
		if (currentCooldown > 0f) return;
		
		
		Vector3 ProjPos = Transform.Position + Transform.Rotation.Forward * 100.0f;
		var tr = Scene.Trace.Ray(Transform.Position, ProjPos).WithoutTags("playerhitbox").Run();
		if(tr.Hit)
		{ ProjPos = tr.HitPosition; }	
		var go = RepulsorPrefab.Clone( ProjPos );
		go.Transform.Rotation = Transform.Rotation;
	
		if (GameObject.Components.TryGet<CharacterController>(out var cc, FindMode.InAncestors)) 
		{ 
			// Launch the player only if they aren't on the ground
			if (!cc.IsOnGround)
			{
				cc.Velocity -= Transform.Rotation.Forward * PersonalIntensity;
			}
		}
		
		currentCooldown = castCooldown;
		
	}
	private void UpdateCooldown()
	{
    	if (currentCooldown > 0f)
    	{
        	currentCooldown -= Time.Delta;
	    }
	}

	public bool canCast()
	{
		return currentCooldown <= 0f;
	}

	protected override void OnUpdate()
	{

	}

	protected override void OnFixedUpdate()
	{
		//temporary debugging
		//Log.Info(currentCooldown);
		UpdateCooldown();
	}


}
