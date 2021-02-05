using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingControllable : MonoBehaviour, IControllable
{
	public int playerID;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void UpdateControl(int ID, Input_Info input_Info)
    {
		if(ID == playerID && input_Info.inputActions.Count != 0)
        {
			if(input_Info.inputActions[0].action == InputConst.Jump)
            {
				input_Info.inputActions[0].timeValue = 0;
				input_Info.inputActions[0].action = null;
				Debug.Log("Hi mom !");
            }
		}
    }
}