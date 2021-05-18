using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface ISavable
{

}


[CreateAssetMenu(fileName = "SaveData_", menuName = "Data/SaveData", order = 1)]
public class SaveData : SerializedScriptableObject, ISavable
{

	[SerializeField]
	private int money;
	public int Money
	{
		get { return money; }
		set { money = value; }
	}

	[SerializeField]
	ISavable savable;

}


[CreateAssetMenu(fileName = "SaveData_", menuName = "Data/SaveDatabase", order = 1)]
public class SaveDatabase : SaveData
{

}

