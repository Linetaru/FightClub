using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
	[SerializeField]
	CharacterState currentState;

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
	void Update()
	{
		currentState.UpdateState(this);
	}
}
