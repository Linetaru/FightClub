using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for AI Behavior
public class AIBehavior : MonoBehaviour
{

	[SerializeField]
	protected CharacterBase character;
	[SerializeField]
	protected InputController inputController;
	[SerializeField]
	protected bool isActive = false;

	protected Input_Info inputs;


	// Start is called before the first frame update
	void Start()
	{
		inputs = new Input_Info();
	}

	// Update is called once per frame
	void Update()
	{
		character.UpdateControl(0, inputs);
	}

	public void SetCharacter(CharacterBase c, InputController input)
	{
		character = c;
		inputController = input;
		//inputController.controllable[Mathf.Abs(character.ControllerID)] = null;
	}

	public void ResetBehavior()
	{
		StopBehavior();
		StartBehavior();
	}

	public virtual void StartBehavior()
	{
		isActive = true;
	}

	public virtual void StopBehavior()
	{
		inputs = new Input_Info();
		isActive = false;
	}
}
