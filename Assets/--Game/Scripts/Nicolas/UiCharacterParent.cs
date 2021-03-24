using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCharacterParent : MonoBehaviour
{
	public CharacterUI[] characterUi;

	private int raised = 0;
	
	public void CharacterInitUi(CharacterBase user)
    {
		characterUi[raised].InitPlayerPanel(user);
		raised++;
	}
}