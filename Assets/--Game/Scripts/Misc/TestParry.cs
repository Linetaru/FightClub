using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParry : MonoBehaviour
{

	[SerializeField]
	BattleManager battleManager;


	bool eloise = false;
	bool guillaume = true;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			if (eloise == false)
			{
				for (int i = 0; i < battleManager.characterAlive.Count; i++)
				{
					battleManager.characterAlive[i].Knockback.Parry.ParryAngle = 200;
				}
			}
			else if (eloise == true)
			{
				for (int i = 0; i < battleManager.characterAlive.Count; i++)
				{
					battleManager.characterAlive[i].Knockback.Parry.ParryAngle = 400;
				}
			}
			eloise = !eloise;
		}
	}
}
