using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Calcule des données affichés sur l'écran de Win
public class CharacterBattleData : MonoBehaviour
{

	[SerializeField]
	CharacterBase character;


	CharacterBase lastAttacker;



	List<CharacterBase> killed = new List<CharacterBase>();
	public List<CharacterBase> Killed
	{
		get { return killed; }
	}

	List<CharacterBase> killer = new List<CharacterBase>();
	public List<CharacterBase> Killer
	{
		get { return killer; }
	}

	private int nbOfParry;
	public int NbOfParry
	{
		get { return nbOfParry; }
	}

	public List<string> attackUsed = new List<string>();
	public List<int> attackNbUsed = new List<int>();

	private void Awake()
	{
		character.OnStateChanged += CallbackState;
		character.Knockback.OnKnockback += CallbackKnockback;
		character.Knockback.Parry.OnParry += CallbackParry;
		character.Action.OnAttack += CallbackAttack;
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
			// Pardon
			if (lastAttacker != null)
			{
				lastAttacker.gameObject.GetComponentInChildren<CharacterBattleData>().Killed.Add(character);
				killer.Add(lastAttacker);
				lastAttacker = null;
			}
			else
			{
				killer.Add(character);
			}
		}
		if (newState is CharacterStateIdle) 
		{
			lastAttacker = null;
		}
	}

	public void CallbackAttack(AttackManager attack)
	{
		for (int i = 0; i < attackUsed.Count; i++)
		{
			if(attackUsed[i].Equals(attack.gameObject.name))
			{
				attackNbUsed[i] += 1;
			}
		}
		attackUsed.Add(attack.gameObject.name);
		attackNbUsed.Add(1);
	}


	private void OnDestroy()
	{
		character.OnStateChanged -= CallbackState;
		character.Knockback.OnKnockback -= CallbackKnockback;
		character.Knockback.Parry.OnParry -= CallbackParry;
		character.Action.OnAttack -= CallbackAttack;
	}

}
