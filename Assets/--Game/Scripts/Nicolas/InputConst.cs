using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public static class InputConst
{
	public static InputAction Horizontal = Rewired.ReInput.mapping.GetAction(0);
	public static InputAction Jump = Rewired.ReInput.mapping.GetAction(1);
	public static InputAction Attack = Rewired.ReInput.mapping.GetAction(2);
	public static InputAction Vertical = Rewired.ReInput.mapping.GetAction(3);
	public static InputAction Smash = Rewired.ReInput.mapping.GetAction(4);
	public static InputAction Interact = Rewired.ReInput.mapping.GetAction(5);
	public static InputAction Return = Rewired.ReInput.mapping.GetAction(6);
	public static InputAction Start = Rewired.ReInput.mapping.GetAction(7);
	public static InputAction LeftShoulder = Rewired.ReInput.mapping.GetAction(8);
	public static InputAction RightShoulder = Rewired.ReInput.mapping.GetAction(9);
	public static InputAction Dodge = Rewired.ReInput.mapping.GetAction(10);
	public static InputAction Grab = Rewired.ReInput.mapping.GetAction(11);
	public static InputAction SignatureMove = Rewired.ReInput.mapping.GetAction(12);
	public static InputAction UpTaunt = Rewired.ReInput.mapping.GetAction(13);
	public static InputAction LeftTaunt = Rewired.ReInput.mapping.GetAction(14);
	public static InputAction DownTaunt = Rewired.ReInput.mapping.GetAction(15);
	public static InputAction RightTaunt = Rewired.ReInput.mapping.GetAction(16);
	public static InputAction LeftTrigger = Rewired.ReInput.mapping.GetAction(19);
	public static InputAction RightTrigger = Rewired.ReInput.mapping.GetAction(20);
}