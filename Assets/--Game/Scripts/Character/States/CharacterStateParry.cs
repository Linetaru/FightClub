using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParry : CharacterState
{
	float timeState = 0f;
	float timeParry = 0f;



	[SerializeField]
	GameObject debug;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.SpeedX = 0;
		character.Movement.SpeedY = character.Movement.SpeedY * 0.1f;
		//character.Movement.SpeedX = 0;
		character.Knockback.Parry.IsParry = true;
		timeState = character.Knockback.Parry.TimingParry[0] / 60f;
		timeParry = character.Knockback.Parry.TimingParry[character.Knockback.Parry.ParryNumber] / 60f;

		/*character.Parry.ParryNumber += 1;
		if (character.Parry.ParryNumber >= character.Parry.TimingParry.Length)
			character.Parry.ParryNumber = 0;*/

		//debug.SetActive(true);
	}

	public override void UpdateState(CharacterBase character)
	{
		character.Movement.ApplyGravity(0.05f);
		timeState -= Time.deltaTime * character.MotionSpeed;
		timeParry -= Time.deltaTime * character.MotionSpeed;

		if (timeParry <= 0)
		{
			character.Knockback.Parry.IsParry = false;
			//debug.SetActive(false);
		}
		if (timeState <= 0)
		{
			/*character.Parry.ParryNumber -= 1;
			character.Parry.ParryNumber = Mathf.Max(character.Parry.ParryNumber, 0);*/
			character.ResetToIdle();
		}

	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.Parry.IsParry = false;
		//debug.SetActive(false);
	}
}