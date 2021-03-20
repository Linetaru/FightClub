using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateAerial : CharacterState
{
    [Title("States")]
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState wallRunState;
    //[SerializeField]
    //CharacterState recoveryChargeState;



    [SerializeField]
    float minimalSpeedToWallRun = 8;

    [Title("Moveset")]
    [SerializeField]
    CharacterMoveset characterMoveset;
    [SerializeField]
    CharacterEvasiveMoveset evasiveMoveset;

    [SerializeField]
    GameObject doubleJumpParticle;

    bool isFastFall = false;




    public override void StartState(CharacterBase character, CharacterState oldState)
    {
    }

    public override void UpdateState(CharacterBase character)
    {

        character.Movement.AirControl(character.Input.horizontal);

        if (isFastFall)
            character.Movement.SpeedY = character.Movement.GravityMax * 0.75f;
        else
            character.Movement.ApplyGravity();


        if (character.Movement.SpeedY <= 0 && !isFastFall)
        {
            if (character.Input.vertical < -0.9f)
                isFastFall = true;
        }

        if (characterMoveset.ActionAttack(character) == true)
        {

        }
        else if (evasiveMoveset.Dodge(character) == true)
        {

        }
        else if (character.Input.inputActions.Count != 0)
        {
            /*if(character.Input.inputActions[0].action == InputConst.Special && character.Input.vertical > .8f)
            {
                character.SetState(recoveryChargeState);
            }*/

            if (character.Movement.CurrentNumberOfJump > 0)
            {
                if (character.Input.inputActions[0].action == InputConst.Jump)
                {
                    character.Input.inputActions[0].timeValue = 0;

                    GameObject jumpRippleEffect = Instantiate(doubleJumpParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
                    Destroy(jumpRippleEffect, 2.0f);
                    character.Movement.CurrentNumberOfJump--;
                    character.Movement.Jump();

                    if (character.Input.horizontal != 0)
                    {
                        int newDirection = (int)Mathf.Sign(character.Input.horizontal);
                        if (newDirection != character.Movement.Direction)
                            character.Movement.SpeedX *= -1;
                        character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
                    }
                }
            }
        }
    }


    /// <summary>
	/// Update après le check de collision
    /// </summary>
    /// <param name="character"></param>
    public override void LateUpdateState(CharacterBase character)
    {
        if (character.Rigidbody.IsGrounded == true)
        {
            character.SetState(idleState);
            return;
        }
        if (character.Rigidbody.CollisionWallInfo != null)
        {
            if (character.Rigidbody.CollisionWallInfo.gameObject.layer == 15)
            {
                if (Mathf.Abs(character.Input.horizontal) > .9 && Mathf.Sign(character.Input.horizontal) == Mathf.Sign(character.Movement.Direction) && Mathf.Abs(character.Movement.SpeedX) > minimalSpeedToWallRun)
                    character.SetState(wallRunState);
            }
            character.Movement.SpeedX = 0;
            return;
        }
        else if (character.Rigidbody.CollisionRoofInfo != null) // ------------ On tombe
        {
            character.Movement.SpeedY = 0;
            character.Movement.ApplyGravity();
        }
    }



    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        isFastFall = false;
    }
}