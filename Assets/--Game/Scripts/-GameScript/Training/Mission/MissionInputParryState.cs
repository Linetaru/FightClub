using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputParryState : MissionInputCondition
{
	[SerializeField]
	bool isDummy = false;

	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		if (isDummy == true)
			dummy.OnStateChanged += CallbackCondition;
		else
			player.OnStateChanged += CallbackCondition;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;

		if (isDummy == true)
			dummy.OnStateChanged -= CallbackCondition;
		else
			player.OnStateChanged -= CallbackCondition;
	}

	public void CallbackCondition(CharacterState oldState, CharacterState newState)
	{
		if (newState is CharacterStateParry)
		{
			condition = true;
		}
	}

}
