using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Pour valider la condition il faut que l'attaque touche
public class MissionInputAttack : MissionInputCondition
{
	[SerializeField]
	[HideLabel]
	[HorizontalGroup]
	AttackManager attack;

	[SerializeField]
	[HorizontalGroup]
	[ListDrawerSettings(Expanded = true)]
	int[] hitboxID = { 0 };

	[SerializeField]
	bool playerReceiveAttack = false;

	bool condition = false;


	public override void InitializeCondition(CharacterBase player, CharacterBase dummy)
	{
		condition = false;
		if(playerReceiveAttack)
			player.Knockback.OnKnockback += KnockbackCallback;
		else
			dummy.Knockback.OnKnockback += KnockbackCallback;
	}

	public override bool UpdateCondition(CharacterBase player, CharacterBase dummy)
	{
		return condition;
	}

	public override void EndCondition(CharacterBase player, CharacterBase dummy)
	{
		if (playerReceiveAttack)
			player.Knockback.OnKnockback -= KnockbackCallback;
		else
			dummy.Knockback.OnKnockback -= KnockbackCallback;
	}

	public void KnockbackCallback(AttackSubManager attackSubManager)
	{
		for (int i = 0; i < hitboxID.Length; i++)
		{
			if (attackSubManager.AttackID == attack.name + "(Clone)" + hitboxID[i]) // le clone y'a pas le choix
			{
				condition = true;
			}
		}

	}
}
