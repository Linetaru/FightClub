using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInputOutOfKnockback : MissionInputCondition
{
	[SerializeField]
	bool isDummy = true;

	bool condition = false;
	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		if(isDummy == true)
			dummy.OnStateChanged += StateChangedCallback;
		else
			player.OnStateChanged += StateChangedCallback;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		if (isDummy == true)
			dummy.OnStateChanged -= StateChangedCallback;
		else
			player.OnStateChanged -= StateChangedCallback;
	}

	public void StateChangedCallback(CharacterState oldState, CharacterState newState)
	{
		if (oldState is CharacterStateKnockback && (newState is CharacterStateIdle || newState is CharacterStateAerial || newState is CharacterStateLanding))
		{
			condition = true;
		}
	}
}
