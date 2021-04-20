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

    [Title("WallRun")]
    [SerializeField]
    LayerMask wallRunLayerMask;


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
                if (character.Input.inputActions[0].action == InputConst.Jump || character.Input.CheckAction(0, InputConst.Smash))
                {
                    character.Input.inputActions[0].timeValue = 0;

                    GameObject jumpRippleEffect = Instantiate(doubleJumpParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
                    Destroy(jumpRippleEffect, 2.0f);
                    character.Movement.CurrentNumberOfJump--;

                    character.Movement.Jump();

                    if (Mathf.Abs(character.Input.horizontal) < 0.25f)
                    {
                        character.Movement.SpeedX = 0;
                    }
                    else
                    {
                        character.Movement.SpeedX = character.Movement.MaxAerialSpeed * Mathf.Abs(character.Input.horizontal);
                        character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
                    }

                    /*if (character.Input.horizontal != 0)
                    {
                        int newDirection = (int)Mathf.Sign(character.Input.horizontal);
                        if (newDirection != character.Movement.Direction)
                        {
                            if(Mathf.Sign(character.Movement.SpeedX * newDirection) != character.Movement.Direction)
                                character.Movement.SpeedX *= 0;
                            else
                                character.Movement.SpeedX *= -1;
                                //character.Movement.SpeedX += (character.Movement.MaxAerialSpeed * 0.5f * Mathf.Abs(character.Input.horizontal));
                        }
                        character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
                    }*/
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
        if (character.Rigidbody.CollisionWallInfo.Collision != null)
        {
            // On ne peut s'accrocher que si le raycast tiré du bas du perso est true
            if (character.Rigidbody.CollisionWallInfo.Collision.gameObject.layer == 15 || character.Rigidbody.CollisionWallInfo.Collision.gameObject.layer == 17)
            {
                // On check bien qu'on touche en entier un mur
                for (int i = 0; i < character.Rigidbody.CollisionWallInfo.Contacts.Length; i++)
                {
                    if(character.Rigidbody.CollisionWallInfo.Contacts[i] == false)
                    {
                        character.Movement.SpeedX = 0;
                        return;
                    }
                }
                if (Mathf.Abs(character.Input.horizontal) > .2) // && Mathf.Sign(character.Input.horizontal) == Mathf.Sign(character.Movement.Direction))
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