using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterEvasiveMoveset : MonoBehaviour
{
	[SerializeField]
	CharacterState stateDodge;
	[SerializeField]
	CharacterState stateDodgeAerial;

	[Title("Cooldown")]
	[SerializeField]
	float dodgeCooldown = 1f;

	int nbOfDodge = 1;
	float dodgeCooldownT = 0f;

	public bool Dodge(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == true)
			nbOfDodge = 1;

		if (character.Input.CheckAction(0, InputConst.Smash) && CanDodge())
		{
			if (character.Rigidbody.IsGrounded == true)
			{
				character.SetState(stateDodge);
				character.Input.inputActions[0].timeValue = 0;
				StartCoroutine(DodgeCooldownCoroutine());
				return true;
			}
			else
			{
				nbOfDodge = 0;
				character.SetState(stateDodgeAerial);
				character.Input.inputActions[0].timeValue = 0;
				StartCoroutine(DodgeCooldownCoroutine());
				return true;
			}

		}




		return false;
	}

	private bool CanDodge()
	{
		if (nbOfDodge == 0)
			return false;
		if (dodgeCooldownT > 0)
			return false;
		return true;
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
}
