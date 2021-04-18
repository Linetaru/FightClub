﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    CharacterBase characterBase;

    [SerializeField]
    GameObject animatorPivot;

    [SerializeField]
    Animator animator;
    [SerializeField]
    CharacterMovement movement;
    bool isHanging = false;    bool canDeccelerate = false;    bool isDeccelerating = false;    bool parryBlow = false;    float previousSpeedT = 0;

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
        animator.ResetTrigger("Parry");

        if (newState is CharacterStateDeath)
        {
            animator.SetTrigger("Idle");
        }

        if (newState is CharacterStateIdle)
        {
            animator.SetTrigger("Idle");
            actualState = ActualState.Idle;
        }
        if (newState is CharacterStateDash)
        {
            animator.SetFloat("Speed", 1);
            //animator.SetTrigger("Idle");
            //actualState = ActualState.Idle;
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

        if (newState is CharacterStateHomingDash)
        {
            animator.SetTrigger("Idle");
            animator.SetTrigger("HomingDash");
        }

        if (newState is CharacterStateTurnAround)
        {
            animator.SetTrigger("TurnAround");
        }

        if (newState is CharacterStateParry)
        {
            animator.SetTrigger("Parry");
        }

        if (newState is CharacterStateParryBlow)
        {
            actualState = ActualState.ParryBlow;
            //animator.SetTrigger("Knockback");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.Direction == 1)
            animatorPivot.transform.localScale = Vector3.one;
        else if (movement.Direction == -1)
            animatorPivot.transform.localScale = new Vector3(-1, 1, 1);
        if (actualState == ActualState.Idle)
        {
            AnimationIdle();
        }
        else if (actualState == ActualState.Wallrun)
        {
            AnimationWallrun();
        }


        else if (actualState == ActualState.ParryBlow)
        {
            AnimationParryBlow();
        }
    }
    void AnimationIdle()
    {
        float speedT = movement.SpeedX / movement.SpeedMax;
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
        float speedT = movement.SpeedY / movement.SpeedMax;
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


    void AnimationParryBlow()
    {
        if (characterBase.MotionSpeed == 0)
            return;
        if (parryBlow == false)
        {
            animator.SetTrigger("Knockback");
            parryBlow = true;
        }
    }
    void OnDestroy()
    {
        characterBase.OnStateChanged -= CheckState;
    }
}
