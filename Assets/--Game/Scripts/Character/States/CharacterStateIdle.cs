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
	[SerializeField]
	CharacterState smashPressedState;


	[Title("Parameter - Controls")]
	[SerializeField]
	float stickWalkThreshold = 0.3f;
	[SerializeField]
	float stickRunThreshold = 0.7f;


	[Title("Parameter - Speed")]
	[SerializeField]
	float speedMultiplierWalk = 0.2f;
	[SerializeField]
	float speedRequiredForWallRun = 8f;

	// Acceleration
	/*[HorizontalGroup("Acceleration")]
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
	AnimationCurve deccelerationMultiplierCurve;*/



	[Title("Parameter - Actions")]
	[SerializeField]
	AttackManager attackKick;


	float timeDecceleration = 0f;


	float inputDirection = 0;

	public bool canWallRun = true;

	float speedImpulsion = 0f;


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		//Debug.Log("IdleState");
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

			character.Movement.Direction = (int)Mathf.Sign(axisX);
			inputDirection = (int)Mathf.Sign(axisX);
			character.Movement.Accelerate();
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

			character.Movement.Direction = (int)Mathf.Sign(axisX);
			// Walk vitesse constante
			if (character.Movement.SpeedX < (character.Movement.SpeedMax * speedMultiplierWalk))
			{
				character.Movement.SpeedX = (character.Movement.SpeedMax * speedMultiplierWalk);
			}
			else
			{
				// Decceleration
				character.Movement.Decelerate();
			}
		}
		else													// R I E N
		{
			if(character.Movement.SpeedX < (character.Movement.SpeedMax * speedMultiplierWalk))
				inputDirection = 0;
			// Decceleration
			character.Movement.Decelerate();
		}
		character.Movement.ApplyGravity();


		if (character.Input.inputActions.Count != 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Attack)
			{
				if(character.Input.horizontal != 0)
                {
					character.SetState(smashPressedState);
                }
                else
				{
					character.Action.Action(attackKick);
				}
				character.Input.inputActions[0].timeValue = 0;
			}
		}
	}


	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.CollisionWallInfo != null && canWallRun == true)
		{
			if (character.Rigidbody.CollisionWallInfo.gameObject.layer == 15 && character.Movement.SpeedX > speedRequiredForWallRun)
				character.SetState(wallRunState);
			else
				character.Movement.ResetAcceleration(); // On reset l'acceleration pour rester a zero
		}
		else if (character.Input.inputActions.Count != 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Jump)
			{
				character.Movement.Jump();
				character.SetState(jumpState);
				character.Input.inputActions[0].timeValue = 0;
			}
		}
	}

	/*private void Accelerate(CharacterBase character)
	{
		timeDecceleration = 0;

		if (timeAcceleration == 0 && (character.Movement.MaxSpeed * speedMultiplierWalk) >= character.Movement.SpeedX)
			character.Movement.SpeedX = (character.Movement.MaxSpeed * speedMultiplierWalk);

		if (timeAcceleration < timeAccelerationMax)
			timeAcceleration += Time.deltaTime;

		character.Movement.Accelerate(accelerationMultiplierCurve.Evaluate(timeAcceleration / timeAccelerationMax));
		/*speed += (movement.MaxSpeed * speedMultiplierWalk);
		movement.SpeedX = speed;*/
	/*}



	private void Deccelerate(CharacterBase character)
	{
		timeAcceleration = 0;
		if (timeDecceleration < timeDeccelerationMax)
			timeDecceleration += Time.deltaTime;
		character.Movement.Decelerate(deccelerationMultiplierCurve.Evaluate(timeDecceleration / timeDeccelerationMax));
	}*/


	public override void EndState(CharacterBase character, CharacterState oldState)
	{
		inputDirection = 0;
		character.Movement.ResetAcceleration();
		/*timeDecceleration = 0;
		timeAcceleration = 0;*/
	}
}