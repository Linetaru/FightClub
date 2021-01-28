using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum State{
	Menu,
	UI_Pause,
	InGame,
}

[System.Serializable]
public class Input_Info
{
	//public Rewired.InputAction inputAction;
	public float horizontal;
	public float vertical;

	public List<Rewired.InputAction> inputActions;

	public Input_Info()
    {
		horizontal = 0;
		vertical = 0;

		inputActions = new List<InputAction>();
	}
}

public class InputController : MonoBehaviour
{
	List<Rewired.Player> players = new List<Player>();

	int[] playerID = new int[4];

	//EntityController[] entityControllers;
	//MenuController menuController;

	public State state;

	public Input_Info[] playerInputBuffer = new Input_Info[4];
	public int bufferLength = 6;

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
            switch (state)
            {
                case State.Menu:
					Input_Interact();
					Input_Return();
					break;
                case State.UI_Pause:
					Input_Interact();
					Input_Return();
					Input_Pause();
					break;
                case State.InGame:
					//if (entityControllers.Length == 0)
					//{
					//	entityControllers = GameObject.FindObjectsOfType<EntityController>();
					//}
					//else
					//{
						Input_Movement(i);
						Input_Jump(i);
						Input_Attack();
						Input_Pause();
					//}
					break;
            }
        }
    }

	void Input_Movement(int ID)
	{
		if (Mathf.Abs(players[ID].GetAxis("Horizontal")) > .35)
		{

		}
		else
		{

		}

	}

	void Input_Jump(int ID)
	{
		if (players[ID].GetButtonDown("Jump"))
		{
			playerInputBuffer[ID].inputActions.Add(ReInput.mapping.GetAction("Jump"));
		}
	}

	void Input_Attack()
	{

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