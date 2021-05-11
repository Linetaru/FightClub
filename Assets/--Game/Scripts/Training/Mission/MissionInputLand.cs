using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputLand : MissionInputCondition
{
	[SerializeField]
	int numberOfLand = 0;

	int nbOfLand = 0;

	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		nbOfLand = 0;
		condition = false;
		player.OnStateChanged += CallbackCondition;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		nbOfLand = 0;
		condition = false;
		player.OnStateChanged -= CallbackCondition;
	}

	public void CallbackCondition(CharacterState oldState, CharacterState newState)
	{
		if(newState is CharacterStateLanding)
		{
			nbOfLand += 1;
			if(nbOfLand >= numberOfLand)
				condition = true;
		}
	}

}
