using Sandbox;

public sealed class SpellArmComponent : Component
{
	[Property] SkinnedModelRenderer ViewModel { get; set; }
	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		if ( Input.Pressed( "attack2" ) )
			CastSpells();
			
	}

	void CastSpells()
	{
		ViewModel.Set( "b_fire", true );
		var spell = Components.Get<ISpell>();
		spell.CastSpell();
	}
}

public interface ISpell
{
	public void CastSpell();
}
