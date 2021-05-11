using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInputOutOfKnockback : MissionInputCondition
{

	bool condition = false;
	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		dummy.OnStateChanged += StateChangedCallback;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		dummy.OnStateChanged -= StateChangedCallback;
	}

	public void StateChangedCallback(CharacterState oldState, CharacterState newState)
	{
		if (oldState is CharacterStateKnockback && (newState is CharacterStateIdle || newState is CharacterStateAerial))
		{
			condition = true;
		}
	}
}
