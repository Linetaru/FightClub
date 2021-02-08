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

	public override void StartState(CharacterBase character)
	{
		t = timeTurnAround;
	}

	public override void UpdateState(CharacterBase character)
	{
		movement.Speed = movement.SpeedX * (t / timeTurnAround);
		movement.Speed = Mathf.Max(movement.Speed, 0);
		characterRigidbody.UpdateCollision(movement.Speed * movement.Direction, -10);
		t -= Time.deltaTime;
		if (t <= 0)
			character.SetState(idleState);

	}

	public override void EndState(CharacterBase character)
	{

	}
}