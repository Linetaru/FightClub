using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputSpacing : MissionInputCondition
{
	[SerializeField]
	float distanceMax = 0;


	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		if (player.transform.position.x > dummy.transform.position.x + 1)
			return true;
		else if (Vector3.Distance(player.transform.position, dummy.transform.position) > distanceMax)
			return true;
		return false;
	}



}
