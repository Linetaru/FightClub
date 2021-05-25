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

	float cooldownNotSelected = 0f;

	public void InitializeComponent(CharacterBase c, InputController input, Input_Info inputInfo)
	{
		character = c;
		inputController = input;
		inputs = inputInfo;
	}


	public int CheckAttackPriority(CharacterBase target)
	{
		if (!targets.Contains(target))
			return 0;
		if (cooldownNotSelected > 0)
			return 0;
		if (Random.Range(0, 100) > probabilityIsSelected)
		{
			cooldownNotSelected = 0.4f;
			return 0;
		}
		return priorityProbability;
	}







	[Space]
	[Title("Attack Behavior")]
	[SerializeField]
	[HideLabel]
	DebugInput inputAttack;

	private float t = 0;
	public bool inputWithDirection = true;

	// Fonction qui controle l'IA, renvois faux quand l'IA arrête de 
	public virtual void StartAttack(CharacterBase target)
	{
		if (inputWithDirection)
			inputAttack.horizontal = character.Movement.Direction;
		inputAttack.AssignInput(inputController, ref inputs);
		t = 0;
	}

	// Fonction qui controle l'IA, renvois faux quand l'IA arrête de 
	public virtual bool UpdateAttack(CharacterBase target)
	{
		// On attend viteuf quelques frames pour laisser le perso passer en state acting
		t += Time.deltaTime;
		if (t < 0.2f)
			return true;


		if (character.CurrentState is CharacterStateActing && character.Action.CharacterHit != null)
			return false;
		if (character.CurrentState is CharacterStateActing)
			return true;
		return false;
	}






	private void Update()
	{
		if (cooldownNotSelected > 0)
			cooldownNotSelected -= Time.deltaTime;
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
