using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "InputRecord_", menuName = "Data/Training/InputRecording", order = 1)]
public class InputRecordingData : ScriptableObject
{
	[InfoBox("0 = Jump\n" +
		"1 = Attack\n" +
		"2 = Special\n" +
		"4 = Parry\n" +
		"5 = Dash")]
	[SerializeField]
	private List<DebugInput> inputsRecorded;

	public List<DebugInput> InputsRecorded
	{
		get { return inputsRecorded; }
		set { inputsRecorded = value; }
	}
}
