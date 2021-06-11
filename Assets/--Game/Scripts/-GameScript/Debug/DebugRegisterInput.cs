using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
public enum EnumInput
{
	A,
	B,
	X,
	Y,
	R1,
	R2
}

[System.Serializable]
public class DebugInput
{
	[HorizontalGroup("Debug")]
	[VerticalGroup("Debug/Left")]
	[SerializeField]
	public float frame;

	[VerticalGroup("Debug/Left")]
	[SerializeField]
	[Range(-1, 1)]
	public float horizontal;
	[VerticalGroup("Debug/Left")]
	[Range(-1, 1)]
	[SerializeField]
	public float vertical;

	[VerticalGroup("Debug/Right")]
	[SerializeField]
	[HideLabel]
	public List<int> inputs;

	public DebugInput(float horizontal, float vertical)
	{
		frame = 0;
		inputs = new List<int>();

		this.horizontal = horizontal;
		this.vertical = vertical;

	}
	public DebugInput(float horizontal, float vertical, int input)
	{
		frame = 0;
		inputs = new List<int>();

		this.horizontal = horizontal;
		this.vertical = vertical;
		inputs.Add(input);

	}

	// Pour enregistrer les inputs
	public DebugInput(float timestamp, Input_Info input_Info)
	{
		frame = timestamp;
		inputs = new List<int>();

		if (input_Info == null)
			return;

		horizontal = input_Info.horizontal;
		vertical = input_Info.vertical;


		if (input_Info.CheckAction(0, InputConst.Jump))
			inputs.Add(0);
		else if (input_Info.CheckAction(0, InputConst.Attack))
			inputs.Add(1);
		else if (input_Info.CheckAction(0, InputConst.Special))
			inputs.Add(2);
		else if (input_Info.CheckAction(0, InputConst.RightShoulder))
			inputs.Add(4);
		else if (input_Info.CheckAction(0, InputConst.RightTrigger))
			inputs.Add(5);
	}



	// Pour jouer les inputs
	public void AssignInput(InputController inputController, ref Input_Info input)
	{
		inputController.AddMovement(horizontal, vertical, ref input);

		for (int i = 0; i < inputs.Count; i++)
		{
			if (inputs[i] == 0)
				inputController.AddInput(InputConst.Jump.name, ref input);
			if (inputs[i] == 1)
				inputController.AddInput(InputConst.Attack.name, ref input);
			if (inputs[i] == 2)
				inputController.AddInput(InputConst.Special.name, ref input);
			if (inputs[i] == 4)
				inputController.AddInput(InputConst.RightShoulder.name, ref input);
			if (inputs[i] == 5)
				inputController.AddInput(InputConst.RightTrigger.name, ref input);
		}
	}



}

public class DebugRegisterInput : MonoBehaviour
{
	[SerializeField]
	InputController inputController;
	[SerializeField]
	int characterID = 0;
	[SerializeField]
	float maxTimeRecord = 1000;

	[SerializeField]
	List<DebugInput> debugInputs;

	[Title("UI")]
	[SerializeField]
	TMPro.TextMeshProUGUI textRecord;

	IControllable character;


	[HideInInspector]
	public bool registerInput = false;
	[HideInInspector]
	public bool playInput = false;

	float playTime = 0;
	Input_Info inputs;
	int indexPlay = 0;


	// =========================================================
	// R E C O R D
	// =========================================================
	[Button]
	public void StartRegisterInput()
	{
		playTime = 0f;
		registerInput = true;
		debugInputs.Clear();

		textRecord.text = "Recording J" + characterID;
		textRecord.gameObject.SetActive(true);
	}

	public void RegisterInput()
	{
		debugInputs.Add(new DebugInput(playTime, inputController.playerInputs[characterID]));
		if (playTime >= maxTimeRecord)
			StopRegisterInput();
	}

	[Button]
	public void StopRegisterInput()
	{
		registerInput = false;


		textRecord.gameObject.SetActive(false);
	}






	// =========================================================
	// P L A Y
	// =========================================================
	[Button]
	public void StartPlayInput()
	{
		if (debugInputs.Count == 0)
			return;
		if(inputController.controllable[characterID] == null)
		{
			StopPlayInput();
			return;
		}
		indexPlay = 0;
		playTime = 0f;
		playInput = true;
		inputs = new Input_Info();
		character = inputController.controllable[characterID];
		inputController.controllable[characterID] = null;

		textRecord.text = "Play Record J" + characterID;
		textRecord.gameObject.SetActive(true);
	}

	public void PlayInput()
	{
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActions);
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActionsUP);


		while (playTime > debugInputs[indexPlay].frame)
		{
			debugInputs[indexPlay].AssignInput(inputController, ref inputs);
			indexPlay+=1;
			if (indexPlay >= debugInputs.Count - 1)
			{
				StopPlayInput();
				return;
			}

		}


		character.UpdateControl(0, inputs);

		if (indexPlay >= debugInputs.Count)
			StopPlayInput();
	}

	public void StopPlayInput()
	{
		indexPlay = 0;
		playTime = 0f;
		playInput = false;
		inputs = new Input_Info();

		inputController.controllable[characterID] = character;
		character = null;

		textRecord.gameObject.SetActive(false);
	}



	private void Update()
	{
		if(playInput == true)
		{
			playTime += 1;//Time.deltaTime;
			PlayInput();
		}
		if (registerInput == true)
		{
			playTime += 1;//Time.deltaTime;
			RegisterInput();
		}
	}
}
