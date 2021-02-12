using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    // 
    protected bool canJumpCancel = false;
    protected bool canMoveCancel = false;

    // Utilisé pour gérer le bug d'animation event si on cancel frame perfect
    protected bool endAction = false;
    protected bool canEndAction = false;

    protected CharacterBase character;
    protected AttackManager attackID; // Les combo/target combo partage le meme attackID
    protected AttackManager currentAttackManager;

    [SerializeField]
    Animator animator;

    // C'est naze faut pas faire ça
    [SerializeField]
    CharacterState stateAction;
    [SerializeField]
    CharacterState stateIdle;
    [SerializeField]
    CharacterState stateAerial;

    public void InitializeComponent(CharacterBase c)
    {
        character = c;
    }


    private bool CanAct()
    {
        if (currentAttackManager != null && canMoveCancel == false)
            return false;
        return true;
    }

    private AttackManager CheckCombo(AttackManager attack)
    {
        if (currentAttackManager != null)
        {
            if (attack == attackID)
            {
                if (currentAttackManager.AtkCombo != null)
                {
                    return currentAttackManager.AtkCombo;
                }
            }
        }
        return attack;
    }






    public bool Action(AttackManager attack)
    {
        if (CanAct() == false)
            return false;

        endAction = false;
        canEndAction = false;
        canMoveCancel = false;

        // Combo
        AttackManager attackToInstantiate = CheckCombo(attack);
        attackID = attack;

        // Animation de l'attaque
        //animator.ResetTrigger("Idle");
        animator.Play(attackToInstantiate.AttackAnim.name, 0, 0f);

        // On créer l'attaque et ça setup différent paramètres
        if (currentAttackManager != null)
             currentAttackManager.CancelAction();
        currentAttackManager = Instantiate(attackToInstantiate, this.transform.position, Quaternion.identity);
        currentAttackManager.CreateAttack(character);

        character.SetState(stateAction);
        return true;
    }


    public void CancelAction()
    {
        if (currentAttackManager != null)
            currentAttackManager.CancelAction();
        currentAttackManager = null;
        attackID = null;

        canMoveCancel = false;
        canEndAction = false;
        endAction = false;

        /*animator.SetTrigger("Idle");*/
        if(character.Rigidbody.IsGrounded)
            character.SetState(stateIdle);
        else
            character.SetState(stateAerial);
    }







    // Appelé par les anims
    public void ActionActive()
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionActive();
        }
    }

    // Appelé par les anims
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

    // Appelé par les anims
    // active le bool pour Cancel l'action à la frame suivante
    public void EndAction()
    {
        if (canEndAction == true)
        {
            endAction = true;
        }
    }







    // Appelé par le State pour gérer les cancel d'animation frame perfect
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






    public void SetAttackMotionSpeed(float newValue)
    {
        animator.speed = newValue;
    }

}
