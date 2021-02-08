using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateWallRun : CharacterState
{

    [SerializeField]
    CharacterState idleState;

    [SerializeField]
    CharacterRigidbody characterRigidbody;
    [SerializeField]
    CharacterMovement movement;
    [SerializeField]
    CharacterMovementJump movementJump;

    [SerializeField]
    float stickRunThreshold = 0.7f;
    [SerializeField]
    float deccelerationRate = 0.7f;

    float wallrunSpeed = 10.0f;
    [SerializeField]
    float wallrunSpeedMax = 10.0f;




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
        Debug.Log("Allo");
    }

    public override void UpdateState(CharacterBase character)
    {

        movement.SetSpeed(0f, wallrunSpeed);

        if (movement.SpeedY > 0)
        {
            wallrunSpeed -= deccelerationRate * Time.deltaTime;
        }
        else
        {
            character.SetState(idleState);
            wallrunSpeed = wallrunSpeedMax;
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