using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class AIC_Attacks : MonoBehaviour
{


	[SerializeField]
	[ReadOnly]
	[ListDrawerSettings(Expanded = true)]
	AIC_AttacksDetector[] attackDatabase;


	//List<AIC_AttacksDetector> attacksActive = new List<AIC_AttacksDetector>();

	CharacterBase character;
	InputController inputController;
	Input_Info inputs;

	AIC_AttacksDetector currentAttack;
	public AIC_AttacksDetector CurrentAttack
	{
		get { return currentAttack; }
	}



	public void InitializeComponent(CharacterBase c, InputController input, Input_Info inputInfo)
	{
		character = c;
		inputController = input;
		inputs = inputInfo;

		for (int i = 0; i < attackDatabase.Length; i++)
		{
			attackDatabase[i].InitializeComponent(c, input, inputInfo);
		}
	}


	public bool CheckAttacks(CharacterBase target)
	{
		List<int> probaTable = new List<int>();
		int maxProba = 0;
		int proba = 0;
		for (int i = 0; i < attackDatabase.Length; i++)
		{
			proba = attackDatabase[i].CheckAttackPriority(target);
			maxProba += proba;
			probaTable.Add(proba);
		}
		if (maxProba == 0)
			return false;

		proba = Random.Range(0, maxProba);
		int index = 0;
		for (int i = 0; i < probaTable.Count; i++)
		{
			if(probaTable[i] < proba)
			{
				index += 1;
				proba -= probaTable[i];
			}
			else
			{
				break;
			}
		}
		currentAttack = attackDatabase[index];
		return true;
	}




	[Button]
	private void UpdateComponents()
	{
		attackDatabase = GetComponentsInChildren<AIC_AttacksDetector>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(GetComponent<BoxCollider>().center + this.transform.position, GetComponent<BoxCollider>().size);
	}
}
