using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour
{
	public GameObject panelUI;
	public TextMeshProUGUI percentText;

	public void InitPlayerPanel(CharacterBase user)
    {
        panelUI.SetActive(!panelUI.activeSelf);
		percentText.text = user.Stats.LifePercentage.ToString() + " %";
	}

	public void UpdateUI(float percent)
    {
		percentText.text = percent.ToString() + " %";
	}
}