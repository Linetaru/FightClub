using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignatureMove : MonoBehaviour
{
	[SerializeField]
	CharacterModel[] characterModelPlayer;

	// Placeholder vu qu'on a que un perso pour le moment
	[SerializeField]
	CharacterModel[] characterModelEnemy;


	CharacterBase target;
	public delegate void Action();
	public event Action OnEnd;

	public void StartSignatureMove(CharacterBase user, CharacterBase target)
	{
		for (int i = 0; i < characterModelPlayer.Length; i++)
		{
			characterModelPlayer[i].SetColor(0, user.Model.GetColor());
		}

		// Placeholder
		for (int i = 0; i < characterModelEnemy.Length; i++)
		{
			characterModelEnemy[i].SetColor(0, target.Model.GetColor());
		}


		this.target = target;
	}


	public void AddDamage(float damage)
	{
		target.Stats.LifePercentage += damage;
	}

	public void EndSignatureMove()
	{		
		OnEnd.Invoke();
		Destroy(this.gameObject);
	}
}
