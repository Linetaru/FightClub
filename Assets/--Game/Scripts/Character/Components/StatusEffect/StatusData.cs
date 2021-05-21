using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "StatusData_", menuName = "Data/StatusData", order = 1)]
public class StatusData : SerializedScriptableObject
{
	[HorizontalGroup]
	[SerializeField]
	[HideLabel]
	[ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
	StatusUpdate[] statusUpdates = new StatusUpdate[0];
	public StatusUpdate[] StatusUpdates
	{
		get { return statusUpdates; }
	}


	[HorizontalGroup]
	[SerializeField]
	[HideLabel]
	[ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
	StatusEffect[] statusEffects = new StatusEffect[0];
	public StatusEffect[] StatusEffects
	{
		get { return statusEffects; }
	}
}
