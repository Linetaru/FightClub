using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputLand : MissionInputCondition
{

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return (player.CurrentState is CharacterStateLanding);
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		
	}

}
