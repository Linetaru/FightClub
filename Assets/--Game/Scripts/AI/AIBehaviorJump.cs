using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Base class for AI Behavior
public class AIBehaviorJump : AIBehavior
{


	void Update()
	{
		if(isActive == true)
			PlayInput();
		else
			character.UpdateControl(0, inputs);
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
