using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class IntroductionCharacter : MonoBehaviour
{
	[ReadOnly]
	[SerializeField]
	CharacterModel[] characterModelPlayer;

	[ReadOnly]
	[SerializeField]
	Camera[] cameras;

	[Button]
	public void UpdateComponents()
	{
		cameras = GetComponentsInChildren<Camera>();
		characterModelPlayer = GetComponentsInChildren<CharacterModel>();
	}

	public void StartIntroduction(CharacterBase user, RenderTexture renderTexture)
	{
		this.gameObject.SetActive(true);
		this.transform.localScale = new Vector3(user.Movement.Direction, 1, 1);
		for (int i = 0; i < characterModelPlayer.Length; i++)
		{
			characterModelPlayer[i].SetColor(0, user.Model.GetColor());
		}

		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i].targetTexture = renderTexture;
		}
	}
}
