using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterBase : MonoBehaviour, IControllable
{
	[SerializeField]
	CharacterState currentState;

	[Title("Components")]
	[SerializeField]
	private CharacterRigidbody rigidbody;
	public CharacterRigidbody Rigidbody
	{
		get { return rigidbody; }
	}

	[SerializeField]
	private CharacterMovement movement;
	public CharacterMovement Movement
	{
		get { return movement; }
	}

	[SerializeField]
	private CharacterAction action;
	public CharacterAction Action
	{
		get { return action; }
	}



	private Input_Info input;
	public Input_Info Input
	{
		get { return input; }
	}

	public delegate void ActionSetState(CharacterState oldState, CharacterState newState);
	public event ActionSetState OnStateChanged;



	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		action.InitializeComponent(this);
	}


	public void SetState(CharacterState characterState)
	{
		if(currentState != null)
			currentState.EndState(this, characterState);
		characterState.StartState(this, currentState);

		OnStateChanged?.Invoke(currentState, characterState);
		currentState = characterState;
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
		rigidbody.UpdateCollision(movement.SpeedX * movement.Direction, movement.SpeedY);
		currentState.LateUpdateState(this);
	}
}
