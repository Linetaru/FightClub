using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDeath : CharacterState
{
	[SerializeField]
	private CharacterState respawnState;

	[SerializeField]
	private SkinnedMeshRenderer renderer;

	[SerializeField]
	private float timebeforeRespawn = 3.0f;
	private float timer = 0.0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Stats.LifeStocks--;
		character.Action.CancelAction();
		Debug.Log("Death State");

		timer = 0f;
		Camera.main.GetComponent<CameraZoomController>().targets.Remove(character.gameObject.transform);
		character.Movement.SetSpeed(0f, 0f);
		renderer.enabled = false;

	}

	public override void UpdateState(CharacterBase character)
	{
		timer += Time.deltaTime;
		if(timer >= timebeforeRespawn)
		{
			renderer.enabled = true;
			character.SetState(respawnState);
        }
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		//character.Stats.RespawnStats();
	}
}