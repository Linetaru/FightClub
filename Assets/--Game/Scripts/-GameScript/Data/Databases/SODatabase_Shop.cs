using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;

// N"hérite pas de SO database pour ne pas implémenter ISavable

[CreateAssetMenu(fileName = "Database_Shop", menuName = "Database/DatabaseShop", order = 1)]
public class SODatabase_Shop : ScriptableObject
{
    [SerializeField]
    bool autoPopulate = true;

    [SerializeField]
    [FolderPath]
    string pathDatabase;

    [SerializeField]
    [AssetList(AutoPopulate = true, CustomFilterMethod = "Filter")]
    List<ShopItemData> database;
    public List<ShopItemData> Database
    {
        get { return database; }
        set { database = value; }
    }


#if UNITY_EDITOR
    private bool Filter(ShopItemData obj)
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
                var prefab = AssetDatabase.LoadAssetAtPath(file, typeof(ShopItemData));
                if (obj.Equals(prefab))
                    return true;
            }
        }
        return false;
    }
#endif
}
