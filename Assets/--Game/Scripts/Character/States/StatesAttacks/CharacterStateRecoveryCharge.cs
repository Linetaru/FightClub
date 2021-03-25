using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRecoveryCharge : CharacterState
{
    [SerializeField]
    CharacterState recoveryState;

    [SerializeField]
    GameObject chargeParticle;

    int joystickPositionX = 0;
    int joystickPositionY = 0;

    Vector2 joystickAngle = new Vector2(0f, 0f);

    [SerializeField]
    float chargeDuration = .5f;

    [SerializeField]
    float recoverySpeed = 3;


    float timer = 0f;
    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        chargeParticle.SetActive(true);
        timer = 0.0f;
        character.Movement.SetSpeed(0.0f, 0.0f);
    }

    public override void UpdateState(CharacterBase character)
    {
        timer += Time.deltaTime;

        if (character.Input.horizontal > .3)
            joystickPositionX = 1;
        else if (character.Input.horizontal < -.3)
            joystickPositionX = -1;
        else
            joystickPositionX = 0;

        if (character.Input.vertical > .3)
            joystickPositionY = 1;
        else if (character.Input.vertical < -.3)
            joystickPositionY = -1;
        else
            joystickPositionY = 0;

        if (timer >= chargeDuration)
        {
            character.SetState(recoveryState);
        }
    }

    public override void LateUpdateState(CharacterBase character)
    {
        character.Movement.ApplyGravity(.1f);
    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {
        chargeParticle.SetActive(false);
        if (Mathf.Abs(character.Input.horizontal) < .3 && Mathf.Abs(character.Input.vertical) < .3)
        {
            character.Movement.SetSpeed(0, recoverySpeed);
            return;
        }
        if (joystickPositionX != 0)
            character.Movement.Direction = joystickPositionX;
        character.Movement.SetSpeed(character.Movement.Direction * joystickPositionX * recoverySpeed, joystickPositionY * recoverySpeed);
    }
}