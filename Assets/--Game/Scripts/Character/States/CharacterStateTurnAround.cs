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

	[Title("Dash")]
	[SerializeField]
	float stickWalkThreshold = 0.3f;
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeForDash = 3f;

	float t = 0f;
	float initialSpeedX = 0;

	float dashTimer = 0f;

	private void Start()
	{
		timeForDash /= 60;
		dashTimer = timeForDash;
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		dashTimer = 0;
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
			//character.Movement.SpeedX = initialSpeedX * 0.75f;
			character.SetState(idleState);
		}
		else
		{
			/*if (Mathf.Abs(character.Input.horizontal) > stickWalkThreshold)
			{
				UpdateDash(character);
			}
			else 
			{
				dashTimer = timeForDash;
			}*/
		}

	}


	private void UpdateDash(CharacterBase character)
	{
		if (dashTimer > 0)
		{
			dashTimer -= Time.deltaTime;
			if (Mathf.Abs(character.Input.horizontal) > 0.95f)
			{
				Debug.Log("Dash Cancel Turn Around");
				for (int i = 0; i < 50; i++)
				{
					character.Movement.Accelerate();
				}
				dashTimer = 0;
				character.SetState(idleState);
			}
		}
	}


	public override void EndState(CharacterBase character, CharacterState oldState)
	{

	}
}