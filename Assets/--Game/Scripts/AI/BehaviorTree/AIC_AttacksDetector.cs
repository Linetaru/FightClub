using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider))]
public class AIC_AttacksDetector : MonoBehaviour
{
	[SerializeField]
	[PropertyRange(0, 100)]
	int probabilityIsSelected = 100;

	[SerializeField]
	int priority = 1;
	[SerializeField]
	int priorityProbability = 100;

	List<CharacterBase> targets = new List<CharacterBase>();

	CharacterBase character;
	InputController inputController;
	Input_Info inputs;

	public void InitializeComponent(CharacterBase c, InputController input, Input_Info inputInfo)
	{
		character = c;
		inputController = input;
		inputs = inputInfo;
	}


	public int CheckAttackPriority(CharacterBase target)
	{
		if (Random.Range(0, 100) > probabilityIsSelected)
			return 0;
		if (!targets.Contains(target))
			return 0;
		return priorityProbability;
	}







	[Space]
	[Title("Attack Behavior")]
	[SerializeField]
	[HideLabel]
	DebugInput inputAttack;

	// Fonction qui controle l'IA, renvois faux quand l'IA arrête de 
	public virtual void StartAttack(CharacterBase target)
	{
		inputAttack.AssignInput(inputController, ref inputs);
	}

	// Fonction qui controle l'IA, renvois faux quand l'IA arrête de 
	public virtual bool UpdateAttack(CharacterBase target)
	{
		if (character.CurrentState is CharacterStateActing && character.Action.CharacterHit != null)
			return false;
		if (character.CurrentState is CharacterStateActing)
			return true;
		return false;
	}









	protected void OnTriggerEnter(Collider other)
	{
		CharacterBase c = other.GetComponent<CharacterBase>();
		if (c != null)
		{
			if (!targets.Contains(c))
				targets.Add(c);
		}
	}

	protected void OnTriggerExit(Collider other)
	{
		CharacterBase c = other.GetComponent<CharacterBase>();
		if (c != null)
		{
			if (targets.Contains(c))
				targets.Remove(c);
		}
	}




	[Space]
	[Title("Debug")]
	[SerializeField]
	AttackManager attackManager;

	[Button]
	private void GenerateBoxCollider()
	{
		GetComponent<BoxCollider>().center = attackManager.GetComponentInChildren<BoxCollider>().center;
		GetComponent<BoxCollider>().size = attackManager.GetComponentInChildren<BoxCollider>().size;
	}


}
