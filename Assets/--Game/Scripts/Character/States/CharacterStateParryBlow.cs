using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParryBlow : CharacterState
{

	[SerializeField]
	float timeBlow = 0.2f;
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;

	float t = 0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		evasiveMoveset.ResetDodge();
		t = timeBlow;
	}

	public override void UpdateState(CharacterBase character)
	{
		character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x * character.Movement.Direction;
		character.Movement.SpeedY = character.Knockback.GetAngleKnockback().y;


		t -= Time.deltaTime * character.MotionSpeed;
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