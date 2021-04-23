using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateParrySuccess : CharacterState
{
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeLag = 24;
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeCancel = 12;

	[SerializeField]
	CharacterMoveset moveset;
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;



	[Title("Parameter - Platform")]
	[SerializeField]
	LayerMask goThroughGroundMask;

	[Title("Dash")]
	[SerializeField]
	float joystickThreshold = 0.3f;

	[Title("Parry")]
	[SerializeField]
	float parryInfluenceAngle = 30f;


	[SerializeField]
	AttackManager counterAction;

	/*[SerializeField]
	CharacterState homingDashState;*/

	float t = 0f;
	bool inHitStop = true;


	private void Start()
	{
		timeLag /= 60;
		timeCancel /= 60;
	}



	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		inHitStop = true;
		t = timeLag;
		character.Movement.CurrentNumberOfJump += 1;
		evasiveMoveset.ResetDodge();
	}

	public override void UpdateState(CharacterBase character)
	{
		if (character.MotionSpeed == 0)
			return;

		if (inHitStop == true) // Première frame de fin de hitlag
		{
			inHitStop = false;
			if (character.Input.CheckActionHold(InputConst.RightShoulder) == true)
			{
				// Counter
				if (moveset.ActionAttack(character, counterAction) == true)
				{
					return;
				}
			}
			ParryInfluence(character);
		}


		/*if (character.Input.CheckAction(0, InputConst.LeftShoulder) && character.MotionSpeed != 0)
		{
			if (character.Action.CharacterHit != null) // On a touché quelqu'un 
			{
				character.SetState(homingDashState);
				character.Input.inputActions[0].timeValue = 0;
				return;
			}
		}*/



		t -= Time.deltaTime * character.MotionSpeed;
		if (t <= timeCancel)
		{
			if (evasiveMoveset.Parry(character))
			{
				return;
			}
			else if (moveset.ActionAttack(character))
			{
				return;
			}
			else if (character.Input.CheckAction(0, InputConst.Jump) || character.Input.CheckAction(0, InputConst.Smash))
			{
				character.ResetToIdle();
			}
		}

		if (t <= 0)
		{
			character.ResetToIdle();
		}
	}
	
	public override void LateUpdateState(CharacterBase character)
	{
		/*if (character.Rigidbody.IsGrounded == true && character.MotionSpeed != 0)
			character.ResetToIdle();*/
		if (t <= timeCancel)
		{
			if (InstantDodge(character))
			{
				return;
			}

		}
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}



	private bool InstantDodge(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded)
		{
			if (character.Input.vertical > joystickThreshold)
			{
				evasiveMoveset.ForceDodgeAerial(character);
				return true;
			}
			else if(character.Input.vertical < -joystickThreshold && character.Rigidbody.CollisionGroundInfo.gameObject.layer == 16)
			{
				character.Rigidbody.SetNewLayerMask(goThroughGroundMask, true);
				evasiveMoveset.ForceDodgeAerial(character);
				StartCoroutine(GoThroughGroundCoroutine(character.Rigidbody));
				return true;
			}
			else if (Mathf.Abs(character.Input.horizontal) > joystickThreshold)
			{
				character.ResetToIdle();
				return true;
			}
		}
		else
		{
			if (Mathf.Abs(character.Input.horizontal) > joystickThreshold || Mathf.Abs(character.Input.vertical) > joystickThreshold)
			{
				evasiveMoveset.ForceDodgeAerial(character);
				return true;
			}
		}
		return false;
	}



	private void ParryInfluence(CharacterBase character)
	{
		if (Mathf.Abs(character.Input.horizontal) < joystickThreshold && Mathf.Abs(character.Input.vertical) < joystickThreshold)
			return;
		Vector2 ejectionAngle = character.Knockback.Parry.CharacterParried.Knockback.GetAngleKnockback();
		Vector2 input = new Vector2(character.Input.horizontal, character.Input.vertical);
		/*
		//ejectionAngle = Vector2.Perpendicular(ejectionAngle);
		float influence = Vector2.Dot(input, Vector2.Perpendicular(ejectionAngle));

		Vector2 finalDirection = Quaternion.Euler(0, 0, parryInfluenceAngle * influence) * ejectionAngle;
		Debug.Log(finalDirection.normalized);
		character.Knockback.Parry.CharacterParried.Knockback.Launch(finalDirection.normalized, 1);*/

		float finalAngle = Vector2.Angle(ejectionAngle, input);
		if (finalAngle <= parryInfluenceAngle)
		{
			character.Knockback.Parry.CharacterParried.Knockback.Launch(input.normalized, 1);
		}
		else
		{
			finalAngle = Vector2.SignedAngle(ejectionAngle, input);
			Vector2 finalDirection = Quaternion.Euler(0, 0, parryInfluenceAngle * Mathf.Sign(finalAngle)) * ejectionAngle;
			character.Knockback.Parry.CharacterParried.Knockback.Launch(finalDirection.normalized, 1);
		}
	}


	private IEnumerator GoThroughGroundCoroutine(CharacterRigidbody rigidbody)
	{
		yield return null;
		rigidbody.ResetLayerMask();
	}


}