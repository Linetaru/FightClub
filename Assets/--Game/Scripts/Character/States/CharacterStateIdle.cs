using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateIdle : CharacterState
{
	[Title("States")]
	[SerializeField]
	CharacterState aerialState;
	[SerializeField]
	CharacterState wallRunState;
	[SerializeField]
	CharacterState jumpStartState;
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


	[Title("Parameter - Actions")]
	[SerializeField]
	CharacterMoveset moveset;

	[Title("Parameter - Platform")]
	[SerializeField]
	LayerMask platformLayerMask;
	[SerializeField]
	LayerMask goThroughGroundMask;




	float inputDirection = 0;
	public bool canWallRun = true;


	public override void StartState(CharacterBase character, CharacterState oldState)
	{

	}

	public override void UpdateState(CharacterBase character)
	{
		Movement(character);

		if(moveset.ActionAttack(character) == true)
		{

		}
		else if (character.Input.inputActions.Count != 0) 
		{
			if (character.Input.inputActions[0].action == InputConst.Jump && character.Rigidbody.CollisionGroundInfo != null && character.Input.vertical < -stickWalkThreshold) // ----------------- On passe au travers de la plateforme
			{
				if (character.Rigidbody.CollisionGroundInfo.gameObject.layer == 16)
				{
					character.Rigidbody.SetNewLayerMask(goThroughGroundMask, true); // Modifie le mask de collision du sol pour passer au travers de la plateforme
					StartCoroutine(GoThroughGroundCoroutine(character.Rigidbody));// Coroutine qui attend 1 frame pour reset le mask de collision du perso

					character.SetState(aerialState);
					character.Movement.SpeedY = 0;
					character.Movement.ApplyGravity();
					character.Input.inputActions[0].timeValue = 0;
				}
			}
			else if (character.Input.inputActions[0].action == InputConst.Jump) // ----------------- Jump
			{
				character.SetState(jumpStartState);
				character.Input.inputActions[0].timeValue = 0;
			}
		}
	}


	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.CollisionWallInfo != null && canWallRun == true) // ------------ Wall run
		{
			if (character.Movement.SpeedX > speedRequiredForWallRun && character.Rigidbody.CollisionWallInfo.gameObject.layer == 15)
				character.SetState(wallRunState);
			else
				character.Movement.ResetAcceleration(); // On reset l'acceleration pour ne pas avoir une vitesse de ouf quand le mur disparait
		}
		
		else if (character.Rigidbody.CollisionGroundInfo == null) // ------------ On tombe
		{
			character.SetState(aerialState);
			character.Movement.SpeedY = 0;
			character.Movement.ApplyGravity();
		}
	}



	public override void EndState(CharacterBase character, CharacterState oldState)
	{
		inputDirection = 0;
		character.Movement.ResetAcceleration();

	}



	private void Movement(CharacterBase character)
	{
		float axisX = character.Input.horizontal;

		// Controls
		if (Mathf.Abs(axisX) > stickRunThreshold)               // R U N
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
		else if (Mathf.Abs(axisX) > stickWalkThreshold)         // W A L K
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
		else                                                    // R I E N
		{
			if (character.Movement.SpeedX < (character.Movement.SpeedMax * speedMultiplierWalk))
				inputDirection = 0;
			// Decceleration
			character.Movement.Decelerate();
		}
		character.Movement.ApplyGravity();
	}





	private IEnumerator GoThroughGroundCoroutine(CharacterRigidbody rigidbody)
	{
		yield return null;
		rigidbody.ResetLayerMask();
	}




}