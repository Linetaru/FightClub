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
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;

	[Title("Parameter - Platform")]
	[SerializeField]
	LayerMask goThroughGroundMask;




	float inputDirection = 0;
	public bool canWallRun = true;
	float gravityConst = 0.1f;

	private void Start()
	{
		gravityConst = -0.1f / Time.deltaTime; // Cette constante est utilisé pour que le rigidbody fasse un test de gravité à chaque update pour bien mettre à jour IsGrounded
	}


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.CurrentNumberOfJump = character.Movement.JumpNumber;
	}

	public override void UpdateState(CharacterBase character)
	{
		Movement(character);
		character.Movement.SpeedY = gravityConst;
		//character.Movement.ApplyGravity();

		if (character.Input.CheckAction(0, InputConst.Jump)) 
		{
			if (character.Rigidbody.CollisionGroundInfo != null && character.Input.vertical < -stickWalkThreshold) // ----------------- On passe au travers de la plateforme
			{
				character.Input.inputActions[0].timeValue = 0;
				if (character.Rigidbody.CollisionGroundInfo.gameObject.layer == 16)
				{
					character.Rigidbody.SetNewLayerMask(goThroughGroundMask, true); // Modifie le mask de collision du sol pour passer au travers de la plateforme
					StartCoroutine(GoThroughGroundCoroutine(character.Rigidbody));// Coroutine qui attend 1 frame pour reset le mask de collision du perso

					character.SetState(aerialState);
					character.Movement.ApplyGravity();
				}
				else
				{
					character.SetState(jumpStartState);
				}
			}
			else if (character.Input.inputActions[0].action == InputConst.Jump) // ----------------- Jump
			{
				character.SetState(jumpStartState);
				character.Input.inputActions[0].timeValue = 0;
			}
		}
		else if (moveset.ActionAttack(character) == true)
		{

		}
		else if (evasiveMoveset.Dodge(character) == true)
		{

		}
	}


	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.CollisionWallInfo.Collision != null && canWallRun == true) // ------------ Wall run
		{
			if (character.Movement.SpeedX > speedRequiredForWallRun && character.Rigidbody.CollisionWallInfo.Collision.gameObject.layer == 15)
				character.SetState(wallRunState);
			else if (character.Rigidbody.CollisionWallInfo.Collision.gameObject.layer == 15)
				character.Movement.ResetAcceleration(); // On reset l'acceleration pour ne pas avoir une vitesse de ouf quand le mur disparait
		}	
		else if (character.Rigidbody.IsGrounded == false) // ------------ On tombe
		{
			character.SetState(aerialState);
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
	}





	private IEnumerator GoThroughGroundCoroutine(CharacterRigidbody rigidbody)
	{
		yield return null;
		rigidbody.ResetLayerMask();
	}




}