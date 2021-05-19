using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface IListSavable
{

}


[CreateAssetMenu(fileName = "SaveData_", menuName = "Data/SaveData", order = 1)]
public class SaveData : SerializedScriptableObject
{

	[SerializeField]
	private int money;
	public int Money
	{
		get { return money; }
		set { money = value; }
	}

	[SerializeField]
	private List<SaveGameVariable> gameVariables = new List<SaveGameVariable>();
	public List<SaveGameVariable> GameVariables
	{
		get { return gameVariables; }
		set { gameVariables = value; }
	}






	// Copie les valeurs d'une save Data dans la saveData
	// A utilisé par le SaveData principal pour charger des profiles de sauvegarde
	public void LoadSaveData(SaveData saveData)
	{

	}


	public int GetVariable(string variableID)
	{
		return 0;
	}

	public void AddVariable(string variableName)
	{

	}

}

