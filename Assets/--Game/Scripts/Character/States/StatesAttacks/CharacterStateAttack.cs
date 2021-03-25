using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateAttack : CharacterState
{
	[Title("Movements options")]

	[Title("Cancels")]
	[SerializeField]
	bool canCancel = false;
	[SerializeField]
	bool canCancelOnlyOnHit = false;
	[SerializeField]
	bool canJumpCancel = false;
	[SerializeField]
	bool canDashCancel = false;
	[SerializeField]
	bool canDodgeCancel = false;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{

	}

	public override void UpdateState(CharacterBase character)
	{


		if(character.Action.CanMoveCancel == true && canCancel == true)
		{

		}
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}