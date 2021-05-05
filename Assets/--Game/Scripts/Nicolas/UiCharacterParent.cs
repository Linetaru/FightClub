using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCharacterParent : MonoBehaviour
{
	public CharacterHUD characterHudPrefab;
	public Transform parent;

	public TeamEnum teamEnums;
	public Color[] teamColors;

	/*public CharacterUI[] characterUi;

	private int raised = 0;*/

	/*public void CharacterInitUi(CharacterBase user)
    {
		characterUi[raised].InitPlayerPanel(user);
		raised++;
	}*/

	public void CharacterInitUi(CharacterBase user)
	{
		CharacterHUD hud = Instantiate(characterHudPrefab, parent);
		hud.InitPlayerPanel(user);
		hud.SetColor(teamColors[(int)user.TeamID]);
	}
}