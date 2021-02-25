using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateRespawn : CharacterState
{
	[SerializeField]
	CharacterState idleState;

    public override void StartState(CharacterBase character, CharacterState oldState)
	{
		Debug.Log("Respawn State");

		Camera.main.GetComponent<CameraController>().playersTarget.Add(character.gameObject);

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