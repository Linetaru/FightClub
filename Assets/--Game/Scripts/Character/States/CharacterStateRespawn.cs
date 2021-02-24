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

		character.transform.position = new Vector3(-2.08f, 2f, 0f);

		character.SetState(idleState);
	}

	public override void UpdateState(CharacterBase character)
	{
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{

	}
}