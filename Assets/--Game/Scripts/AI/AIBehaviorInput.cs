using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Base class for AI Behavior
public class AIBehaviorInput : AIBehavior
{

	[Title("Inputs")]
	[SerializeField]
	InputRecordingData inputData;

	[SerializeField]
	bool playRepeat = false;

	int indexPlay = 0;
	float playTime = 0f;


	void Update()
	{
		if (isActive == true)
		{
			playTime += 1;
			PlayInput();
		}
		else
		{
			character.UpdateControl(0, inputs);
		}
	}



	public override void StartBehavior()
	{
		base.StartBehavior();
		indexPlay = 0;
		playTime = 0f;
	}

	private void PlayInput()
	{
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActions);
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActionsUP);


		while (playTime > inputData.InputsRecorded[indexPlay].frame)
		{
			inputData.InputsRecorded[indexPlay].AssignInput(inputController, ref inputs);
			indexPlay += 1;
			if (indexPlay >= inputData.InputsRecorded.Count - 1)
			{
				StopBehavior();
				if (playRepeat == true)
					StartBehavior();
				return;
			}
		}


		character.UpdateControl(0, inputs);

		if (indexPlay >= inputData.InputsRecorded.Count)
		{
			StopBehavior();
			if (playRepeat == true)
				StartBehavior();
		}
	}

	public override void StopBehavior()
	{
		base.StopBehavior();
		indexPlay = 0;
		playTime = 0f;
	}
}
