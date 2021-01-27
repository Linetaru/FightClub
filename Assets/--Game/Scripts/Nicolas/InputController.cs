using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public enum State{
	Menu,
	UI_Pause,
	InGame,
}

public class InputController : MonoBehaviour
{
	List<Rewired.Player> players = new List<Player>();

	int[] playerID = new int[4];

	// Start is called before the first frame update
	void Start()
	{
		for (int i = 0; i < playerID.Length; i++)
		{
			playerID[i] = i;
			players.Add(ReInput.players.GetPlayer(i));
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(players[0].GetButtonDown("Jump"))
        {
			Debug.Log("Test");
        }
	}


}