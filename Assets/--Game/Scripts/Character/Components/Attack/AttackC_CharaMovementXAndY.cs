using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_CharaMovementXAndY : AttackComponent
{
	[SerializeField]
	float startForceTime = 0.2f;

	[SerializeField]
	float initialSpeedY;

	[SerializeField]
	float initialSpeedX;

	[SerializeField]
	bool speedOnce = false;
	/*[SerializeField]
	AnimationCurve curveSpeedY;
	[SerializeField]
	float finalSpeedY;*/


	float t = 0f;
	bool once = false;


	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
	{

	}

	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
	{
		t += Time.deltaTime;
		if (t >= startForceTime)
		{
			if (speedOnce && once)
				return;
			user.Movement.SpeedY = initialSpeedY;
			user.Movement.SpeedX = initialSpeedX;
			once = true;
		}
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
