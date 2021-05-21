using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInputRespawn : MissionInputCondition
{

	bool condition = false;
	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		player.OnStateChanged += StateChangedCallback;
		dummy.OnStateChanged += StateChangedCallback;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		player.OnStateChanged -= StateChangedCallback;
		dummy.OnStateChanged -= StateChangedCallback;
	}


	public void StateChangedCallback(CharacterState oldState, CharacterState newState)
	{
		if (newState is CharacterStateRespawn)
		{
			condition = true;
		}
	}
}
