﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateHomingDash : CharacterState
{
	[Title("Parameter")]
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeStartup = 10f;

	CharacterBase target;
	float timer = 0;
	bool canDash = false;


	[Title("Feedback")]
	[SerializeField]
	private ParticleSystem startupParticle;
	[SerializeField]
	private ParticleSystem jumpParticleSystem;

	private void Start()
	{
		timeStartup = timeStartup /= 60f;
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Knockback.IsInvulnerable = true;
		character.Action.CancelAction();

		target = character.Action.CharacterHit;

		target.SetMotionSpeed(0.2f, timeStartup);
		character.Movement.SetSpeed(0, 0);

		timer = 0f;
		canDash = false;


		ParticleSystem particle = Instantiate(startupParticle, character.CenterPoint.position, Quaternion.identity);
		Destroy(particle.gameObject, 1f);

	}

	public override void UpdateState(CharacterBase character)
	{
		timer += Time.deltaTime;

		if (timer >= timeStartup && canDash == false)
		{
			canDash = true;
			character.Movement.SetSpeed(target.Movement.SpeedX * target.Movement.Direction * character.Movement.Direction, target.Movement.SpeedY);
			StartHomingDash(character);
			return;
		}

		Vector2 direction = target.transform.position - this.transform.position;
		if (target.Stats.Death == true)
		{
			character.Movement.SetSpeed(0, 0);
			character.ResetToIdle();
		}
		if (canDash == true && direction.sqrMagnitude <= 1)
		{
			character.Movement.SetSpeed(0, 0);
			character.ResetToIdle();
		}
		else if (canDash == true)
		{
			//Vector2 direction = target.transform.position - this.transform.position;
			direction.Normalize();
			float speedX = Mathf.Max(character.Movement.SpeedMax, Mathf.Abs(target.Movement.SpeedX) * 1.05f);
			float speedY = Mathf.Max(character.Movement.SpeedMax, Mathf.Abs(target.Movement.SpeedY) * 1.05f);
			character.Movement.SetSpeed(direction.x * character.Movement.Direction * speedX, direction.y * speedY);
			//character.Movement.SetSpeed(target.Movement.SpeedX * target.Movement.Direction * character.Movement.Direction, target.Movement.SpeedY);
		}

	}



	private void StartHomingDash(CharacterBase character)
	{
		character.Action.Animator.SetTrigger("Fall");
		ParticleSystem particle = Instantiate(jumpParticleSystem, this.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(character.Movement.SpeedX * character.Movement.Direction, character.Movement.SpeedY) * Mathf.Rad2Deg));
		Destroy(particle.gameObject, 0.5f);
	}


	/*public override void UpdateState(CharacterBase character)
	{
		timer += Time.deltaTime;

		Vector2 direction = target.transform.position - this.transform.position;
		if (timer >= timeStartup && target.Knockback.KnockbackDuration >= 0)
		{
			direction.Normalize();
			character.Movement.SetSpeed(direction.x * character.Movement.Direction * Mathf.Abs(target.Movement.SpeedX), direction.y * Mathf.Abs(target.Movement.SpeedY));
			//character.Action.CharacterHit
		}
		else if (timer >= timeStartup && direction.sqrMagnitude < 1f) //target.Knockback.KnockbackDuration <= 0)
		{
			character.ResetToIdle();
		}
	}*/

	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.IsInvulnerable = false;
	}
}