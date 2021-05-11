using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputGauge: MissionInputCondition
{
	[SerializeField]
	float gaugeNeeded = 0;


	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return (player.PowerGauge.CurrentPower >= gaugeNeeded);
	}




}
