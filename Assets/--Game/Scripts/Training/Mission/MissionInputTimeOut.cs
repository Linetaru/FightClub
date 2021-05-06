using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputTimeOut : MissionInputCondition
{
	[SerializeField]
	float time = 0;

	float t = 0f;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		t = 0f;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		t += Time.deltaTime;
		return (t >= time);
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		t = 0f;
	}


}
