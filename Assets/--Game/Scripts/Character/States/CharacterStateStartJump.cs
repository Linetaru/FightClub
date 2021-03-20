using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateStartJump : CharacterState
{

    [Title("States")]
    [SerializeField]
    CharacterState jumpState;

    [Title("Parameter")]
    [SerializeField]
    [SuffixLabel("en frames")]
    float crouchTime = 10f;

    float maxCrouchTime = 0f;
    float currentCrouchTime = 0f;

    [SerializeField]
    [Range(0, 1)]
    private float shortJumpForceMultiplier = 0.5f;


    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        maxCrouchTime = crouchTime / 60f;
        currentCrouchTime = 0f;
        //character.Movement.SpeedY = 0;
    }

    public override void UpdateState(CharacterBase character)
    {
        currentCrouchTime += Time.deltaTime;
        if(currentCrouchTime >= maxCrouchTime)
        {
            if (character.Input.inputActionsUP.Count != 0)
            {
                if (character.Input.inputActionsUP[0].action == InputConst.Jump)
                {
                    character.Movement.Jump(character.Movement.JumpForce * shortJumpForceMultiplier);
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