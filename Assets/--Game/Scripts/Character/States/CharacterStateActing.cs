using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateActing : CharacterState
{

	[Title("Parameter - Actions")]
	[SerializeField]
	AttackManager attackKick;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{

	}

	public override void UpdateState(CharacterBase character)
	{
		character.Action.CanEndAction();
		// Mettre les inputs en dessous

		if (character.Input.inputActions.Count != 0)
		{
			if (character.Input.inputActions[0].action == InputConst.Attack)
			{
				if(character.Action.Action(attackKick) == true) // L'attaque est bien parti
					character.Input.inputActions[0].timeValue = 0;
			}
		}
	}

	public override void LateUpdateState(CharacterBase character)
	{
		character.Action.EndActionState();
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{

	}
}