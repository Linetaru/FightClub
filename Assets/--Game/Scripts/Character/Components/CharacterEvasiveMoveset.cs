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





	[Title("Parry")]
	[SerializeField]
	CharacterState stateParry;


	public bool Dodge(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == true)
			nbOfDodge = 2;

		if (character.Input.CheckAction(0, InputConst.RightTrigger) && CanDodge() && (Mathf.Abs(character.Input.horizontal) > 0.3f || Mathf.Abs(character.Input.vertical) > 0.3f))
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
		nbOfDodge = 2;
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
			character.SetState(stateParry);
			character.Input.inputActions[0].timeValue = 0;
		}
		return false;
	}
}
