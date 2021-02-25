using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(AudioSource))]
public class AttackC_Sounds : AttackComponent
{

	// Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
		
    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		
    }
	
	// Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		
    }
}
