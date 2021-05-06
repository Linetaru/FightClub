using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateDashEnd : CharacterState
{
	[Title("State")]
	[SerializeField]
	CharacterState idleState;
	[SerializeField]
	CharacterState dashState;
	[SerializeField]
	CharacterState jumpStartState;

	[Title("Dash")]
	[SerializeField]
	float dashEndTime = 4f;

	[Title("Parameter - Actions")]
	[SerializeField]
	float stickDashThreshold = 0.7f;

	[Title("Parameter - Actions")]
	[SerializeField]
	CharacterMoveset moveset;
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;

	[Title("Parameter - Platform")]
	[SerializeField]
	LayerMask goThroughGroundMask;

	/*[Title("Feedback")]
	[SerializeField]
	private ParticleSystem jumpParticleSystem;*/


	float t = 0f;
	/*int dashDirection = 0;
	bool inDashStartup = true;*/


	private void Start()
	{
		dashEndTime /= 60f;
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = 0f;
	}

	public override void UpdateState(CharacterBase character)
	{
		t += Time.deltaTime;
		character.Movement.Decelerate();

		if (t > dashEndTime)
		{
			if (Mathf.Abs(character.Input.horizontal) > stickDashThreshold)
			{
				character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
				character.SetState(dashState);
			}
			else
			{
				character.ResetToIdle();
			}
		}
		



	}
	
	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == false)
		{
			character.ResetToAerial();
		}
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}


	private IEnumerator GoThroughGroundCoroutine(CharacterRigidbody rigidbody)
	{
		yield return null;
		rigidbody.ResetLayerMask();
	}

}