using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInputDodge : MissionInputCondition
{
	[SerializeField]
	bool aerialDodge = false;

	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		player.OnStateChanged += StateChangedCallback;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		player.OnStateChanged -= StateChangedCallback;
	}

	public void StateChangedCallback(CharacterState oldState, CharacterState newState)
	{
		if (newState is CharacterStateDodge && aerialDodge == false)
		{
			condition = true;
		}
		else if (newState is CharacterStateDodgeAerial && aerialDodge == true)
		{
			condition = true;
		}
	}
}
