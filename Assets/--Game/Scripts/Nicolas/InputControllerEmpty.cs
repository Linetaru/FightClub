using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InputControllerEmpty : SerializedMonoBehaviour
{
	[SerializeField]
	public IControllable controllable;

	Input_Info input;

	private void Awake()
	{
		input = new Input_Info();
	}

	private void Update()
	{
		if(controllable != null)
			controllable.UpdateControl(0, input);
	}

}
