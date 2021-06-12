using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Grab : AttackComponent
{
	[HideInInspector]
	public CharacterBase grabbedTarget = null;

	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
    {
		grabbedTarget = null;
    }
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		if (grabbedTarget != null)
		{
			grabbedTarget.Rigidbody.transform.position = user.Rigidbody.transform.position;
		}
		
	}
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		target.Knockback.Launch(new Vector2(0, 0), 0);
		target.Movement.SetSpeed(0f, 0f);
		target.Movement.ResetAcceleration();
		if (grabbedTarget == null)
        {
            grabbedTarget = target;
			target.Knockback.KnockbackDuration = 1.5f;
        }
    }
	
	// Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
	{

	}
}
