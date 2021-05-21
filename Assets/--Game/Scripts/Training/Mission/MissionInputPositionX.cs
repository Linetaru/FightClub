using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputPositionX : MissionInputCondition
{
	[SerializeField]
	bool isDummy = false;
	[SerializeField]
	Vector2 distanceX = Vector2.zero;


	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		if (isDummy)
			return (distanceX.x < dummy.transform.position.x && dummy.transform.position.x < distanceX.y);
		else
			return (distanceX.x < player.transform.position.x && player.transform.position.x < distanceX.y);
	}



}
