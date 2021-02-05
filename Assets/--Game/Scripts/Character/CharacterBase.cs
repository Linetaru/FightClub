using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IControllable
{
	[SerializeField]
	CharacterState currentState;

	private Input_Info input;
	public Input_Info Input
	{
		get { return input; }
	}


	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
	}


	public void SetState(CharacterState characterState)
	{
		if(currentState != null)
			currentState.EndState(this);
		currentState = characterState;
		currentState.StartState(this);
	}


	// Update is called once per frame
	/*void Update()
	{
		currentState.UpdateState(this);
	}*/

	public void UpdateControl(int ID, Input_Info input_Info)
	{
		input = input_Info;
		currentState.UpdateState(this);
	}
}
