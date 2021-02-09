using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateIdle : CharacterState
{
	[Title("States")]
	[SerializeField]
	CharacterState wallRunState;
	[SerializeField]
	CharacterState jumpState;
	[SerializeField]
	CharacterState turnAroundState;

	[Title("Components")]
	[SerializeField]
	CharacterRigidbody characterRigidbody;
	[SerializeField]
	CharacterMovement movement;

	[Title("Parameter - Controls")]
	[SerializeField]
	float stickWalkThreshold = 0.3f;
	[SerializeField]
	float stickRunThreshold = 0.7f;


	[Title("Parameter - Speed")]
	[SerializeField]
	float speedMultiplierWalk = 0.2f;

	// Acceleration
	[HorizontalGroup("Acceleration")]
	[SerializeField]
	float timeAccelerationMax = 1;

	[HorizontalGroup("Acceleration")]
	[SerializeField]
	[HideLabel]
	AnimationCurve accelerationMultiplierCurve;

	float timeAcceleration = 0f;

	// Decceleration
	[HorizontalGroup("Decceleration")]
	[SerializeField]
	float timeDeccelerationMax = 1;
	[HorizontalGroup("Decceleration")]
	[SerializeField]
	[HideLabel]
	AnimationCurve deccelerationMultiplierCurve;


	float timeDecceleration = 0f;


	float inputDirection = 0;

	public bool canWallRun = true;

	float speedImpulsion = 0f;


	public override void StartState(CharacterBase character)
	{
		Debug.Log("IdleState");
	}

	public override void UpdateState(CharacterBase character)
	{
		float axisX = character.Input.horizontal;

		// Controls
		if (Mathf.Abs(axisX) > stickRunThreshold)				// R U N
		{
			if (inputDirection == 0)
			{
				inputDirection = (int)Mathf.Sign(axisX);
			}
			if (inputDirection != Mathf.Sign(axisX))
			{
				inputDirection = 0;
				character.SetState(turnAroundState);
				return;
			}

			movement.Direction = (int)Mathf.Sign(axisX);
			inputDirection = (int)Mathf.Sign(axisX);
			Accelerate();
		}
		else if (Mathf.Abs(axisX) > stickWalkThreshold)			// W A L K
		{
			if (inputDirection == 0)
			{
				inputDirection = (int)Mathf.Sign(axisX);
			}
			if (inputDirection != Mathf.Sign(axisX))
			{
				inputDirection = 0;
				character.SetState(turnAroundState);
				return;
			}

			movement.Direction = (int)Mathf.Sign(axisX);
			// Walk vitesse constante
			if (movement.SpeedX < (movement.MaxSpeed * speedMultiplierWalk))
			{
				movement.SpeedX = (movement.MaxSpeed * speedMultiplierWalk);
			}
			else
			{
				// Decceleration
				Deccelerate();
			}
		}
		else													// R I E N
		{
			if(movement.SpeedX < (movement.MaxSpeed * speedMultiplierWalk))
				inputDirection = 0;
			// Decceleration
			Deccelerate();
		}

		characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, -10);

		

		if (characterRigidbody.CollisionWallInfo != null && canWallRun == true)
		{
			character.SetState(wallRunState);
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
		timeDecceleration = 0;

		if (timeAcceleration == 0)
			movement.SpeedX = (movement.MaxSpeed * speedMultiplierWalk);

		if (timeAcceleration < timeAccelerationMax)
			timeAcceleration += Time.deltaTime;

		movement.Accelerate(accelerationMultiplierCurve.Evaluate(timeAcceleration / timeAccelerationMax));
		/*speed += (movement.MaxSpeed * speedMultiplierWalk);
		movement.SpeedX = speed;*/
	}



	private void Deccelerate()
	{
		timeAcceleration = 0;
		if (timeDecceleration < timeDeccelerationMax)
			timeDecceleration += Time.deltaTime;
		movement.Decelerate(deccelerationMultiplierCurve.Evaluate(timeDecceleration / timeDeccelerationMax));
	}


	public override void EndState(CharacterBase character)
	{

	}
}