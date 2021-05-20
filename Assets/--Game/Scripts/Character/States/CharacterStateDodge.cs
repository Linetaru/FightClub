using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateDodge : CharacterState
{

	[Title("Controller")]
	[SerializeField]
	float joystickThreshold = 0.3f;

	[Title("Physics")]
	[SerializeField]
	LayerMask dodgeLayerMask;

	[Title("Parameter")]
	[SerializeField]
	float distanceDodge = 5;
	[SerializeField]
	[SuffixLabel("en frame")]
	float timeDodge = 20;
	[SerializeField]
	[SuffixLabel("en frame")]
	Vector2 invulnerableInterval = new Vector2(4, 15);

	[Title("States")]
	[SerializeField]
	CharacterState groundState; // à bouger dans le characterBase peut etre ?

	float speedDodge;
	float t = 0f;
	Vector2 directionDodge;

	private void Start()
	{
		timeDodge /= 60f;
		invulnerableInterval /= 60f;
		speedDodge = distanceDodge / (invulnerableInterval.y - invulnerableInterval.x);
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		// Set la direction du dodge
		t = 0f;
		directionDodge = new Vector2(0, 0);


		if (Mathf.Abs(character.Input.horizontal) > joystickThreshold)
			directionDodge.x = Mathf.Sign(character.Input.horizontal);
		else
			directionDodge.x = character.Movement.Direction;

		character.Rigidbody.SetNewLayerMask(dodgeLayerMask);
		//character.Rigidbody.PreventFall(true);
	}

	public override void UpdateState(CharacterBase character)
	{
		t += Time.deltaTime;
		//character.Movement.SetSpeed(directionDodge.x * speedDodge * character.Movement.Direction, 0);

		if (t >= invulnerableInterval.x && t < invulnerableInterval.y)
		{
			character.Knockback.IsInvulnerable = true;
			character.Movement.SetSpeed(directionDodge.x * speedDodge * character.Movement.Direction, 0);
		}

		else if (t >= invulnerableInterval.y || t < invulnerableInterval.x)
		{
			character.Movement.SetSpeed(0, 0);
			character.Knockback.IsInvulnerable = false;
		}


		if (t >= timeDodge)
		{
			character.Movement.SetSpeed(0, 0);
			character.SetState(groundState);
		}

	}
	
	/*public override void LateUpdateState(CharacterBase character)
	{

	}*/

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.IsInvulnerable = false;
		character.Rigidbody.ResetLayerMask();
		//character.Rigidbody.PreventFall(false);
	}
}