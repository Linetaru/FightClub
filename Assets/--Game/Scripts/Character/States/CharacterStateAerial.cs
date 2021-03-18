using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateAerial : CharacterState
{

    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState wallRunState;
    [SerializeField]
    CharacterState recoveryChargeState;

    [SerializeField]
    float minimalSpeedToWallRun = 8;

    [SerializeField]
    int numberOfAerialJump = 1;

    [SerializeField]
    [ReadOnly] int currentNumberOfAerialJump = 1;

    [SerializeField]
    float jumpForce = 10f;

    [SerializeField]
    float airControl = 1f;
    [SerializeField]
    float airFriction = 0.9f;
    [SerializeField]
    float maxAerialSpeed = 10f;

    [SerializeField]
    CharacterMoveset characterMoveset;
    [SerializeField]
    CharacterEvasiveMoveset evasiveMoveset;

    [SerializeField]
    GameObject doubleJumpParticle;

    bool isFastFall = false;



    // Start is called before the first frame update
    void Start()
    {
        currentNumberOfAerialJump = numberOfAerialJump;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
    }

    public override void UpdateState(CharacterBase character)
    {
        AirControl(character);
        if (isFastFall)
            character.Movement.ApplyGravity(2);
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
            if(character.Input.inputActions[0].action == InputConst.Special && character.Input.vertical > .8f)
            {
                character.SetState(recoveryChargeState);
            }

            if (currentNumberOfAerialJump > 0)
            {
                if (character.Input.inputActions[0].action == InputConst.Jump)
                {
                    GameObject jumpRippleEffect = Instantiate(doubleJumpParticle, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
                    Destroy(jumpRippleEffect, 2.0f);
                    currentNumberOfAerialJump--;
                    character.Movement.Jump(jumpForce);

                    if (character.Input.horizontal != 0)
                        character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);

                    character.Input.inputActions[0].timeValue = 0;
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
            if (Mathf.Abs(character.Input.horizontal) > .9
                && Mathf.Sign(character.Input.horizontal) == Mathf.Sign(character.Movement.Direction)
                && Mathf.Abs(character.Movement.SpeedX) > minimalSpeedToWallRun)
                character.SetState(wallRunState);

            character.Movement.SpeedX = 0;
            return;
        }
        else if (character.Rigidbody.CollisionRoofInfo != null) // ------------ On tombe
        {
            character.Movement.SpeedY = 0;
            character.Movement.ApplyGravity();
        }
    }



    private void AirControl(CharacterBase character)
    {
        float axisX = character.Input.horizontal;

        float aerialDirection;

        if (character.Movement.Direction > 0)
            aerialDirection = axisX;
        else
            aerialDirection = -axisX;

        character.Movement.SpeedX += (airControl * aerialDirection * airFriction) * Time.deltaTime;

        if (character.Movement.SpeedX >= maxAerialSpeed)
        {
            character.Movement.SpeedX = maxAerialSpeed;
        }
        else if (character.Movement.SpeedX <= -maxAerialSpeed)
        {
            character.Movement.SpeedX = -maxAerialSpeed;
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        currentNumberOfAerialJump = numberOfAerialJump;
        isFastFall = false;
    }
}