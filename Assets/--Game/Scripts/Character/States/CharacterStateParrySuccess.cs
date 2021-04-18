using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParrySuccess : CharacterState
{
	[SerializeField]
	float timeLag = 0.2f;
	[SerializeField]
	float timeCancel = 0.2f;
	[SerializeField]
	float timeCancelAttack = 0.2f;

	[SerializeField]
	CharacterMoveset moveset;
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;

	[SerializeField]
	AttackManager counterAction;

	[SerializeField]
	CharacterState homingDashState;

	float t = 0f;
	bool inHitStop = true;

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
			ParryInfluence(character);
			inHitStop = false;
		}

		//if((!character.Input.CheckActionUP(0, InputConst.RightShoulder) && !character.Input.CheckActionUP(0, InputConst.RightTrigger)) && character.MotionSpeed != 0)
		/*if (character.Input.CheckActionHold(InputConst.RightShoulder) == true && character.MotionSpeed != 0)
		{
			if (moveset.ActionAttack(character, counterAction) == true)
			{
				return;
			}
		}*/

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
			else if (Mathf.Abs(character.Input.horizontal) > 0.25f || Mathf.Abs(character.Input.vertical) > 0.25f)
			{
				evasiveMoveset.ForceDodge(character);
			}

		}
		if (t <= timeCancelAttack)
		{
			if (moveset.ActionAttack(character))
			{
				return;
			}
			/*else if (evasiveMoveset.Dodge(character))
			{
				return;
			}*/

			else if (character.Input.CheckAction(0, InputConst.Jump))
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
		if (character.Rigidbody.IsGrounded == true && character.MotionSpeed != 0)
			character.ResetToIdle();
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}

	[SerializeField]
	float parryInfluenceAngle = 30f;
	private void ParryInfluence(CharacterBase character)
	{
		if (Mathf.Abs(character.Input.horizontal) < 0.25f && Mathf.Abs(character.Input.vertical) < 0.25f)
			return;
		Vector2 ejectionAngle = character.Knockback.Parry.CharacterParried.Knockback.GetAngleKnockback();
		Vector2 input = new Vector2(character.Input.horizontal, character.Input.vertical);

		//ejectionAngle = Vector2.Perpendicular(ejectionAngle);
		float influence = Vector2.Dot(input, Vector2.Perpendicular(ejectionAngle));

		Vector2 finalDirection = Quaternion.Euler(0, 0, parryInfluenceAngle * influence) * ejectionAngle;
		Debug.Log(finalDirection.normalized);
		character.Knockback.Parry.CharacterParried.Knockback.Launch(finalDirection.normalized, 1);
	}



}