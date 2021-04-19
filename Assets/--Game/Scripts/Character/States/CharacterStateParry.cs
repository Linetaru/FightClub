using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParry : CharacterState
{
	float timeState = 0f;
	float timeParry = 0f;

	[SerializeField]
	float timeInParry = 10;
	[SerializeField]
	float timeInGuard = 20;

	[SerializeField]
	GameObject debug;


	float t = 0f;

	private void Start()
	{
		timeInParry /= 60f;
		timeInGuard /= 60f;
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		//character.Movement.SpeedX = character.Movement.SpeedX * 0.2f;
		character.Movement.SpeedY = character.Movement.SpeedY * 0.1f;
		//character.Movement.SpeedX = 0;
		character.Knockback.Parry.IsParry = true;

		t = 0f;
		//timeState = character.Knockback.Parry.TimingParry[0] / 60f;
		//timeParry = character.Knockback.Parry.TimingParry[character.Knockback.Parry.ParryNumber] / 60f;

		/*character.Parry.ParryNumber += 1;
		if (character.Parry.ParryNumber >= character.Parry.TimingParry.Length)
			character.Parry.ParryNumber = 0;*/

		//debug.SetActive(true);
	}

	public override void UpdateState(CharacterBase character)
	{
		character.Movement.SpeedX = character.Movement.SpeedX * 0.9f;
		character.Movement.ApplyGravity(0.05f);
		t += Time.deltaTime * character.MotionSpeed;
		/*timeState -= Time.deltaTime * character.MotionSpeed;
		timeParry -= Time.deltaTime * character.MotionSpeed;*/

		/*if (timeParry <= 0)
		{
			character.Knockback.Parry.IsParry = false;
		}
		if (timeState <= 0)
		{
			character.ResetToIdle();
		}*/

		if (t >= timeInParry && t <= timeInParry + timeInGuard)
		{
			character.Knockback.Parry.IsParry = false;
			character.Knockback.Parry.IsGuard = true;
		}
		else if (t >= timeInParry + timeInGuard)
		{
			character.ResetToIdle();
		}

	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.Parry.IsParry = false;
		character.Knockback.Parry.IsGuard = false;
		//debug.SetActive(false);
	}
}