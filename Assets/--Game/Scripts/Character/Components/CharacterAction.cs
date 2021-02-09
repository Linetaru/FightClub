using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{

    protected bool canMoveCancel = false;
    protected bool endAction = false;
    protected bool canEndAction = false;


    protected AttackController currentAttackController;


    public bool CanAct()
    {
        return true;
    }

    public void Action(AttackController attack)
    {
        endAction = false;
        canEndAction = false;
        canMoveCancel = false;

        // Animation de l'attaque
        //animator.ResetTrigger("Idle");
        //animator.Play(currentAttackController.AttackAnimation.name, 0, 0f);

        // On créer l'attaque et ça setup différent paramètres
       /* if (currentAttackController != null)
            currentAttackController.CancelAction();*/
        currentAttackController = Instantiate(attack, this.transform.position, Quaternion.identity);
        //currentAttackController.CreateAttack(attack, character);
    }

    // Appelé par les anims, active l'attaque
    public void ActionActive()
    {
        if (currentAttackController != null)
        {
           // currentAttackController.ActionActive();
        }
    }

    // Appelé par les anims
    // Créer une subaction de l'attaque (Si l'attaque n'a pas de subaction, ne fais rien)
    public void SubAction(int nb)
    {
        if (currentAttackController != null)
        {
            //currentAttackController.SubAction(nb);
        }
    }

    // Appelé par les anims
    public void MoveCancelable()
    {
        canMoveCancel = true;
    }

    // Appelé par les anims, active le bool pour Cancel l'action à la frame suivante
    public void EndAction()
    {
        if (canEndAction == true)
        {
            endAction = true;
        }
    }


    public void CanEndAction()
    {
        if (canEndAction == false)
            canEndAction = true;
    }

    // Cancel l'action si le bool end action est toujours validé
    public void EndActionState()
    {
        if (endAction == true)
        {
            CancelAction();
            //OnActionEnd.Invoke();
        }
    }


    public void CancelAction()
    {
       /* if (currentAttackController != null)
            currentAttackController.CancelAction();*/
        currentAttackController = null;

        canMoveCancel = false;
        canEndAction = false;
        endAction = false;

        /*animator.SetTrigger("Idle");*/
    }

}
