using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SaveCategory
{
	[SerializeField]
	private string categoryID;
	public string CategoryID
	{
		get { return categoryID; }
		set { categoryID = value; }
	}

	[SerializeField]
	private List<SaveGameVariable> gameVariables = new List<SaveGameVariable>();
	public List<SaveGameVariable> GameVariables
	{
		get { return gameVariables; }
		set { gameVariables = value; }
	}


	public SaveCategory(string s, List<SaveGameVariable> variables)
	{
		categoryID = s;
		gameVariables = variables;
	}

	public void Save(string s, List<SaveGameVariable> variables)
	{
		categoryID = s;
		gameVariables.Clear();
		gameVariables = variables;
	}
}



[CreateAssetMenu(fileName = "SaveData_", menuName = "Data/SaveData", order = 1)]
public class SaveData : ScriptableObject
{

	[SerializeField]
	private List<SaveCategory> dataSaves = new List<SaveCategory>();
	public List<SaveCategory> DataSaves
	{
		get { return dataSaves; }
		set { dataSaves = value; }
	}


	// On copie save profile dans dataSaves
	public void LoadProfile(SaveProfile saveProfile)
	{
		dataSaves.Clear();
		for (int i = 0; i < saveProfile.SavesProfile.Count; i++)
		{
			List<SaveGameVariable> list = new List<SaveGameVariable>(saveProfile.SavesProfile[i].GameVariables.Count);
			for (int j = 0; j < saveProfile.SavesProfile[i].GameVariables.Count; j++)
			{
				list.Add(new SaveGameVariable(saveProfile.SavesProfile[i].GameVariables[j].variableName, saveProfile.SavesProfile[i].GameVariables[j].variableValue));
			}
			dataSaves.Add(new SaveCategory(saveProfile.SavesProfile[i].CategoryID, list));
		}
	}




	// Utilisé pour sauver une modif sans avoir à faire des dingueries dans tout les sens
	public void Save(ISavable savablesObjects)
	{
		for (int i = 0; i < dataSaves.Count; i++)
		{
			if(dataSaves[i].CategoryID == savablesObjects.GetSaveID())
			{
				dataSaves[i].Save(savablesObjects.GetSaveID(), savablesObjects.Save());
			}
		}
	}

	// Utilisé pour tout sauvegarder sans ambiguité
	public void Save(List<ISavable> savablesObjects)
	{
		dataSaves.Clear();

		for (int i = 0; i < savablesObjects.Count; i++)
		{
			dataSaves.Add(new SaveCategory(savablesObjects[i].GetSaveID(), savablesObjects[i].Save()));
		}
	}

	public void Load(List<ISavable> loadablesObjects)
	{
		for (int i = dataSaves.Count-1; i >= 0; i--)
		{
			bool exist = false;
			for (int j = 0; j < loadablesObjects.Count; j++)
			{
				if(dataSaves[i].CategoryID == loadablesObjects[j].GetSaveID())
				{
					loadablesObjects[j].Load(dataSaves[i].GameVariables);
					loadablesObjects.RemoveAt(j);
					exist = true;
					break;
				}
			}

			if(exist == false) // Ce qu'on a sauvegardé n'existe plus dans la nouvelle version de Break Punch
			{
				dataSaves.RemoveAt(i);
			}
		}
	}


}

