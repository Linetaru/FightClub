using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRespawn : CharacterState
{
	[SerializeField]
	CharacterState idleState;

    public override void StartState(CharacterBase character, CharacterState oldState)
	{


		Camera.main.GetComponent<TestCamera>().Targets.Add(character.gameObject.transform);

		character.transform.position = BlastZoneManager.Instance.spawnpoint.position;

		//character.SetState(idleState);  ACTIVER CELUI LA PLUTOT QUE CELUI DANS LE UPDATE QUAND BUG FIX

	}

	public override void UpdateState(CharacterBase character)
	{
		//TMP
		character.SetState(idleState);
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{

	}
}