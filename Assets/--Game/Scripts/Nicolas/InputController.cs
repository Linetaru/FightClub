using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

//Class to create Buffer for each input, stock Rewired action and time before action disapear
[System.Serializable]
public class InputBuffer
{
	public float timeValue;
	public InputAction action;
	public bool hold;

	public InputBuffer()
	{
		timeValue = 0f;
		action = null;
	}
}

//Class for each player can use to have input reference and know each input is use.
[System.Serializable]
public class Input_Info
{
	public float horizontal;
	public float vertical;

	public List<InputBuffer> inputActions;
	public List<InputBuffer> inputActionsUP;

	public List<InputAction> inputActionsHold;

	public Rewired.InputAction inputUiAction;

	public Input_Info()
    {
		inputActions = new List<InputBuffer>();
		inputActionsUP = new List<InputBuffer>();
		inputActionsHold = new List<InputAction>();
		inputUiAction = null;

		horizontal = 0;
		vertical = 0;
	}

	//Check Action can reduce code to help you to check if list of input is different from 0 and then check if input u want to know is in this list
	public bool CheckAction(int id, InputAction inputAction)
	{
		if (inputActions.Count != 0)
		{
			if(inputActions[id].action == inputAction)
            {
				return true;
            }
		}
		return false;
	}

	public bool CheckActionUI(InputAction inputAction)
	{
		if (inputUiAction == inputAction)
		{
			inputUiAction = null;
			return true;
		}
		return false;
	}

	public bool CheckActionUP(int id, InputAction inputAction)
	{
		if (inputActionsUP.Count != 0)
		{
			if (inputActionsUP[id].action == inputAction)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckActionHold(InputAction inputAction)
	{
		return inputActionsHold.Contains(inputAction);
	}

}

//Main class for Input Management, Send input to all player attached to this controller And manage input buffer for each player.
public class InputController : SerializedMonoBehaviour 
{
	List<Rewired.Player> players = new List<Player>();

	//All Entity using Input is stocked in this array to be able to use input
	[OdinSerialize]
	public IControllable[] controllable = new IControllable[4];

	[ReadOnly] public Input_Info[] playerInputs = new Input_Info[4];

	//Buffer Length is start time before input is removed for each input in buffer
	public float bufferLength = 6;

	public PackageCreator.Event.GameEvent pauseEvent;

	// Start will add all player Referenced by Rewired
	void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			players.Add(ReInput.players.GetPlayer(i));
		}
	}

	// Will Update Time action in Buffer, Check if a input is push or unpush and Send at each entity all input in their linked lists 
	void Update()
	{
		//Repeat for each Entity
		for (int i = 0; i < 4; i++)
		{
			//Update Time of each down action in each Entity buffer and remove them if time has come to zero  
			if (playerInputs[i].inputActions.Count != 0)
			{
				UpdateTimeInBuffer(playerInputs[i].inputActions);
			}

			//Update Time of each up action in each Entity buffer and remove them if time has come to zero  
			if (playerInputs[i].inputActionsUP.Count != 0)
			{
				UpdateTimeInBuffer(playerInputs[i].inputActionsUP);
			}


			//Check if Movement Axis is moving to reference in each buffer
			Input_Movement(i, InputConst.Horizontal.name);
			Input_Movement(i, InputConst.Vertical.name);

			//Check if a Action is using to reference in each buffer
			Input_Action(i, InputConst.Jump.name);
			Input_Action(i, InputConst.Attack.name);
			Input_Action(i, InputConst.Smash.name);
			Input_Action(i, InputConst.LeftShoulder.name);
			Input_Action(i, InputConst.RightShoulder.name);
			Input_Action(i, InputConst.Dodge.name);
			Input_Action(i, InputConst.Grab.name);
			Input_Action(i, InputConst.SignatureMove.name);
			Input_Action(i, InputConst.UpTaunt.name);
			Input_Action(i, InputConst.LeftTaunt.name);
			Input_Action(i, InputConst.DownTaunt.name);
			Input_Action(i, InputConst.RightTaunt.name);
			Input_Action(i, InputConst.LeftTrigger.name);
			Input_Action(i, InputConst.RightTrigger.name);
			Input_Action(i, InputConst.Special.name);

			//Check if a Action UI is using to reference in each buffer
			Input_Action(i, InputConst.Pause.name);
			Input_Action(i, InputConst.Back.name);

			Input_ActionUI(i, InputConst.Pause.name);
			Input_ActionUI(i, InputConst.Jump.name);
			Input_ActionUI(i, InputConst.Interact.name);
			Input_ActionUI(i, InputConst.Return.name);

			if (pauseEvent != null)
				if (playerInputs[i].inputUiAction == InputConst.Pause)
				{
					pauseEvent.Raise();
				}

			//If we got at least one entity will send to each entity their linked list for input buffer
			if (controllable[i] != null)
			{
				controllable[i].UpdateControl(i, playerInputs[i]);
				if(playerInputs[i].inputUiAction != null && Time.timeScale > 0)
					playerInputs[i].inputUiAction = null;
			}

		}
	}

	//Update Time of each action in Entity buffer and remove them if time has come to zero, or action is null
	public void UpdateTimeInBuffer(List<InputBuffer> input)
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


	//Check if Movement Axis is moving to reference in each buffer
	void Input_Movement(int ID, string axis)
	{
		InputBuffer tmp = new InputBuffer();
		var input = playerInputs[ID].inputActions;
		if(axis == InputConst.Horizontal.name)
			playerInputs[ID].horizontal = players[ID].GetAxis(axis);
		else
			playerInputs[ID].vertical = players[ID].GetAxis(axis);
	}

	//Check if a Action is using to reference in each buffer
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

			playerInputs[ID].inputActionsHold.Add(ReInput.mapping.GetAction(action));
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
					playerInputs[ID].inputActionsHold.Remove(ReInput.mapping.GetAction(action));
					return;
				}
			}
			input.Add(tmp);
			input[input.Count - 1].action = ReInput.mapping.GetAction(action);
			input[input.Count - 1].timeValue = bufferLength;

			playerInputs[ID].inputActionsHold.Remove(ReInput.mapping.GetAction(action));
		}
	}


	//Check if a Action UI is using to reference in each buffer
	void Input_ActionUI(int ID, string action)
	{
		if (players[ID].GetButtonDown(action))
		{
			playerInputs[ID].inputUiAction = ReInput.mapping.GetAction(action);
		}
	}









	public void AddInput(string action, ref Input_Info inputInfo)
	{

		InputBuffer tmp = new InputBuffer();
		var input = inputInfo.inputActions;
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

	public void AddMovement(float horizontal, float vertical, ref Input_Info inputInfo)
	{
		/*InputBuffer tmp = new InputBuffer();
		var input = inputInfo.inputActions;*/
		inputInfo.horizontal = horizontal;
		inputInfo.vertical = vertical;
	}

}