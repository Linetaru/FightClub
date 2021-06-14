using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateAcumod : CharacterState
{

	[SerializeField]
	[SuffixLabel("en frames")]
	float timeAcumod = 20;

	float t = 0;

	// Start is called before the first frame update
	void Start()
	{
		timeAcumod /= 60;
	}


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = 0f;
	}

	public override void UpdateState(CharacterBase character)
	{
		t += Time.deltaTime;
		if (t > timeAcumod)
			character.ResetToIdle();
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}