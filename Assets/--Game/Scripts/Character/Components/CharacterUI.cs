using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour
{
	[ReadOnly] public TextMeshProUGUI percentText;
	[ReadOnly] public GameObject panelUI;
	CharacterBase userCharacter;

	public void InitPlayerPanel(CharacterBase user, GameObject panelPlayer, TextMeshProUGUI percentPlayer)
    {
		panelUI = panelPlayer;
		percentText = percentPlayer;

		panelUI.SetActive(!panelUI.activeSelf);
		percentText.text = user.Stats.LifePercentage.ToString() + " %";
		userCharacter = user;
	}

	public void UpdateUI()
    {
		percentText.text = userCharacter.Stats.LifePercentage.ToString() + " %";
	}
}