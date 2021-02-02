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

	public Input_Info()
    {
		inputActions = new List<InputBuffer>();

		horizontal = 0;
		vertical = 0;
	}
}

public class InputController : SerializedMonoBehaviour 
{
	List<Rewired.Player> players = new List<Player>();

	int[] playerID = new int[4];

	//EntityController[] entityControllers;
	//MenuController menuController;
	[OdinSerialize]
	public IControllable[] controllable = new IControllable[4];

	public Input_Info[] playerInputs = new Input_Info[4];
	public float bufferLength = 6;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	// Start is called before the first frame update
	void Start()
	{
		for (int i = 0; i < 4; i++)
		{
			players.Add(ReInput.players.GetPlayer(i));
			playerID[i] = players[i].id;
		}
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (playerInputs[i].inputActions.Count != 0)
			{
				var input = playerInputs[i].inputActions;
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

			Input_Interact();
			Input_Return();
			Input_Pause();
			//if (entityControllers.Length == 0)
			//{
			//	entityControllers = GameObject.FindObjectsOfType<EntityController>();
			//}
			//else
			//{
			Input_Horizontal(i);
			Input_Vertical(i);
			Input_Jump(i);
			Input_Attack(i);
			//}

			if(controllable[i] != null)
				controllable[i].UpdateControl(i, playerInputs[i]);
		}
	}

	void Input_Horizontal(int ID)
	{
		InputBuffer tmp = new InputBuffer();
		var input = playerInputs[ID].inputActions;
		playerInputs[ID].horizontal = players[ID].GetAxis("Horizontal");
	}

	void Input_Vertical(int ID)
	{
		InputBuffer tmp = new InputBuffer();
		var input = playerInputs[ID].inputActions;
		playerInputs[ID].vertical = players[ID].GetAxis("Vertical");
	}

	void Input_Jump(int ID)
	{
		if (players[ID].GetButtonDown("Jump"))
		{
			InputBuffer tmp = new InputBuffer();
			var input = playerInputs[ID].inputActions;
			foreach(InputBuffer ic in input)
            {
				if(ic.action == ReInput.mapping.GetAction("Jump"))
                {
					ic.timeValue = bufferLength;
					return;
                }
            }
			input.Add(tmp);
			input[input.Count - 1].action = ReInput.mapping.GetAction("Jump");
			input[input.Count - 1].timeValue = bufferLength;
		}
	}

	void Input_Attack(int ID)
	{
		if (players[ID].GetButtonDown("Attack"))
		{
			InputBuffer tmp = new InputBuffer();
			var input = playerInputs[ID].inputActions;
			foreach (InputBuffer ic in input)
			{
				if (ic.action == ReInput.mapping.GetAction("Attack"))
				{
					ic.timeValue = bufferLength;
					return;
				}
			}
			input.Add(tmp);
			input[input.Count - 1].action = ReInput.mapping.GetAction("Attack");
			input[input.Count - 1].timeValue = bufferLength;
		}
	}
	void Input_Interact()
	{

	}

	void Input_Return()
	{

	}

	void Input_Pause()
	{

	}
}