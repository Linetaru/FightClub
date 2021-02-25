using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[System.Serializable]
public class InputBuffer
{
	public float timeValue;
	public InputAction action;

	public InputBuffer()
	{
		timeValue = 0f;
		action = null;
	}
}

[System.Serializable]
public class Input_Info
{
	public float horizontal;
	public float vertical;

	public List<InputBuffer> inputActions;
	public List<InputBuffer> inputActionsUP;
	public Rewired.InputAction inputUiAction;

	public Input_Info()
    {
		inputActions = new List<InputBuffer>();
		inputUiAction = null;

		horizontal = 0;
		vertical = 0;
	}
	public bool CheckAction(int id, InputAction inputAction)
	{
		if (inputActions.Count != 0)
		{
			if(inputActions[id].action == inputAction)
            {
				return true;
            }
			else
				return false;
		}
		else
			return false;
	}
}

public class InputController : SerializedMonoBehaviour 
{
	List<Rewired.Player> players = new List<Player>();

	[OdinSerialize]
	public IControllable[] controllable = new IControllable[4];

	[ReadOnly] public Input_Info[] playerInputs = new Input_Info[4];
	public float bufferLength = 6;

	// Start is called before the first frame update
	void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			players.Add(ReInput.players.GetPlayer(i));
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (playerInputs[i].inputActions.Count != 0)
			{
				UpdateTimeInBuffer(playerInputs[i].inputActions);
			}
			if (playerInputs[i].inputActionsUP.Count != 0)
			{
				UpdateTimeInBuffer(playerInputs[i].inputActionsUP);
			}

			Input_Movement(i, InputConst.Horizontal.name);
			Input_Movement(i, InputConst.Vertical.name);
			Input_Action(i, InputConst.Jump.name);
			Input_Action(i, InputConst.Attack.name);
			Input_Action(i, InputConst.Smash.name);
			Input_ActionUI(i, InputConst.Pause.name);
			Input_ActionUI(i, InputConst.Interact.name);
			Input_ActionUI(i, InputConst.Return.name);

			if (controllable[i] != null)
			{
				controllable[i].UpdateControl(i, playerInputs[i]);
				if(playerInputs[i].inputUiAction != null)
					playerInputs[i].inputUiAction = null;
			}
		}
	}

	void UpdateTimeInBuffer(List<InputBuffer> input)
    {
		for (int z = input.Count - 1; z >= 0; z--)
		{
			if (input[z].action != null && input[z].timeValue > 0)
			{
				input[z].timeValue -= Time.deltaTime;
			}
			else if (input[z].action != null && input[z].timeValue <= 0)
			{
				input.Remove(input[z]);
			}
			else if (input[0].action == null && input[z].timeValue <= 0)
			{
				input.Remove(input[0]);
			}
		}
	}

	void Input_Movement(int ID, string axis)
	{
		InputBuffer tmp = new InputBuffer();
		var input = playerInputs[ID].inputActions;
		if(axis == InputConst.Horizontal.name)
			playerInputs[ID].horizontal = players[ID].GetAxis(axis);
		else
			playerInputs[ID].vertical = players[ID].GetAxis(axis);
	}

	void Input_Action(int ID, string action)
	{
		if (players[ID].GetButtonDown(action))
		{
			InputBuffer tmp = new InputBuffer();
			var input = playerInputs[ID].inputActions;
			foreach (InputBuffer ic in input)
			{
				if (ic.action == ReInput.mapping.GetAction(action))
				{
					ic.timeValue = bufferLength;
					return;
				}
			}
			input.Add(tmp);
			input[input.Count - 1].action = ReInput.mapping.GetAction(action);
			input[input.Count - 1].timeValue = bufferLength;
		}
		else if (players[ID].GetButtonUp(action))
		{
			InputBuffer tmp = new InputBuffer();
			var input = playerInputs[ID].inputActionsUP;
			foreach (InputBuffer ic in input)
			{
				if (ic.action == ReInput.mapping.GetAction(action))
				{
					ic.timeValue = bufferLength;
					return;
				}
			}
			input.Add(tmp);
			input[input.Count - 1].action = ReInput.mapping.GetAction(action);
			input[input.Count - 1].timeValue = bufferLength;
		}
	}

	void Input_ActionUI(int ID, string action)
	{
		if (players[ID].GetButtonDown(action))
		{
			playerInputs[ID].inputUiAction = ReInput.mapping.GetAction(action);
		}
	}

}