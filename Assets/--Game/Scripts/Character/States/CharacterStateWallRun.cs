using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateWallRun : CharacterState
{

    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    CharacterState aerialState;

    [SerializeField]
    CharacterRigidbody characterRigidbody;
    [SerializeField]
    CharacterMovement movement;

    [SerializeField]
    float stickRunThreshold = 0.7f;
    [SerializeField]
    float deccelerationRate = 0.7f;

    float wallrunSpeed = 10.0f;
    [SerializeField]
    float wallrunSpeedMax = 10.0f;
    [SerializeField]
    float wallrunSpeedMin = -2.0f;

    [SerializeField]
    float wallJumpSpeedX = 5.0f;

    float horizontalDeadZone = .1f;

    bool wallCollision = false;
    
    [SerializeField]
    LayerMask wallLayer;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StartState(CharacterBase character)
    {
        //wallCollision = true;
        Debug.Log("Wallrun");
        Debug.Log(movement.SpeedX);

        float speedXBeforeWallRun = movement.SpeedX;


        Debug.Log(speedXBeforeWallRun);

        wallrunSpeed = wallrunSpeedMax + speedXBeforeWallRun;
        if (movement.SpeedX > 0)
        {

            movement.SetSpeed(0.0f, wallrunSpeed/* + speedXBeforeWallRun*/);
        }
        else
        {
            if (characterRigidbody.IsGrounded)
            {
                character.SetState(idleState);
            }
            else
            {
                character.SetState(aerialState);
            }
        }
    }

    public override void UpdateState(CharacterBase character)
    {
        wallCollision = (Physics.Raycast(transform.position, Vector3.right*movement.Direction, out _, 1.0f, wallLayer));
        
        if (Mathf.Abs(character.Input.horizontal) > horizontalDeadZone && Mathf.Sign(character.Input.horizontal) == movement.Direction && wallCollision)
        {
            if (movement.SpeedY > wallrunSpeedMin)
            {
                wallrunSpeed -= deccelerationRate * Time.deltaTime;
                movement.SpeedY = wallrunSpeed;
            }
            else
            {
                movement.SpeedY = wallrunSpeedMin;
            }

            if (movement.SpeedY > 0)
            {
                //Play Wallrun animation
            }
            else
            {
                //Play WallSlide Animation (rester accroché au mur tout en se laissant tomber)
            }

            if (character.Input.inputActions.Count != 0 && wallCollision)
            {
                if (character.Input.inputActions[0].action == InputConst.Jump)
                {
                    if (movement.SpeedY > 0)
                        movement.SpeedX = wallJumpSpeedX + movement.SpeedY;
                    else
                        movement.SpeedX = wallJumpSpeedX;
                    movement.Direction = movement.Direction * -1;
                    movement.Jump();

                    //Play Walljump animation

                    character.SetState(aerialState);
                    character.Input.inputActions[0].timeValue = 0;
                    wallCollision = false;
                }
            }
        }
        else
        {
            Debug.Log("IS NO MORE IN WALLRUN");
            if (characterRigidbody.IsGrounded)
            {
                character.SetState(idleState);
            }
            else
            {
                character.SetState(aerialState);
            }
        }

        characterRigidbody.UpdateCollision(movement.SpeedX, movement.SpeedY);

        //if (character.Input.inputActions[0].action == InputConst.Jump)
        //{
        //    //movementJump.Jump();
        //}





        /*float axisX = Input.GetAxis("Horizontal");
		if (Mathf.Abs(axisX) > stickRunThreshold)
		{
			if(movement.SpeedX > 0)
			{
				characterRigidbody.UpdateCollision(movement.SpeedX * Mathf.Sign(axisX), movement.SpeedX);
				movement.SpeedX -= deccelerationRate * Time.deltaTime;
			}
			else
			{
				// On tombe
				movement.SpeedX -= deccelerationRate * Time.deltaTime;
				characterRigidbody.UpdateCollision(0, movement.SpeedX);
			}
		}
		else
		{
			characterRigidbody.UpdateCollision(0, 0);
			movement.SpeedX = 0;
			character.SetState(idleState);
		}*/
    }

    public override void EndState(CharacterBase character)
    {

    }



    public void JumpWallRun()
    {
    }

}