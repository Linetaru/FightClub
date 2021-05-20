using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

// Utilisé pour sauvegarder un bool avec pour ID une entrée dans une database
[CreateAssetMenu(fileName = "SaveProfile_", menuName = "Data/SaveProfile", order = 1)]
public class SaveProfile : ScriptableObject
{

	[SerializeField]
	[AssetList(AutoPopulate = true, CustomFilterMethod = "isSavable")]
	ScriptableObject[] dataToSave;
	private bool isSavable(ScriptableObject obj)
	{
		return (obj is ISavable);
	}


	[SerializeField]
	private List<SaveCategory> savesProfile = new List<SaveCategory>();
	public List<SaveCategory> SavesProfile
	{
		get { return savesProfile; }
		set { savesProfile = value; }
	}

	[Button]
	private void GenerateID()
	{
		if (dataToSave == null)
			return;
		for (int i = 0; i < dataToSave.Length; i++)
		{
			ISavable save = dataToSave[i] as ISavable;
			bool exist = false;

			for (int j = 0; j < savesProfile.Count; j++)
			{
				// La catégorie existe déjà
				if(savesProfile[j].CategoryID.Equals(save.GetSaveID()))
				{
					GenerateVariable(j, save.GetAllSavesID());
					exist = true;
					break;
				}
			}

			// Si la catégorie n'existe pas on la créé
			if(exist == false)
			{
				savesProfile.Insert(i, new SaveCategory(save.GetSaveID(), GenerateCategory(save.GetAllSavesID())));
			}
		}
		UnityEditor.EditorUtility.SetDirty(this);
	}


	private void GenerateVariable(int category, List<string> allIDs)
	{
		if (savesProfile[category].GameVariables == null)
			return;

		// Ajoute les nouvelles entrées
		for (int j = 0; j < allIDs.Count; j++)
		{
			bool exist = false;
			for (int i = 0; i < savesProfile[category].GameVariables.Count; i++)
			{
				if (allIDs[j].Equals(savesProfile[category].GameVariables[i].variableName))
				{
					exist = true;
					break;
				}
			}
			// Si l'entrée n'existe pas, on la rajoute
			if (exist == false)
				savesProfile[category].GameVariables.Insert(j, new SaveGameVariable(allIDs[j], 0));
		}

		// Retire les entrées qui n'apparaissent plus dans la database allIDs
		for (int i = savesProfile[category].GameVariables.Count-1; i >= 0; i--)
		{
			if (!allIDs.Contains(savesProfile[category].GameVariables[i].variableName))
				savesProfile[category].GameVariables.RemoveAt(i);
		}
	}

	private List<SaveGameVariable> GenerateCategory(List<string> allIDs)
	{
		List<SaveGameVariable> res = new List<SaveGameVariable>(allIDs.Count);
		for (int j = 0; j < allIDs.Count; j++)
		{
			res.Add(new SaveGameVariable(allIDs[j], 0));
		}
		return res;
	}



}


