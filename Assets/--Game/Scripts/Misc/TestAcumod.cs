using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAcumod : MonoBehaviour
{
	[SerializeField]
	BattleManager battleManager;
	[SerializeField]
	StatusData buffAttack;
	[SerializeField]
	StatusData buffDefense;

	bool eloise = false;
	bool guillaume = true;



	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (eloise == false)
			{
				for (int i = 0; i < battleManager.characterAlive.Count; i++)
				{
					battleManager.characterAlive[i].GetComponentInChildren<CharacterMoveset>().StatusData = buffAttack;
				}
			}
			else if (eloise == true)
			{
				for (int i = 0; i < battleManager.characterAlive.Count; i++)
				{
					battleManager.characterAlive[i].GetComponentInChildren<CharacterMoveset>().StatusData = buffDefense;
				}
			}
			eloise = !eloise;
		}
	}
}
