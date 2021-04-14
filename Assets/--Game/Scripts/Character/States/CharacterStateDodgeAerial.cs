using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateDodgeAerial : CharacterState
{
	[Title("Controller")]
	[SerializeField]
	float joystickThreshold = 0.3f;

	[Title("Physics")]
	[SerializeField]
	LayerMask dodgeLayerMask;

	[Title("Parameter")]
	[SerializeField]
	float distanceDodge = 2.5f;
	[SerializeField]
	[SuffixLabel("en frame")]
	float timeDodge = 0.2f;
	[SerializeField]
	[SuffixLabel("en frame")]
	Vector3 invulnerableInterval = new Vector2(0.02f, 0.15f);

	[Title("States")]
	[SerializeField]
	CharacterState groundState; // à bouger dans le characterBase peut etre ?
	[SerializeField]
	CharacterState aerialState; // à bouger dans le characterBase peut etre ?

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
		t = 0f;
		directionDodge = new Vector2(0, 0);

		if (Mathf.Abs(character.Input.vertical) > joystickThreshold)
			directionDodge.y = Mathf.Sign(character.Input.vertical);
		if (Mathf.Abs(character.Input.horizontal) > joystickThreshold)
			directionDodge.x = Mathf.Sign(character.Input.horizontal);

		directionDodge.Normalize();

		character.Rigidbody.SetNewLayerMask(dodgeLayerMask);
	}

	public override void UpdateState(CharacterBase character)
	{
		t += Time.deltaTime;

		if (t >= invulnerableInterval.x && t < invulnerableInterval.y) // Dodge
		{
			//character.Knockback.IsInvulnerable = true;
			character.Movement.SetSpeed(directionDodge.x * speedDodge * character.Movement.Direction, directionDodge.y * speedDodge);
		}
		else if (t < invulnerableInterval.x) // Startup du Dodge
		{
			character.Movement.SetSpeed(0, 0);
			//character.Knockback.IsInvulnerable = false;
		}
		else if (t >= invulnerableInterval.y) // Lag du Dodge
		{
			//AirControl(character);
			//character.Movement.ApplyGravity();
			//character.Knockback.IsInvulnerable = false;
			character.Movement.SetSpeed(0, 0);
		}


		if (t >= timeDodge)
		{
			character.SetState(aerialState);
		}
	}
	
	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == true) // Ground cancel
		{
			character.Movement.ResetAcceleration();
			character.Movement.SetSpeed(character.Movement.SpeedX, -5f);
			character.SetState(groundState);
		}
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.IsInvulnerable = false;
		character.Rigidbody.ResetLayerMask();
	}




	// Note - à dégager le plus vite possible
	/*[Title("Air Control")]
	[SerializeField]
	float airControl = 1f;
	[SerializeField]
	float airFriction = 0.9f;
	[SerializeField]
	float maxAerialSpeed = 10f;
	private void AirControl(CharacterBase character)
	{
		float axisX = character.Input.horizontal;

		float aerialDirection;

		if (character.Movement.Direction > 0)
			aerialDirection = axisX;
		else
			aerialDirection = -axisX;

		character.Movement.SpeedX += (airControl * aerialDirection * airFriction) * Time.deltaTime;

		if (character.Movement.SpeedX >= maxAerialSpeed)
		{
			character.Movement.SpeedX = maxAerialSpeed;
		}
		else if (character.Movement.SpeedX <= -maxAerialSpeed)
		{
			character.Movement.SpeedX = -maxAerialSpeed;
		}
	}*/

}