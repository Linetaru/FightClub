using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateTurnAround : CharacterState
{

	[Title("States")]
	[SerializeField]
	CharacterState idleState;


	[SerializeField]
	float timeTurnAround = 0.1f;

	float t = 0f;
	float initialSpeedX = 0;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.ResetAcceleration();
		t = timeTurnAround;
		initialSpeedX = character.Movement.SpeedX;
	}

	public override void UpdateState(CharacterBase character)
	{
		character.Movement.SpeedX = initialSpeedX * (t / timeTurnAround);
		character.Movement.SpeedX = Mathf.Max(character.Movement.SpeedX, 0);
		t -= Time.deltaTime;
		if (t <= 0)
		{
			character.Movement.SpeedX = initialSpeedX * 0.75f;
			character.SetState(idleState);
		}

	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{

	}
}