using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDeath : CharacterState
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

	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Stats.RespawnStats();
	}
}