using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInputClash : MissionInputCondition
{
	bool condition = false;

	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		dummy.Knockback.Parry.OnClash += CallbackCondition;
		player.Knockback.Parry.OnClash += CallbackCondition;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		dummy.Knockback.Parry.OnClash -= CallbackCondition;
		player.Knockback.Parry.OnClash -= CallbackCondition;
	}

	public void CallbackCondition(AttackSubManager attackSubManager)
	{
		condition = true;
	}
}
