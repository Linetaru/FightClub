using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateTurnAround : CharacterState
{

	[Title("States")]
	[SerializeField]
	CharacterState idleState;

	[Title("Components")]
	[SerializeField]
	CharacterRigidbody characterRigidbody;
	[SerializeField]
	CharacterMovement movement;

	[SerializeField]
	float timeTurnAround = 0.1f;

	float t = 0f;
	float initialSpeedX = 0;

	public override void StartState(CharacterBase character)
	{
		t = timeTurnAround;
		initialSpeedX = movement.SpeedX;
	}

	public override void UpdateState(CharacterBase character)
	{
		movement.SpeedX = initialSpeedX * (t / timeTurnAround);
		movement.SpeedX = Mathf.Max(movement.SpeedX, 0);
		characterRigidbody.UpdateCollision(movement.SpeedX * movement.Direction, -10);
		t -= Time.deltaTime;
		if (t <= 0)
			character.SetState(idleState);

	}

	public override void EndState(CharacterBase character)
	{

	}
}