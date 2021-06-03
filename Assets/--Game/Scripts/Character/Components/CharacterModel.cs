using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class CharacterModel : MonoBehaviour
{
	[SerializeField]
	Material flashMaterial;
	[SerializeField]
	SkinnedMeshRenderer[] skinnedMeshRenderers;

	[SerializeField]
	TextMeshPro textPlayer;

	private IEnumerator flashCoroutine = null;

	[Button]
	private void UpdateComponents()
	{
		skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
	}

	public void SetColor(int characterID, Material color)
	{
		if (textPlayer != null)
			textPlayer.text = (characterID + 1) + "P";

		Material[] materials;
		if (flashMaterial != null)
		{
			materials = new Material[2];
			materials[0] = color;
			materials[1] = flashMaterial;
		}
		else
		{
			materials = new Material[1];
			materials[0] = color;
		}

		for (int i = 0; i < skinnedMeshRenderers.Length; i++)
		{
			//skinnedMeshRenderers[i].material = color;
			skinnedMeshRenderers[i].materials = materials;
		}


	}

	public void SetTextColor(Color color)
	{
		if (textPlayer != null)
			textPlayer.color = color;
	}

	public Material GetColor()
	{
		return skinnedMeshRenderers[0].material;
	}

	[Button]
	public void FlashModel()
	{
		if(this.gameObject.activeInHierarchy == true)
			FlashModelCoroutine(Color.white, 1f);
		//StartCoroutine(FlashCoroutine(Color.white, 1f));
	}

	public void FlashModel(Color flashColor, float time)
	{
		if (this.gameObject.activeInHierarchy == true)
			FlashModelCoroutine(flashColor, time);
			//StartCoroutine(FlashCoroutine(flashColor, time));
	}

	public void FlashModelCoroutine(Color flashColor, float time)
	{
		if (flashCoroutine != null)
			StopCoroutine(flashCoroutine);
		flashCoroutine = FlashCoroutine(flashColor, time);
		StartCoroutine(flashCoroutine);
	}

	private IEnumerator FlashCoroutine(Color flashColor, float time)
	{
		if (skinnedMeshRenderers[0].materials.Length < 2)
			yield break;
		float t = 0f;
		Color c = new Color(flashColor.r, flashColor.g, flashColor.b, 1);
		Color transparent = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
		while (t < time)
		{
			t += Time.deltaTime;
			c = Color.Lerp(c, transparent, t / time);
			for (int i = 0; i < skinnedMeshRenderers.Length; i++)
			{
				skinnedMeshRenderers[i].materials[1].SetColor("_UnlitColor", c);
			}
			yield return null;
		}
		flashCoroutine = null;
	}


}
