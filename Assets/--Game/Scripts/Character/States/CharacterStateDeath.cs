﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateDeath : CharacterState
{
	[SerializeField]
	private CharacterState respawnState;

	[SerializeField]
	private GameObject playerObject;

	private SkinnedMeshRenderer[] meshes;

	[SerializeField]
	private float timebeforeRespawn = 3.0f;
	private float timer = 0.0f;

    private void Awake()
    {
		meshes = playerObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Stats.LifeStocks--;
		character.Action.CancelAction();


		timer = 0f;
		Camera.main.GetComponent<CameraZoomController>().targets.Remove(character.gameObject.transform);
		character.Movement.SetSpeed(0f, 0f);
		HidePlayer();

	}

	public override void UpdateState(CharacterBase character)
	{
		timer += Time.deltaTime;
		if(timer >= timebeforeRespawn)
		{
			DisplayPlayer();
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

	private void DisplayPlayer()
    {
		foreach(SkinnedMeshRenderer mesh in meshes)
        {
			mesh.enabled = true;
        }
    }

	private void HidePlayer()
	{
		foreach (SkinnedMeshRenderer mesh in meshes)
		{
			mesh.enabled = false;
		}
	}
}