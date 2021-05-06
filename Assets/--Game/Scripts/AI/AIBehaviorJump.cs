using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Base class for AI Behavior
public class AIBehaviorJump : AIBehavior
{

	[SerializeField]
	bool playAtStart = false;


	void Update()
	{
		PlayInput();
	}


	private void PlayInput()
	{
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActions);
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActionsUP);

		if(character.Rigidbody.IsGrounded == true)
			inputController.AddInput(InputConst.Jump.name, ref inputs);

		character.UpdateControl(0, inputs);

	}

}
