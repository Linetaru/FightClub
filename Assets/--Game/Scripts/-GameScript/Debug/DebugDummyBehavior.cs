using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DummyBehavior
{
	Idle,
	Jump,
}

public class DebugDummyBehavior : AIBehavior
{

	int idleBehavior = 0;
	bool parryBehavior = false;
	bool canTech = false;


	public void SetBehavior(int behavior, bool parry, bool tech)
	{
		idleBehavior = behavior;
		parryBehavior = parry;
		canTech = tech;
	}


	// Update is called once per frame
	void Update()
	{
		if (character == null)
			return;
		if (isActive == false)
			return;

		BehaviorIdle();
		BehaviorParry();

		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActions);
		character.UpdateControl(0, inputs);
	}




	private void BehaviorIdle()
	{
		if (idleBehavior == 1) 
		{
			if (character.Knockback.KnockbackDuration <= 0)
			{
				inputController.AddInput(InputConst.Jump.name, ref inputs);
			}
		}
		else if (idleBehavior == 2)
		{
			if (character.Knockback.KnockbackDuration <= 0)
			{
				inputController.AddInput(InputConst.RightShoulder.name, ref inputs);
			}
		}
	}

	private void BehaviorParry()
	{
		if (character.Knockback.KnockbackDuration > 0)
		{
			if (parryBehavior)
				inputController.AddInput(InputConst.RightShoulder.name, ref inputs);

			/*if(character.Rigidbody.CheckGroundNear(0.2f) && canTech)
				inputController.AddInput(InputConst.RightTrigger.name, ref inputs);*/
		}
	}
}
