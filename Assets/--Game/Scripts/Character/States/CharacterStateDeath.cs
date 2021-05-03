using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDeath : CharacterState
{
	[SerializeField]
	private CharacterState respawnState;

	[SerializeField]
	private GameObject playerObject; // Si on désactive l'objet avec l'animator y'a un bug trop chelou ou la rotation du perso est cassé, je scale a 0 du coup y'a sans doute mieux

	[SerializeField]
	private float timebeforeRespawn = 3.0f;
	private float timer = 0.0f;

    public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Stats.LifeStocks--;
		character.Stats.LifePercentage = 0;
		HidePlayer();
		character.Action.CancelAction();
		character.Knockback.IsInvulnerable = true;

		timer = 0f;
		//Camera.main.GetComponent<CameraZoomController>().targets.Remove(character.gameObject.transform);
		Camera.main.GetComponent<CameraZoomController>().targets.Remove(character.gameObject.transform);
		character.Movement.SetSpeed(0f, 0f);

	}

	public override void UpdateState(CharacterBase character)
	{
		if(!character.Stats.Death)
        {
			timer += Time.deltaTime;
			if (timer >= timebeforeRespawn)
			{
				Debug.Log("RESPAWNING");
				DisplayPlayer();
				character.SetState(respawnState);
			}
		}
	}

	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Knockback.IsInvulnerable = false;
		DisplayPlayer();
		//character.Stats.RespawnStats();
	}

	private void DisplayPlayer()
    {
		playerObject.transform.root.localScale = new Vector3(1, 1, 1);
		//playerObject.SetActive(true);
    }

	private void HidePlayer()
	{

		playerObject.transform.root.localScale = new Vector3(0, 0, 1);
		//playerObject.SetActive(false);
	}
}