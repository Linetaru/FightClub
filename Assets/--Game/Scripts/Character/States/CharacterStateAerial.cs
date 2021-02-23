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
    AttackManager attack;



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
                character.Movement.Jump(jumpForce);
                character.Input.inputActions[0].timeValue = 0;
            }
        }
        character.Movement.ApplyGravity();


        // Placeholder
        if (character.Input.inputActions.Count != 0)
        {
            if (character.Input.inputActions[0].action == InputConst.Attack)
            {
                character.Action.Action(attack);
                character.Input.inputActions[0].timeValue = 0;
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
            if (character.Rigidbody.CollisionWallInfo.gameObject.layer == 15 && Mathf.Abs(character.Input.horizontal) > .9 
                && Mathf.Sign(character.Input.horizontal) == Mathf.Sign(character.Movement.Direction)
                && Mathf.Abs(character.Movement.SpeedX) > minimalSpeedToWallRun)
                character.SetState(wallRunState);
            return;
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
        else if(character.Movement.SpeedX <= -maxAerialSpeed)
        {
            character.Movement.SpeedX = -maxAerialSpeed;
        }

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

    public override void EndState(CharacterBase character, CharacterState oldState)
    {
        Debug.Log("Aerial End");
        currentNumberOfAerialJump = numberOfAerialJump;
        /*if (currentNumberOfAerialJump == 0 && characterRigidbody.IsGrounded)
		{
			currentNumberOfAerialJump = numberOfAerialJump;
		}*/
    }
}