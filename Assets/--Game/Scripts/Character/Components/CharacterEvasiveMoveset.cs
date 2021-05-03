﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterEvasiveMoveset : MonoBehaviour
{
	[SerializeField]
	CharacterState stateDodge;
	[SerializeField]
	CharacterState stateDodgeAerial;

	[SerializeField]
	int maxNbOfDodge = 2;

	[Title("Cooldown")]
	[SerializeField]
	float dodgeCooldown = 1f;

	int nbOfDodge = 1;
	float dodgeCooldownT = 0f;





	[Title("Parry")]
	[SerializeField]
	CharacterState stateParry;
	[SerializeField]
	CharacterState stateAction;
	[SerializeField]
	AttackManager attackParry;

	public bool Dodge(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == true)
			ResetDodge();

		if (character.Input.CheckActionUP(0, InputConst.RightTrigger) && CanDodge() && (Mathf.Abs(character.Input.horizontal) > 0.3f || Mathf.Abs(character.Input.vertical) > 0.3f))
		{
			if (character.Rigidbody.IsGrounded == true)
			{
				if (character.Input.vertical > 0.4f)
				{
					nbOfDodge -= 1;
					character.Movement.SpeedY = 1;
					character.SetState(stateDodgeAerial);
					character.Input.inputActions[0].timeValue = 0;
					StartCoroutine(DodgeCooldownCoroutine());
					return true;
				}
				else
				{
					character.SetState(stateDodge);
					character.Input.inputActions[0].timeValue = 0;
					StartCoroutine(DodgeCooldownCoroutine());
					return true;
				}
			}
			else
			{
				nbOfDodge -= 1;
				character.SetState(stateDodgeAerial);
				character.Input.inputActions[0].timeValue = 0;
				StartCoroutine(DodgeCooldownCoroutine());
				return true;
			}

		}
		return false;
	}


	public void ForceDodgeGround(CharacterBase character)
	{
		nbOfDodge -= 1;
		character.SetState(stateDodge);
		StartCoroutine(DodgeCooldownCoroutine());
	}
	public void ForceDodgeAerial(CharacterBase character)
	{
		if (!CanDodge())
			return;
		nbOfDodge -= 1;
		character.SetState(stateDodgeAerial);
		StartCoroutine(DodgeCooldownCoroutine());
	}



	private bool CanDodge()
	{
		if (nbOfDodge == 0)
			return false;
		if (dodgeCooldownT > 0)
			return false;
		return true;
	}

	public void ResetDodge()
	{
		nbOfDodge = maxNbOfDodge;
	}


	private IEnumerator DodgeCooldownCoroutine()
	{
		dodgeCooldownT = dodgeCooldown;
		while (dodgeCooldownT > 0)
		{
			dodgeCooldownT -= Time.deltaTime; 
			yield return null; 
		}
	}





	public bool Parry(CharacterBase character)
	{
		if (character.Input.CheckAction(0, InputConst.RightShoulder))
		{
			/*if(character.Action.Action(attackParry) == true)
			{
				character.SetState(stateAction);
				character.Input.inputActions[0].timeValue = 0;
				return true;
			}*/
			character.SetState(stateParry);
			character.Input.inputActions[0].timeValue = 0;
		}
		return false;
	}
}
