using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputParry : MissionInputCondition
{
	[SerializeField]
	bool isParry = true;
	[SerializeField]
	bool isDummy = false;

	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		if (isDummy == false)
		{
			if (isParry == true)
				player.Knockback.Parry.OnParry += CallbackCondition;
			else
				player.Knockback.Parry.OnGuard += CallbackCondition;
		}
		else
		{
			if (isParry == true)
				dummy.Knockback.Parry.OnParry += CallbackCondition;
			else
				dummy.Knockback.Parry.OnGuard += CallbackCondition;
		}
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		if (isDummy == false)
		{
			if (isParry == true)
				player.Knockback.Parry.OnParry -= CallbackCondition;
			else
				player.Knockback.Parry.OnGuard -= CallbackCondition;
		}
		else
		{
			if (isParry == true)
				dummy.Knockback.Parry.OnParry -= CallbackCondition;
			else
				dummy.Knockback.Parry.OnGuard -= CallbackCondition;
		}
	}

	public void CallbackCondition(CharacterBase characterParried)
	{
		condition = true;
	}

}
