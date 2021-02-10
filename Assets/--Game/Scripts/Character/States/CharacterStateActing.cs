using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateActing : CharacterState
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{

	}

	public override void UpdateState(CharacterBase character)
	{
		character.Action.CanEndAction();
		// Mettre les inputs en dessous


	}

	public override void LateUpdateState(CharacterBase character)
	{
		character.Action.EndActionState();
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{

	}
}