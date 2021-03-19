using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Dash : AttackComponent
{
    [SerializeField]
    private bool isChargeDash = false;
    [SerializeField] 
    [ShowIf("isChargeDash")] 
    private AttackManager dashAttackHit;
    [SerializeField]
	private float dashSpeed = 20f;

    private CharacterBase currentUser;

	// Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
        currentUser = user;
	}

    private void Update()
    {
        if(currentUser != null)
        { 
            if(GetComponent<BoxCollider>().enabled)
            {
                currentUser.Movement.SpeedX = dashSpeed;
            }
            else
            {
                currentUser.Movement.Decelerate();
            }
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
        if(dashAttackHit != null)
        {
            user.Action.CancelAction();
            user.Action.Action(dashAttackHit);
        }
        else
        {
            user.Action.FinishAction();
        }
    }
	
	// Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
		
    }
}
