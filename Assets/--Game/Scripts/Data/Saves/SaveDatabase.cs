using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

// Utilisé pour sauvegarder un bool avec pour ID une entrée dans une database

[CreateAssetMenu(fileName = "SaveDatabase_", menuName = "Data/SaveDatabase", order = 1)]
public class SaveDatabase : SerializedScriptableObject
{
	[SerializeField]
	IListSavable azer;



	[SerializeField]
	[ListDrawerSettings(Expanded = true, HideRemoveButton = true, HideAddButton = true)]
	private List<ListSaveDatabase> unlocked;

	public List<ListSaveDatabase> Unlocked
	{
		get { return unlocked; }
		set { unlocked = value; }
	}
}

[System.Serializable]
public class ListSaveDatabase
{
	[HorizontalGroup]
	[HideLabel]
	[ReadOnly]
	public string ItemID;

	[HorizontalGroup]
	[HideLabel]
	public bool unlocked = false;
}
