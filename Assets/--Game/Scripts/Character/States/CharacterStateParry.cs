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
	bool flash = false;
	bool spamParry = false;

	private void Start()
	{
		timeInParry /= 60f;
		timeInGuard /= 60f;

	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		//Debug.Log(spamParry);
		/*if (oldState is CharacterStateParryBlow)
			spamParry = false;*/
		character.Knockback.Parry.IsParry = true;

		t = 0f;
		flash = false;

		if (Mathf.Abs(character.Input.horizontal) < 0.3f && Mathf.Abs(character.Input.vertical) < 0.3f)
			character.Knockback.Parry.ParryDirection = new Vector2(character.Movement.Direction, 0);
		else
			character.Knockback.Parry.ParryDirection = new Vector2(character.Input.horizontal, character.Input.vertical);

		if (character.Knockback.Parry.ParryAngle < 360)
		{
			debug.SetActive(true);
			debug.transform.localRotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(new Vector2(character.Knockback.Parry.ParryDirection.x, -character.Knockback.Parry.ParryDirection.y), Vector2.up) + 180f);
		}
		if (spamParry == false)
		{
			character.Model.FlashModel(Color.white, timeInParry);
			character.Movement.SpeedY = character.Movement.SpeedY * 0.1f;
		}
		else
		{
			t = timeInParry;
		}
	}

	public override void UpdateState(CharacterBase character)
	{
		character.Movement.SpeedX = character.Movement.SpeedX * 0.9f;
		character.Movement.ApplyGravity(0.05f);
		t += Time.deltaTime * character.MotionSpeed;

		if (t >= timeInParry && t <= timeInParry + timeInGuard)
		{
			character.Knockback.Parry.IsParry = false;
			character.Knockback.Parry.IsGuard = true;
		}
		else if (t >= timeInParry + timeInGuard)
		{
			StartCoroutine(PreventSpamParry());
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
		if (character.Knockback.Parry.ParryAngle < 360)
		{
			debug.SetActive(false);
		}
	}

	private IEnumerator PreventSpamParry()
	{
		spamParry = true;
		yield return new WaitForSeconds(0.2f);
		spamParry = false;
	}
}