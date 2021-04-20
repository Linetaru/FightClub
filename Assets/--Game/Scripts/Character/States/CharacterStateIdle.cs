using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateIdle : CharacterState
{
	[Title("States")]
	[SerializeField]
	CharacterState dashState;
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

	[SerializeField]
	[SuffixLabel("en frames")]
	float timeForDash = 3f;




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
	bool canWallRun = true;
	float gravityConst = 0.1f;

	float dashTimer = 0f;

	private void Start()
	{
		timeForDash /= 60;
		dashTimer = timeForDash;
		gravityConst = -0.1f / Time.deltaTime; // Cette constante est utilisé pour que le rigidbody fasse un test de gravité à chaque update pour bien mettre à jour IsGrounded
	}


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		dashTimer = 0;
		character.Movement.CurrentNumberOfJump = character.Movement.JumpNumber;
	}

	public override void UpdateState(CharacterBase character)
	{
		Movement(character);
		character.Movement.SpeedY = gravityConst;

		if (character.Rigidbody.CollisionGroundInfo != null)
		{
			if (character.Rigidbody.CollisionGroundInfo.gameObject.layer == 16)
			{
				character.Rigidbody.PreventFall(false);
			}
			else
			{
				character.Rigidbody.PreventFall(true);
			}
		}


		if (character.Input.CheckAction(0, InputConst.Jump) || character.Input.CheckAction(0, InputConst.Smash)) 
		{
			character.Input.inputActions[0].timeValue = 0;
			if (character.Rigidbody.CollisionGroundInfo != null && character.Input.vertical < -stickWalkThreshold) // ----------------- On passe au travers de la plateforme
			{
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
			else  // ----------------- Jump
			{
				character.SetState(jumpStartState);
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
		character.Rigidbody.PreventFall(true);
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
			UpdateDash(character);
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
			// Le + 0.1f c'est un probleme de precision de float (et c'est ce probleme qui nous empeche de faire du rollback si on a des float)
			if (character.Movement.SpeedX <= (character.Movement.SpeedMax * speedMultiplierWalk) + 0.1f) 
			{
				character.Movement.SpeedX = (character.Movement.SpeedMax * speedMultiplierWalk);
			}
			else
			{
				character.Movement.Decelerate();
			}
			UpdateDash(character);
		}
		else                                                    // R I E N
		{
			if (character.Movement.SpeedX < (character.Movement.SpeedMax * speedMultiplierWalk))
				inputDirection = 0;
			character.Movement.Decelerate();

			dashTimer = timeForDash;
		}
	}

	private void UpdateDash(CharacterBase character)
	{
		/*if (dashTimer > 0) 
		{
			dashTimer -= Time.deltaTime;
			float axisX = character.Input.horizontal;
			if (Mathf.Abs(axisX) > 0.95f)
			{
				Debug.Log("Dash");
				dashTimer = 0;
				character.SetState(dashState);
			}
		}*/
	}

	private void Jump()
	{

	}

	private IEnumerator GoThroughGroundCoroutine(CharacterRigidbody rigidbody)
	{
		yield return null;
		rigidbody.ResetLayerMask();
	}




}