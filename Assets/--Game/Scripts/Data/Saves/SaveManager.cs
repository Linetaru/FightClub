using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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


    public void SaveFile(SaveData saveData)
    {
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
            return true;
        }
        return false;
    }

}
