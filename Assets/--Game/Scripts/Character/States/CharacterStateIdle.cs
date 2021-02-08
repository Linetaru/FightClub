using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateIdle : CharacterState
{

	[SerializeField]
	CharacterState wallRunState;
	[SerializeField]
	CharacterState jumpState;

	[SerializeField]
	CharacterRigidbody characterRigidbody;
	[SerializeField]
	CharacterMovement movement;

	[SerializeField]
	float stickWalkThreshold = 0.3f;
	[SerializeField]
	float stickRunThreshold = 0.7f;

	//float speedX = 0;

	[SerializeField]
	float speedXMin = 1;
	[SerializeField]
	float speedXMax = 8;
	[SerializeField]
	float timeAccelerationMax = 1;
	[SerializeField]
	float timeDeccelerationMax = 1;

	[SerializeField]
	AnimationCurve accelerationCurve;
	[SerializeField]
	AnimationCurve deccelerationCurve;

	bool acceleration = false;
	float timeAcceleration = 0f;

	public bool canWallRun = true;

	//int wallrunCount = 1;

	//int direction;

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
		Debug.Log("IdleState");
		//if (wallrunCount < 1 && characterRigidbody.IsGrounded)
		//{
		//	wallrunCount = 1;
		//}

	}

	public override void UpdateState(CharacterBase character)
	{
		float axisX = Input.GetAxis("Horizontal");

		if (Mathf.Abs(axisX) > stickRunThreshold)
		{
			movement.Direction = (int)Mathf.Sign(axisX);
			// Acceleration
			Accelerate();

		}
		else if (Mathf.Abs(axisX) > stickWalkThreshold)
		{
			movement.Direction = (int)Mathf.Sign(axisX);
			// Walk vitesse constante
			if (movement.SpeedX < speedXMin)
			{
				movement.SpeedX = speedXMin;
			}
			else
			{
				// Decceleration
				Deccelerate();
			}

		}
		else
		{
			// Decceleration
			Deccelerate();
		}

		//characterRigidbody.UpdateCollision(10, -10);
		characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, -10);

		

		if (characterRigidbody.CollisionWallInfo != null && canWallRun == true/*wallrunCount == 1 &&*/ )
		{
			character.SetState(wallRunState);
			//wallrunCount = 0;
		}
		else if (character.Input.inputActions.Count != 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Jump)
			{
				movement.Jump();
				character.SetState(jumpState);
				character.Input.inputActions[0].timeValue = 0;
			}
		}
	}



	private void Accelerate()
	{
		if (acceleration == false)
		{
			timeAcceleration = movement.SpeedX / speedXMax * timeAccelerationMax;
		}
		acceleration = true;
		if (timeAcceleration < timeAccelerationMax)
			timeAcceleration += Time.deltaTime;
		if (movement.SpeedX < speedXMax)
		{
			float t = timeAcceleration / timeAccelerationMax;
			movement.SpeedX = speedXMin + (accelerationCurve.Evaluate(t) * (speedXMax - speedXMin));
		}

	}

	private void Deccelerate()
	{
		if (acceleration == true)
		{
			timeAcceleration = movement.SpeedX / speedXMax * timeDeccelerationMax;
		}
		acceleration = false;
		if (timeAcceleration > 0)
			timeAcceleration -= Time.deltaTime;
		if (movement.SpeedX > 0)
		{
			float t = timeAcceleration / timeDeccelerationMax;
			movement.SpeedX = deccelerationCurve.Evaluate(t) * speedXMax;	
		}


	}

	public override void EndState(CharacterBase character)
	{

	}
}