using System;
using System.Linq;
using Sandbox;



public sealed class SpellArmComponent : Component
{

	float startScale = 0.20f; 
	float endScale = 1f; 

	float startBrightness = 1f; 
	float endBrightness = 30f; 
	[Property] SkinnedModelRenderer ViewModel { get; set; }
	protected override void OnFixedUpdate()
	{
   		Color startColor = Color.Red;
    	Color endColor = Color.FromRgb(0x112233);

		
		var spell = Components.Get<ISpell>();

		var spellFarticlesRenderer = GameObject.Components.GetInDescendants<ParticleSpriteRenderer>();
		var spellFarticlesEffect = GameObject.Components.GetInDescendants<ParticleEffect>();

		//makes da spell effect decrease when spell is on cooldown
		spellFarticlesRenderer.Scale = MathX.Lerp(startScale, endScale, 1f - (spell.currentCooldown / spell.castCooldown));
		spellFarticlesEffect.Brightness = MathX.Lerp(startBrightness, endBrightness, 1f - (spell.currentCooldown / spell.castCooldown));
		Color interpolatedColor = Color.Lerp(startColor, endColor, 1f - (spell.currentCooldown / spell.castCooldown));

		spellFarticlesEffect.Tint = interpolatedColor;

		if ( Input.Pressed( "attack2" ) )

			
			CastSpells();
			
	}

	void CastSpells()
	{
		var spell = Components.Get<ISpell>();
		
		if (spell.canCast())
		{
		spell.CastSpell();
		ViewModel.Set( "b_fire", true );
		}
		else ViewModel.Set("b_cooldown", true);
		

	}
}

public interface ISpell
{
	public void CastSpell();
	public bool canCast();
	float currentCooldown { get; }
    float castCooldown { get; }
	float cooldownPercentage { get; }
}
