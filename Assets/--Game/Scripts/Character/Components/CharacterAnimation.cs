using System.Collections;
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

    [SerializeField]
    float wallRunOffset = 0.35f;    bool isHanging = false;

    public enum ActualState
    {
        Null,
        Idle,
        Knockback,
        Wallrun,        StartJump
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

        animator.SetBool("Hanging", false);
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Fall");
        animator.ResetTrigger("Wallrun");
        animator.ResetTrigger("Knockback");
        animator.ResetTrigger("Crouch");


        if (newState is CharacterStateDeath)
        {
            animator.SetTrigger("Idle");
        }

        if (newState is CharacterStateIdle)
        {
            animator.SetTrigger("Idle");
            actualState = ActualState.Idle;
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

        if (newState is CharacterStateDodgeAerial)
        {
            animator.SetTrigger("DodgeAerial");
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
    }
    void AnimationIdle()
    {
        float speedT = movement.SpeedX / movement.SpeedMax;
        animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));
    }
    void AnimationWallrun()
    {
        float speedT = movement.SpeedY / movement.SpeedMax;
        animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));
        if(speedT < 0)
        {
            //animatorPivot.transform.localPosition = Vector3.zero;
            //animatorPivot.transform.rotation = Quaternion.Euler(0, 90, 0);
            animator.SetBool("Hanging", true);
        }
        else
        {
            if (movement.Direction == 1)
            {
                //animatorPivot.transform.localPosition = new Vector3(wallRunOffset, 0, 0);
                //animatorPivot.transform.rotation = Quaternion.Euler(-90, 90, 0);
            }
            else if (movement.Direction == -1)
            {
                //animatorPivot.transform.localPosition = new Vector3(-wallRunOffset, 0, 0);
                //animatorPivot.transform.rotation = Quaternion.Euler(90, 90, 0);
            }
            animator.SetBool("Hanging", false);
        }
    }

    void OnDestroy()
    {
        characterBase.OnStateChanged -= CheckState;
    }
}
