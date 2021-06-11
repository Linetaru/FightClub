using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class SaveGameVariable 
{
	[HorizontalGroup(Width = 0.8f)]
	[HideLabel]
	[SerializeField]
	public string variableName;

	[HorizontalGroup]
	[HideLabel]
	[SerializeField]
	public int variableValue;

	public SaveGameVariable(string id, int value)
	{
		variableName = id;
		variableValue = value;
	}

}
