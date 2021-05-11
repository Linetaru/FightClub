using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputRecord_", menuName = "Data/Training/InputRecording", order = 1)]
public class InputRecordingData : ScriptableObject
{
	[SerializeField]
	private List<DebugInput> inputsRecorded;

	public List<DebugInput> InputsRecorded
	{
		get { return inputsRecorded; }
		set { inputsRecorded = value; }
	}
}
