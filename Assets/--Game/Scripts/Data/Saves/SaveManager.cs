using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public class SaveManager : MonoBehaviour
{

    private static SaveManager instance;
    public static SaveManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Load la save la première fois que le Save Manager est appelé (donc idéalement dès qu'on lance une session du jeu)
            if(LoadFile() == false)
            {
                // Il n'y a pas de save donc on doit load une save par défaut
                Debug.Log("Load failed but first time so loading default save");
                saveData.LoadProfile(saveNewGameProfile);
                LoadData();
            }
            else // Sinon le load c'est bien passé
            {
                Debug.Log("Load Successful");
            }

        }
        else
        {
            Destroy(this);
        }
    }



    [SerializeField]
    string saveFileName = "save";

    [SerializeField]
    SaveData saveData;

    [SerializeField]
    SaveProfile saveNewGameProfile;

    [SerializeField]
    [AssetList(AutoPopulate = true, CustomFilterMethod = "isSavable")]
    ScriptableObject[] dataToSave;

    private bool isSavable(ScriptableObject obj)
    {
        return (obj is ISavable);
    }



    // Utilisé pour sauver une modif sans avoir à reappeler la fonction save de tout les dataToSave
    private void SaveData(ISavable saveItem)
    {
        saveData.Save(saveItem);
    }

    private void SaveAllData()
    {
        List<ISavable> savables = new List<ISavable>(dataToSave.Length);
        for (int i = 0; i < dataToSave.Length; i++)
        {
            savables.Add(dataToSave[i] as ISavable);
        }
        saveData.Save(savables);
    }

    private void LoadData()
    {
        List<ISavable> savables = new List<ISavable>(dataToSave.Length);
        for (int i = 0; i < dataToSave.Length; i++)
        {
            savables.Add(dataToSave[i] as ISavable);
        }
        saveData.Load(savables);
    }













    public void SaveFile(ISavable saveItem = null)
    {
        if(saveItem == null)
        {
            SaveAllData();
        }
        else
        {
            SaveData(saveItem);
        }


        // Save dans un fichier Json saveData
        string json = JsonUtility.ToJson(saveData);
        string filePath = string.Format("{0}/saves/{1}.json", Application.persistentDataPath, saveFileName);
        Debug.Log(filePath);
        FileInfo fileInfo = new FileInfo(filePath);

        if (!fileInfo.Directory.Exists)
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
        }
        File.WriteAllText(filePath, json);
    }






    public bool LoadFile()
    {
        string filePath = string.Format("{0}/saves/{1}.json", Application.persistentDataPath, saveFileName);
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(dataAsJson, saveData);
            LoadData();
            return true;
        }
        return false;
    }

}
