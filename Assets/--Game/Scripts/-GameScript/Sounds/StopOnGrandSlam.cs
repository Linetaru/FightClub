using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnGrandSlam : MonoBehaviour
{
	[SerializeField]
	GameData gameData;

	// Start is called before the first frame update
	void Start()
	{
		if (gameData.slamMode)
			Destroy(this.gameObject);
	}

}
