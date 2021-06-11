using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Lit un inputData après une réaction
public class AIBehaviorInputReact : AIBehavior
{
	[InfoBox("React à la parry uniquement actuellement")]
	[Title("Inputs")]
	[SerializeField]
	InputRecordingData[] inputDatas;

	InputRecordingData inputData;

	[SerializeField]
	EnumInput inputReact = 0;

	Rewired.InputAction inputToReact;

	[SerializeField]
	bool playRepeat = false;

	int indexPlay = 0;
	float playTime = 0f;

	bool react = false;
	int indexPlayer = 0;

	void Update()
	{
		if (isActive == true && react == false)
		{
			if (inputController.playerInputs[indexPlayer].CheckAction(0, inputToReact) == true)
			{
				react = true;
			}
		}
		if (isActive == true && react == true)
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
		inputData = inputDatas[Random.Range(0, inputDatas.Length)];
		react = false;

		// En mode Schlag
		for (int i = 0; i < inputController.controllable.Length; i++)
		{
			if(inputController.controllable[i] != null)
			{
				indexPlayer = i;
				break;
			}
		}
		switch(inputReact)
		{
			case EnumInput.R1:
				inputToReact = InputConst.RightShoulder;
				break;
			case EnumInput.A:
				inputToReact = InputConst.Attack;
				break;
			case EnumInput.Y:
				inputToReact = InputConst.Jump;
				break;
		}
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
		react = false;
	}
}
