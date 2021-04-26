using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallKickOffCharacterState : CharacterState
{
	[SerializeField]
	CharacterState BallIdleState;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Stats.LifePercentage = 80f;
	}

	public override void UpdateState(CharacterBase character)
	{

	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}