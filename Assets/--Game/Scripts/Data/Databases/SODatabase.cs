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
public class SODatabase<T> : ScriptableObject, IListSavable
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

}

