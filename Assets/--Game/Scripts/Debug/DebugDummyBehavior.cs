using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DummyBehavior
{
	Idle,
	Jump,
	Dodge
}

public class DebugDummyBehavior : MonoBehaviour
{
	[SerializeField]
	InputController inputController;
	[SerializeField]
	int characterID;

	CharacterBase characterBase;

	Input_Info input;

	//bool knockbackOn = false;
	DummyBehavior behavior;

	// Start is called before the first frame update
	void Start()
	{
		input = new Input_Info();
	}


	public void SetBehaviorToCharacter(int id, DummyBehavior dummyBehavior)
	{

		behavior = dummyBehavior;
		if (dummyBehavior == DummyBehavior.Idle && characterBase != null)
		{
			characterID = id;
			inputController.controllable[id] = characterBase;
			characterBase = null;
			return;
		}

		if (inputController.controllable[id] != null)
		{
			characterID = id;
			characterBase = (CharacterBase)inputController.controllable[id];
			inputController.controllable[id] = null;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (characterBase == null)
			return;

		if (behavior == DummyBehavior.Jump)
			BehaviorJump();
		else if (behavior == DummyBehavior.Dodge)
			BehaviorDodge();

		characterBase.UpdateControl(0, input);
	}




	public void BehaviorJump()
	{
		if (characterBase.Knockback.KnockbackDuration > 0)
		{
			inputController.AddInput(InputConst.Jump.name, ref input);
		}
		else
		{
			if (input.inputActions.Count != 0)
				inputController.UpdateTimeInBuffer(input.inputActions);
		}
	}


	public void BehaviorDodge()
	{

	}



}
