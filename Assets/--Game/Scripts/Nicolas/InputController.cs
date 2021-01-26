using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour
{
	Rewired.Player players;

	// Start is called before the first frame update
	void Start()
	{
		players = ReInput.players.GetPlayer(playerID);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}