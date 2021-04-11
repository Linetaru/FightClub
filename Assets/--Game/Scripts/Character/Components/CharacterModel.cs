using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class CharacterModel : MonoBehaviour
{
	[SerializeField]
	SkinnedMeshRenderer[] skinnedMeshRenderers;

	[SerializeField]
	TextMeshPro textPlayer;


	[Button]
	private void UpdateComponents()
	{
		skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	public void SetColor(int characterID, Material color)
	{
		if (textPlayer != null)
			textPlayer.text = (characterID + 1) + "P";

		for (int i = 0; i < skinnedMeshRenderers.Length; i++)
		{
			skinnedMeshRenderers[i].material = color;
		}
	}

	public Material GetColor()
	{
		return skinnedMeshRenderers[0].material;
	}


}
