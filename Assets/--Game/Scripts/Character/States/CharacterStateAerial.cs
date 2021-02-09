using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateAerial : CharacterState
{

    [SerializeField]
    CharacterRigidbody characterRigidbody;
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState wallRunState;
    [SerializeField]
    CharacterMovement movement;

    [SerializeField]
    float minimalSpeedToWallRun = -2;

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



    // Start is called before the first frame update
    void Start()
    {
        currentNumberOfAerialJump = numberOfAerialJump;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StartState(CharacterBase character)
    {
        Debug.Log("AerialState");
        //if (currentNumberOfJump == 0 && characterRigidbody.IsGrounded)
        //{
        //	currentNumberOfJump = numberOfJump;
        //}

        //if (currentNumberOfJump > 0)
        //{
        //	currentNumberOfJump--;
        //	movement.SpeedY = jumpForce;
        //}
    }

    public override void UpdateState(CharacterBase character)
    {
        AirControl(character);

        if (character.Input.inputActions.Count != 0 && currentNumberOfAerialJump > 0)
        {
            if (character.Input.inputActions[0].action == InputConst.Jump)
            {
                currentNumberOfAerialJump--;
                movement.Jump(jumpForce);
                character.Input.inputActions[0].timeValue = 0;
            }
        }
        movement.ApplyGravity();
        characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, movement.SpeedY);


        if (characterRigidbody.CollisionGroundInfo != null)
        {
            character.SetState(idleState);
            return;
        }
        if (characterRigidbody.CollisionWallInfo != null && Mathf.Abs(movement.SpeedX) > 2)
        {
            character.SetState(wallRunState);
            //wallrunCount = 0;
            return;
        }
    }

    private void AirControl(CharacterBase character)
    {
        float axisX = character.Input.horizontal;

        float aerialDirection;

        if (movement.Direction > 0)
            aerialDirection = axisX;
        else
            aerialDirection = -axisX;

        movement.SpeedX = ((movement.SpeedX * airFriction) + (airControl * aerialDirection));
        //    //movement.Direction = (int)Mathf.Sign(axisX);
        //    // Walk vitesse constante
        //    if (movement.SpeedX < maxAerialSpeed)
        //    {
        //        movement.SpeedX += (movement.Acceleration * aerialDirection) * Time.deltaTime;
        //    }
        //    else
        //    {
        //        movement.SpeedX = maxAerialSpeed;
        //    }

        
        //if (movement.SpeedX > (movement.MaxSpeed))
        //{
        //	movement.SpeedX = (movement.MaxSpeed);
        //}

        //characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, -10);
    }

    public override void EndState(CharacterBase character)
    {
        currentNumberOfAerialJump = numberOfAerialJump;
        /*if (currentNumberOfAerialJump == 0 && characterRigidbody.IsGrounded)
		{
			currentNumberOfAerialJump = numberOfAerialJump;
		}*/
    }
}