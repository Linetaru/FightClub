using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_GuardDash : AttackComponent
{
	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
	{
		user.Knockback.Parry.IsGuardDash = true;
	}
	
	// Appelé tant que l'attaque existe 
	public override void UpdateComponent(CharacterBase user)
    {
		user.Knockback.Parry.IsGuardDash = true;
	}
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		
    }

	public override void OnParry(CharacterBase user, CharacterBase target)
	{

	}

	public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
	{

	}

	public override void OnClash(CharacterBase user, CharacterBase target)
	{

	}

	// Appelé au moment de la destruction de l'attaque
	public override void EndComponent(CharacterBase user)
    {
		user.Knockback.Parry.IsGuardDash = false;
	}
}
