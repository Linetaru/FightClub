using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputGauge: MissionInputCondition
{
	[SerializeField]
	float gaugeNeeded = 0;
	[SerializeField]
	bool superior = true;

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		if(superior)
			return (player.PowerGauge.CurrentPower >= gaugeNeeded);
		return (player.PowerGauge.CurrentPower <= gaugeNeeded);
	}




}
