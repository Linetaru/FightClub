using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputLand : MissionInputCondition
{

	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		player.OnStateChanged += CallbackCondition;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		player.OnStateChanged -= CallbackCondition;
	}

	public void CallbackCondition(CharacterState oldState, CharacterState newState)
	{
		if(newState is CharacterStateLanding)
			condition = true;
	}

}
