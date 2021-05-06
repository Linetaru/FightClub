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

	bool playAtStart = false;
	bool playRepeat = false;

	bool isPlaying = false;
	int indexPlay = 0;
	float playTime = 0f;

	private void Awake()
	{
		if (playAtStart == true)
			StartPlayInput();
	}

	void Update()
	{
		if (isPlaying == false)
			return;
		PlayInput();
	}

	public void StartPlayInput()
	{
		indexPlay = 0;
		playTime = 0f;
		isPlaying = false;
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
				StopPlayInput();
				return;
			}
		}


		character.UpdateControl(0, inputs);

		if (indexPlay >= inputData.InputsRecorded.Count)
			StopPlayInput();
	}

	public void StopPlayInput()
	{
		indexPlay = 0;
		playTime = 0f;
		isPlaying = false;

		if (playRepeat == true)
			StartPlayInput();
	}
}
