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


    [Title("Feedback")]
    [SerializeField]
    private ParticleSystem jumpParticleSystem;


    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        maxCrouchTime = crouchTime / 60f;
        currentCrouchTime = 0f;
        //character.Movement.SpeedY = 0;
    }

    public override void UpdateState(CharacterBase character)
    {
        currentCrouchTime += Time.deltaTime * character.MotionSpeed;
        if(currentCrouchTime >= maxCrouchTime)
        {
            if (character.Input.CheckAction(0, InputConst.Attack))
            {
                character.Movement.Jump(character.Movement.JumpForce * shortJumpForceMultiplier);
            }
            else if (character.Input.inputActionsUP.Count != 0)
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

            ParticleSystem particle = Instantiate(jumpParticleSystem, this.transform.position, Quaternion.Euler(0,0, Mathf.Atan2(character.Movement.SpeedX * character.Movement.Direction, character.Movement.SpeedY) * Mathf.Rad2Deg));
            Destroy(particle.gameObject, 0.5f);
        }
    }

    public override void LateUpdateState(CharacterBase character)
    {

    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}