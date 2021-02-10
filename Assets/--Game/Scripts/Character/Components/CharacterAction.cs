using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{

    protected bool canMoveCancel = false;
    protected bool endAction = false;
    protected bool canEndAction = false;

    protected CharacterBase character;
    protected AttackManager currentAttackManager;

    [SerializeField]
    Animator animator;

    [SerializeField]
    CharacterState stateAction;
    [SerializeField]
    CharacterState stateIdle;

    public void InitializeComponent(CharacterBase c)
    {
        character = c;
    }


    public bool CanAct()
    {
        return true;
    }

    public void Action(AttackManager attack)
    {
        endAction = false;
        canEndAction = false;
        canMoveCancel = false;

        // Animation de l'attaque
        //animator.ResetTrigger("Idle");
        animator.Play(attack.AttackAnim.name, 0, 0f);

        // On créer l'attaque et ça setup différent paramètres
        /* if (currentAttackManager != null)
             currentAttackManager.CancelAction();*/
        currentAttackManager = Instantiate(attack, this.transform.position, Quaternion.identity);
        currentAttackManager.CreateAttack(character);

        character.SetState(stateAction);
    }

    // Appelé par les anims, active l'attaque
    public void ActionActive()
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionActive();
        }
    }

    public void ActionUnactive()
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionUnactive();
        }
    }

    // Appelé par les anims
    // Créer une subaction de l'attaque (Si l'attaque n'a pas de subaction, ne fais rien)
    public void SubAction(int nb)
    {
        if (currentAttackManager != null)
        {
            //currentAttackManager.SubAction(nb);
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
        if (currentAttackManager != null)
             currentAttackManager.CancelAction();
        currentAttackManager = null;

        canMoveCancel = false;
        canEndAction = false;
        endAction = false;

        /*animator.SetTrigger("Idle");*/
        character.SetState(stateIdle);
    }

}
