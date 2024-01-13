using Sandbox;



public sealed class SpellArmComponent : Component
{
	[Property] SkinnedModelRenderer ViewModel { get; set; }
	protected override void OnFixedUpdate()
	{
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
		

	}
}

public interface ISpell
{
	public void CastSpell();
	public bool canCast();
}
