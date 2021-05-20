using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateRespawn : CharacterState
{
	[SerializeField]
	float timeRespawn = 60f;

	[Title("Sticks")]
	[SerializeField]
	float stickThreshold = 0.3f;

	[Title("Feedbacks")]
	[SerializeField]
	Color colorFlash = Color.white;
	[SerializeField]
	float timeFlash = 0.1f;

	float t = 0f;
	float tFlash = 0f;

	private void Start()
	{
		timeRespawn /= 60f;
	}

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = 0f;
		Camera.main.GetComponent<CameraZoomController>().ModifyTargetPriority(character.transform, 0);
		//Camera.main.GetComponent<CameraZoomController>().targets.Add(character.gameObject.transform);

		character.transform.position = BlastZoneManager.Instance.spawnpoint.position;

		character.Knockback.IsInvulnerable = true;
	}

	public override void UpdateState(CharacterBase character)
	{
		t += Time.deltaTime;
		tFlash += Time.deltaTime;

		character.Movement.SpeedY = character.Movement.GravityMax * 0.05f;

		if (tFlash > timeFlash)
		{
			tFlash = 0f;
			character.Model.FlashModel(colorFlash, timeFlash);
		}

		if (t > timeRespawn)
		{
			character.ResetToIdle();
		}


		if(Mathf.Abs(character.Input.horizontal) > stickThreshold)
		{
			character.ResetToIdle();
		}
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{
		character.Knockback.IsInvulnerable = false;
	}
}