using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputJump : MissionInputCondition
{

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return player.Input.CheckAction(0, InputConst.Jump);

	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		
	}

}
