using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Calcule des données affichés sur l'écran de Win
public class CharacterBattleData : MonoBehaviour
{

	[SerializeField]
	CharacterBase character;


	CharacterBase lastAttacker;
	List<CharacterBase> killer = new List<CharacterBase>();


	private int nbOfParry;
	public int NbOfParry
	{
		get { return nbOfParry; }
	}

	public Dictionary<AttackManager, int> attackUsed = new Dictionary<AttackManager, int>();

	private void Awake()
	{
		character.OnStateChanged += CallbackState;
		character.Knockback.OnKnockback += CallbackKnockback;
		character.Knockback.Parry.OnParry += CallbackParry;
	}


	public void CallbackParry(CharacterBase c)
	{
		nbOfParry += 1;
	}

	public void CallbackKnockback(AttackSubManager attackSubManager)
	{
		lastAttacker = attackSubManager.User;
	}

	public void CallbackState(CharacterState oldState, CharacterState newState)
	{
		if(newState is CharacterStateDeath)
		{
			killer.Add(lastAttacker);
			lastAttacker = null;
		}
		if (newState is CharacterStateIdle) 
		{
			lastAttacker = null;
		}
	}

	public void CallbackAttack(AttackSubManager attack)
	{
		nbOfParry += 1;
	}



	private void OnDestroy()
	{
		character.Knockback.Parry.OnParry -= CallbackParry;
	}

}
