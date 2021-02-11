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
	public static InputAction Pause = Rewired.ReInput.mapping.GetAction(7);
}