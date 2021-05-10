using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateDash : CharacterState
{
	[Title("State")]
	[SerializeField]
	CharacterState dashEndState;
	[SerializeField]
	CharacterState aerialState;
	[SerializeField]
	CharacterState jumpStartState;

	[Title("Dash")]
	[SerializeField]
	float dashMultiplier = 1.2f;

	[SerializeField]
	float dashStartup = 1f;
	[SerializeField]
	float dashTime = 6f;
	[SerializeField]
	float dashDanceTime = 8f;

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

	[Title("Feedback")]
	[SerializeField]
	private ParticleSystem jumpParticleSystem;


	float t = 0f;
	int dashDirection = 0;
	bool inDashStartup = true;

	int bufferDirection = 0;


	private void Start()
	{
		dashStartup /= 60f;
		dashTime /= 60f;
		dashDanceTime /= 60f;
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = 0f;

		character.Knockback.Parry.IsGuardDash = true;
		dashDirection = character.Movement.Direction;
		inDashStartup = true;
		bufferDirection = 0;

		ParticleSystem particle = Instantiate(jumpParticleSystem, this.transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(1 * character.Movement.Direction, 1) * Mathf.Rad2Deg));
		Destroy(particle.gameObject, 0.5f);
	}

	public override void UpdateState(CharacterBase character)
	{
		// DASH STARTUP
		if (inDashStartup == true)
		{
			if (!character.Input.CheckActionHold(InputConst.RightTrigger))
			{
				evasiveMoveset.ForceDodgeGround(character);
				return;
			}

			if (Mathf.Abs(character.Input.horizontal) > (stickDashThreshold * 0.5f) && Mathf.Sign(character.Input.horizontal) != dashDirection)
			{
				if (t < dashDanceTime)
				{
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
					character.SetState(this);
					return;
				}
				else
				{
					bufferDirection = (int)Mathf.Sign(character.Input.horizontal);
				}
			}


			if (t < dashStartup)
			{
				character.Movement.SpeedX = 0;
			}
			else if (t < dashTime + dashStartup)
			{
				character.Movement.MaxAcceleration();
				character.Movement.SpeedX = character.Movement.SpeedMax * dashMultiplier;
			}
			else
			{
				inDashStartup = false;
			}
			t += Time.deltaTime;
		}



		// DASH 
		else if (inDashStartup == false)
		{
			if (bufferDirection != 0)
			{
				Debug.Log("Ah ?");
				character.Movement.Direction = bufferDirection;
				character.SetState(this);
				return;
			}

			if (!character.Input.CheckActionHold(InputConst.RightTrigger)) 
			{
				character.SetState(dashEndState);
			}
			else if (Mathf.Abs(character.Input.horizontal) > stickDashThreshold && Mathf.Sign(character.Input.horizontal) == dashDirection)
			{
				if (character.Movement.SpeedX < character.Movement.SpeedMax)
					character.Movement.Accelerate();
				else
					character.Movement.SpeedX = character.Movement.SpeedMax;





				if (character.Input.CheckAction(0, InputConst.Jump) || character.Input.CheckAction(0, InputConst.Smash))
				{
					character.Input.inputActions[0].timeValue = 0;
					if (character.Rigidbody.CollisionGroundInfo != null && character.Input.vertical < -0.25f) // ----------------- On passe au travers de la plateforme
					{
						if (character.Rigidbody.CollisionGroundInfo.gameObject.layer == 16)
						{
							character.Rigidbody.SetNewLayerMask(goThroughGroundMask, true); // Modifie le mask de collision du sol pour passer au travers de la plateforme
							StartCoroutine(GoThroughGroundCoroutine(character.Rigidbody));  // Coroutine qui attend 1 frame pour reset le mask de collision du perso

							character.SetState(aerialState);
							character.Movement.ApplyGravity();
						}
						else
						{
							character.SetState(jumpStartState);
						}
					}
					else  // ----------------- Jump
					{
						character.SetState(jumpStartState);
					}
				}
				else if (moveset.ActionAttack(character) == true)
				{

				}
				/*else if (evasiveMoveset.Dodge(character) == true)
				{

				}*/
				else if (evasiveMoveset.Parry(character) == true)
				{

				}
			}
			else // On arrête de courir
			{
				character.SetState(dashEndState);
			}
		}



	}
	
	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == false)
		{
			character.SetState(aerialState);
		}
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.Parry.IsGuardDash = false;
	}


	private IEnumerator GoThroughGroundCoroutine(CharacterRigidbody rigidbody)
	{
		yield return null;
		rigidbody.ResetLayerMask();
	}

}