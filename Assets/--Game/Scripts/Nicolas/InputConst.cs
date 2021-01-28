using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputConst : ScriptableObject
{
	string Movement = Rewired.ReInput.mapping.GetAction(0).name;
	string Jump = Rewired.ReInput.mapping.GetAction(1).name;
	string Attack = Rewired.ReInput.mapping.GetAction(2).name;
	string Action3 = Rewired.ReInput.mapping.GetAction(3).name;
	string Action4 = Rewired.ReInput.mapping.GetAction(4).name;
	string Interact = Rewired.ReInput.mapping.GetAction(5).name;
	string Return = Rewired.ReInput.mapping.GetAction(6).name;
	string Pause = Rewired.ReInput.mapping.GetAction(7).name;
}