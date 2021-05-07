using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputAttackWhiff : MissionInputCondition
{
	[SerializeField]
	[HideLabel]
	[HorizontalGroup]
	AttackManager attack;


	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		if (player.Action.CurrentAttackManager == null)
			return false;
		return (player.Action.CurrentAttackManager.name == (attack.name + "(Clone)"));
	}


}
