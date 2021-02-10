using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    CharacterBase characterBase;


    [SerializeField]
    Animator animator;
    [SerializeField]
    CharacterMovement movement;

    [SerializeField]
    float speedDelta = 30;

    public enum ActualState
    {
        Idle,
        Knockback,
        Wallrun
    }
    ActualState actualState;

    // Start is called before the first frame update
    void Start()
    {
        characterBase.OnStateChanged += CheckState;
    }

    public void CheckState(CharacterState oldState, CharacterState newState)
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if(actualState == ActualState.Idle)
        {
            AnimationIdle();
        }
        else if (actualState == ActualState.Wallrun)
        {
            AnimationWallrun();
        }
        else if (actualState == ActualState.Knockback)
        {

        }
    }
    void AnimationIdle()
    {
        float speedT = movement.SpeedX / (movement.MaxSpeed - speedDelta);
        animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));

        if (movement.Direction == 1)
            animator.transform.localScale = Vector3.one;
        else if (movement.Direction == -1)
            animator.transform.localScale = new Vector3(1, 1, -1);
    }
    void AnimationWallrun()
    {
        if (movement.SpeedY > 0)
        {
            animator.SetTrigger("Wallrun");
        }
        else
        {
            animator.SetTrigger("Hanging");
        }
    }

    void OnDestroy()
    {
        characterBase.OnStateChanged -= CheckState;
    }
}
