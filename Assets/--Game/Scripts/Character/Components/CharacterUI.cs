using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour
{
	public TextMeshProUGUI percentText;
	public GameObject panelUI;
	CharacterBase userCharacter;

	public void InitPlayerPanel(CharacterBase user)
    {
		panelUI.SetActive(!panelUI.activeSelf);
		percentText.text = user.Stats.LifePercentage.ToString() + " %";
		userCharacter = user;
	}

	public void UpdateUI()
    {
		percentText.text = userCharacter.Stats.LifePercentage.ToString() + " %";
	}
}