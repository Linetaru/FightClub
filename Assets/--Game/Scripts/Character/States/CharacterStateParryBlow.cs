﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParryBlow : CharacterState
{

	[SerializeField]
	float timeBlow = 0.2f;
	[SerializeField]
	float timeStop = 0.2f;
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;

	float t = 0f;
	float initialSpeedX = 0f;
	float initialSpeedY = 0f;

	[SerializeField]
	float ejectionPower = 10f;
	[SerializeField]
	AnimationCurve ejectionCurve;


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		evasiveMoveset.ResetDodge();
		t = timeBlow;

		initialSpeedX = character.Knockback.GetAngleKnockback().x * ejectionPower * character.Movement.Direction;
		initialSpeedY = character.Knockback.GetAngleKnockback().y * ejectionPower;

		character.Movement.SpeedX = initialSpeedX;
		character.Movement.SpeedY = initialSpeedY;
	}

	public override void UpdateState(CharacterBase character)
	{
		float coef = ejectionCurve.Evaluate(1 - (t / (timeBlow - timeStop)));

		initialSpeedX = character.Knockback.GetAngleKnockback().x * ejectionPower * character.Movement.Direction;
		initialSpeedY = character.Knockback.GetAngleKnockback().y * ejectionPower;
		character.Movement.SpeedX = Mathf.Lerp(initialSpeedX, 0, coef); //character.Knockback.GetAngleKnockback().x * character.Movement.Direction;
		character.Movement.SpeedY = Mathf.Lerp(initialSpeedY, 0, coef); //character.Knockback.GetAngleKnockback().y;


		t -= Time.deltaTime * character.MotionSpeed;
		if (t <= (timeBlow - timeStop))
		{
			evasiveMoveset.Parry(character);
			//evasiveMoveset.Dodge(character);
		}

		if (t <= 0)
			character.ResetToIdle();
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}