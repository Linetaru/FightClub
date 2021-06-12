using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAI : MonoBehaviour
{
	[SerializeField]
	int characterID = 1;

	[SerializeField]
	AIController aiController;
	[SerializeField]
	InputController inputController;


	public void AddCharacter(CharacterBase c)
	{
		if(c.PlayerID == characterID)
		{
			aiController.AIBehaviors[0].SetCharacter(c, inputController);
			//aiController.StartBehaviors();
		}
	}
}
