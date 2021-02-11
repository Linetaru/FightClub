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
    float speedDelta = 30;    bool isHanging = false;

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
        if (oldState is CharacterStateWallRun)
        {
            isHanging = false;
            animator.transform.rotation = Quaternion.Euler(0, 90, 0);
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

            if (movement.SpeedY > 0)
            {
                isHanging = false;
                animator.SetTrigger("Wallrun");                if (movement.Direction == 1)
                    animator.transform.rotation = Quaternion.Euler(-90, 90, 0);

                else if (movement.Direction == -1)
                    animator.transform.rotation = Quaternion.Euler(90, 90, 0);
            }

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
        if (movement.Direction == 1)
            animator.transform.localScale = Vector3.one;
        else if (movement.Direction == -1)
            animator.transform.localScale = new Vector3(1, 1, -1);
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

        }
    }
    void AnimationIdle()
    {
        float speedT = movement.SpeedX / (movement.MaxSpeed - speedDelta);
        animator.SetFloat("Speed", Mathf.Clamp(speedT, 0, 1));

    }
    void AnimationWallrun()
    {
        if (movement.SpeedY < 0)
        {
            if (!isHanging)
            {
                animator.transform.rotation = Quaternion.Euler(0, 90, 0);
                animator.SetTrigger("Hanging");                isHanging = true;
            }
        }
    }

    void OnDestroy()
    {
        characterBase.OnStateChanged -= CheckState;
    }
}
