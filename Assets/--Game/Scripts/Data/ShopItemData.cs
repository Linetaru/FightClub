using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ShopItemData_", menuName = "Data/ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get { return itemName; }
    }

    [SerializeField]
    int itemPrice;
    public int ItemPrice
    {
        get { return itemPrice; }
    }

    [SerializeField]
    private string itemDescription;
    public string ItemDescription
    {
        get { return itemDescription; }
    }
}


/*[CreateAssetMenu(fileName = "Database", menuName = "Data/Database", order = 1)]
public class Database : ScriptableObject
{
    [FolderPath]
    string path;

    [SerializeField]
    [AssetList(AutoPopulate = true, Path = "path")]
    List<ScriptableObject> scriptableObjects;
}*/


/*[CreateAssetMenu(fileName = "Database", menuName = "Data/Database", order = 1)]
public class Database : ScriptableObject
{
    [SerializeField]
    Database<T> database;
}*/



