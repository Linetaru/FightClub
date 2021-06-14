using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Status : AttackComponent
{
	[Title("Status on User")]
	[SerializeField]
	StatusInstance[] statusUser;


	[Title("Status on Hit")]
	[SerializeField]
	StatusInstance[] statusHit;

	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
    {
		for (int i = 0; i < statusUser.Length; i++)
		{
			user.Status.AddStatus(statusUser[i].CreateStatus());
		}
    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		for (int i = 0; i < statusHit.Length; i++)
		{
			target.Status.AddStatus(statusHit[i].CreateStatus());
		}
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
		
    }
}
