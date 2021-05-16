using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Applique une force fixe indépendant des stats de gravité du joueur
public class AttackC_CharaMovementForce : AttackComponent
{
	[SerializeField]
	float startForceTime = 0.2f;

	[SerializeField]
	float initialSpeedY;
	/*[SerializeField]
	AnimationCurve curveSpeedY;
	[SerializeField]
	float finalSpeedY;*/


	float t = 0f;


	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
    {

    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		t += Time.deltaTime;
		if(t>=startForceTime)
		{
			user.Movement.SpeedY = initialSpeedY;
		}
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
		
    }
}
