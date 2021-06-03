using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    CharacterBase characterBase;
    [SerializeField]
    GameObject animatorPivot;
    [SerializeField]
    Animator animator;    [Title("Animations")]
    [SerializeField]
    AnimationClip animationParry;
    [SerializeField]
    AnimationClip animationParryAerial;
    [SerializeField]
    AnimationClip animationAcumod;    bool isHanging = false;    bool canDeccelerate = false;    bool isDeccelerating = false;    bool parryBlow = false;    float previousSpeedT = 0;

    public enum ActualState
    {
        Null,
        Idle,
        Knockback,
        Wallrun,        StartJump,
        ParryBlow
    }
    ActualState actualState;

    // Start is called before the first frame update
    void Start()
    {
        characterBase.OnStateChanged += CheckState;
    }
    // Si le cast n'est pas performant ou qu'on a trop de sous state, ajouter des tag sur les states pour les identifier
    public void CheckState(CharacterState oldState, CharacterState newState)
    {
        actualState = ActualState.Null;

        canDeccelerate = false;
        isDeccelerating = false;
        parryBlow = false;

        animator.SetBool("Hanging", false);
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Fall");
        animator.ResetTrigger("Wallrun");
        animator.ResetTrigger("Knockback");
        animator.ResetTrigger("Crouch");
        animator.ResetTrigger("Deccelerate");
        animator.ResetTrigger("TurnAround");
        //animator.ResetTrigger("Parry");

        characterBase.CenterPivot.localRotation = Quaternion.identity;

        if (newState is CharacterStateDeath)
        {
            animator.SetTrigger("Idle");
            animator.SetFloat("Speed", 0);
        }

        if (newState is CharacterStateIdle)
        {
            animator.SetTrigger("Idle");
            actualState = ActualState.Idle;
        }
        if (newState is CharacterStateDash)
        {
            animator.SetTrigger("Idle");
            animator.SetFloat("Speed", 1);
            //animator.SetTrigger("Idle");
            //actualState = ActualState.Idle;
        }
        if (newState is CharacterStateDashEnd)
        {
            animator.SetTrigger("Deccelerate");
        }
        if (newState is CharacterStateAerial)
        {
            animator.SetTrigger("Fall");
        }
        if (newState is CharacterStateWallRun)
        {
            animator.SetTrigger("Wallrun");
            actualState = ActualState.Wallrun;
        }
        if (newState is CharacterStateKnockback)
        {
            animator.SetTrigger("Knockback");
            actualState = ActualState.Knockback;
        }
        if(newState is CharacterStateStartJump || newState is CharacterStateLanding)
        {
            animator.SetTrigger("Crouch");
        }

        if (newState is CharacterStateDodge)
        {
            animator.SetTrigger("Dodge");
        }

        if (newState is CharacterStateDodgeAerial)
        {
            animator.SetTrigger("DodgeAerial");
        }

        if (newState is CharacterStateTurnAround)
        {
            animator.SetTrigger("TurnAround");
        }

        if (newState is CharacterStateParry)
        {
            if(characterBase.Rigidbody.IsGrounded)
                animator.Play(animationParry.name);
            else
                animator.Play(animationParryAerial.name);
        }

        if (newState is CharacterStateParrySuccess && !(oldState is CharacterStateActing))
        {
            if (characterBase.Rigidbody.IsGrounded)
                animator.Play(animationParry.name);
            else
                animator.Play(animationParryAerial.name);
        }

        if (newState is CharacterStateParryBlow)
        {
            actualState = ActualState.ParryBlow;
        }

        if (newState is CharacterStateAcumod)
        {
            animator.Play(animationAcumod.name);
        }

        if (newState is CharacterStateBurst)
        {
            animator.Play(animationAcumod.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (characterBase.Movement.Direction == 1)
            animatorPivot.transform.localScale = Vector3.one;
        else if (characterBase.Movement.Direction == -1)
            animatorPivot.transform.localScale = new Vector3(-1, 1, 1);
        if (actualState == ActualState.Idle)
        {
            AnimationIdle();
        }
        else if (actualState == ActualState.Wallrun)
        {
            AnimationWallrun();
        }

        else if (actualState == ActualState.Knockback)
        {
            AnimationKnockback();
        }

        else if (actualState == ActualState.ParryBlow)
        {
            AnimationParryBlow();
        }
    }
    void AnimationIdle()
    {
        float speedT = characterBase.Movement.SpeedX / characterBase.Movement.SpeedMax;
        animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));
        if (speedT >= 1)
        {
            canDeccelerate = true;
        }
        else if (speedT < 1 && canDeccelerate == true)
        {
            animator.SetTrigger("Deccelerate");
            canDeccelerate = false;
            isDeccelerating = true;
            previousSpeedT = speedT;
        }

        if(isDeccelerating == true)
        {
            if (speedT == 0)
            {
                animator.SetTrigger("Idle");
                isDeccelerating = false;
            }
            if (speedT > previousSpeedT)
            {
                animator.SetTrigger("Idle");
                isDeccelerating = false;
            }
            previousSpeedT = speedT;
        }

    }
    void AnimationWallrun()
    {
        float speedT = characterBase.Movement.SpeedY / characterBase.Movement.SpeedMax;
        animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));
        if(speedT < 0)
        {
            animator.SetBool("Hanging", true);
        }
        else
        {
            animator.SetBool("Hanging", false);
        }
    }

    void AnimationKnockback()
    {
        characterBase.CenterPivot.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(new Vector2(characterBase.Movement.SpeedX, characterBase.Movement.SpeedY), Vector2.left * characterBase.Movement.Direction));
    }


    void AnimationParryBlow()
    {
        if (characterBase.MotionSpeed == 0)
            return;
        if (parryBlow == false)
        {
            animator.SetTrigger("Knockback");
            parryBlow = true;
        }
        AnimationKnockback();
    }
    void OnDestroy()
    {
        characterBase.OnStateChanged -= CheckState;
    }
}
