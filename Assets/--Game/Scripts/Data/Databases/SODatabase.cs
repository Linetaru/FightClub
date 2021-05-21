using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEditor;
using System.IO;

// Comme on peut pas générer des SO generic, y'a une dizaine de classe database qui hérite de cette clase
// Chaque enfant de SODatabase doit être son propre fichier sinon Unity ne reconnait pas le SO comme valide
public class SODatabase<T> : ScriptableObject, ISavable
{
    [SerializeField]
    bool autoPopulate = true;


    [SerializeField]
    [FolderPath]
    string pathDatabase;

    [SerializeField]
    [AssetList(AutoPopulate = true, CustomFilterMethod = "Filter")]
    List <T> database;
    public List<T> Database
    {
        get { return database; }
        set { database = value; }
    }


    List<bool> unlocked = new List<bool>();
    /*public List<bool> Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }*/


#if UNITY_EDITOR
    private bool Filter(T obj)
    {
        if (autoPopulate)
        {
            return true;
        }
        else
        {
            string[] files = Directory.GetFiles(pathDatabase, "*.asset", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var prefab = AssetDatabase.LoadAssetAtPath(file, typeof(T));
                if (obj.Equals(prefab))
                    return true;
            }
        }
        return false;
    }
#endif





    public bool GetUnlocked(int i)
    {
        if (unlocked.Count == 0)
        {
            CreateUnlockableList();
        }
        return unlocked[i];
    }
    public bool GetUnlocked(string id)
    {
        if (unlocked.Count == 0)
        {
            CreateUnlockableList();
        }
        for (int i = 0; i < database.Count; i++)
        {
            if (id.Equals(database[i].ToString()))
                return unlocked[i];
        }
        return false;
    }

    public void SetUnlocked(int i, bool b)
    {
        if (unlocked.Count == 0)
        {
            CreateUnlockableList();
        }
        unlocked[i] = b;
    }

    public void SetUnlocked(T item, bool b)
    {
        if (unlocked.Count == 0)
        {
            CreateUnlockableList();
        }

        for (int i = 0; i < database.Count; i++)
        {
            Debug.Log(database[i].Equals(item));
            if(database[i].Equals(item))
            {
                unlocked[i] = b;
                Debug.Log(unlocked[i]);
                return;
            }
        }
    }







    public string GetSaveID()
    {
        return this.name;
    }

    public List<string> GetAllSavesID()
    {
        List<string> ids = new List<string>(database.Count);
        for (int i = 0; i < database.Count; i++)
        {
            ids.Add(database[i].ToString());
        }
        return ids;
    }

    public List<SaveGameVariable> Save()
    {
        if (unlocked.Count == 0)
        {
            CreateUnlockableList();
        }

        List<SaveGameVariable> save = new List<SaveGameVariable>(unlocked.Count);
        for (int i = 0; i < unlocked.Count; i++)
        {
            if(unlocked[i] == true)
            {
                save.Add(new SaveGameVariable(database[i].ToString(), 1));
            }
        }

        return save;
    }

    public void Load(List<SaveGameVariable> gameVariables)
    {
        unlocked.Clear();
        if (unlocked.Count == 0)
        {
            CreateUnlockableList();
        }

        for (int i = 0; i < gameVariables.Count; i++)
        {
            for (int j = 0; j < database.Count; j++)
            {
                if (database[j].ToString().Equals(gameVariables[i].variableName))
                {
                    if (gameVariables[i].variableValue >= 1)
                    {
                        Debug.Log(gameVariables[i].variableValue);
                        unlocked[j] = true;
                    }
                    break;
                }
            }
        }
    }


    private void CreateUnlockableList()
    {
        unlocked = new List<bool>(database.Count);
        for (int i = 0; i < database.Count; i++)
        {
            unlocked.Add(false);
        }
    }


}

