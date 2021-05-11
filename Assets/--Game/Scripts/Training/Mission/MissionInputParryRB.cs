using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut parry avec RB/R1
public class MissionInputParryRB : MissionInputCondition
{
	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return player.Input.CheckAction(0, InputConst.RightShoulder);
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;

	}

}
