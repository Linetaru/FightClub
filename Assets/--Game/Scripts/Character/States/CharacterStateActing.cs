using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateActing : CharacterState
{

	[Title("Parameter - Actions")]
	[SerializeField]
	CharacterMoveset characterMoveset;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		Debug.Log("Action");
	}

	public override void UpdateState(CharacterBase character)
	{
		//character.Action.CanEndAction();
		// Mettre les inputs en dessous

		characterMoveset.ActionAttack(character);
	}

	public override void LateUpdateState(CharacterBase character)
	{
		//character.Action.EndActionState();
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{
		Debug.Log("End Action");
	}
}