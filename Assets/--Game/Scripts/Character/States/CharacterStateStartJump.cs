using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStartJump : CharacterState
{
    [SerializeField]
    private CharacterState jumpState;

    [SerializeField]
    private Animator animator;


    [SerializeField]
    private AnimationClip animationName;
    public AnimationClip AnimationName
    {
        get { return animationName; }
    }

    [SerializeField]
    private float shortJumpForce;
    public float ShortJumpForce
    {
        get { return shortJumpForce; }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        character.Movement.SpeedY = 0;
    }

    public override void UpdateState(CharacterBase character)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName.name))
        {
            if (character.Input.inputActionsUP.Count != 0)
            {
                if (character.Input.inputActionsUP[0].action == InputConst.Jump)
                {
                    character.Movement.Jump(shortJumpForce);
                }
            }
            else
            {
                character.Movement.Jump();
            }
            character.SetState(jumpState);
        }
    }

    public override void LateUpdateState(CharacterBase character)
    {

    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}