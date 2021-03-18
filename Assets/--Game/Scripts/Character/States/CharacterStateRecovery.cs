using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRecovery : CharacterState
{
	[SerializeField]
	GameObject recoveryAura;


	[SerializeField]
	float recoveryDuration = 1f;

	float timer = 0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		timer = 0f;
		recoveryAura.SetActive(true);
	}

	public override void UpdateState(CharacterBase character)
	{
		timer += Time.deltaTime;
		if(timer >= recoveryDuration)
        {
			character.ResetToIdle();
        }
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		recoveryAura.SetActive(false);
	}
}