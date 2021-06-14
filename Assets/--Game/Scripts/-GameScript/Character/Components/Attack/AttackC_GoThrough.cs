using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_GoThrough : AttackComponent
{
	[SerializeField]
	float delay;

	[SerializeField]
	LayerMask horizontalLayerMask;

	float t;

	// Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
		if(delay <= 0)
			user.Rigidbody.SetNewLayerMask(horizontalLayerMask);

	}
	
	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		if(delay >= 0)
		{
			t += Time.deltaTime;
			if (t > delay)
			{
				user.Rigidbody.SetNewLayerMask(horizontalLayerMask);
				t = -999999999;
			}
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
		user.Rigidbody.ResetLayerMask();
	}
}
