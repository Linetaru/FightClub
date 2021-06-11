using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStartCharacterState : CharacterState
{
	[SerializeField]
	CharacterState ballIdleState;

	[SerializeField]
	float startKickOffDuration = 3.0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		StartCoroutine(LaunchBall(character));
		character.Knockback.IsInvulnerable = true;
	}

	public override void UpdateState(CharacterBase character)
	{

	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	IEnumerator LaunchBall(CharacterBase character)
    {
		yield return new WaitForSeconds(startKickOffDuration);
		character.Knockback.IsInvulnerable = false;
		character.SetState(ballIdleState);
		character.Movement.Jump();
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		StopCoroutine(LaunchBall(character));
		character.Knockback.IsInvulnerable = false;
		//character.Movement.SetSpeed(0f, 4f);
	}
}