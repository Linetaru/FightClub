using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ShopItemData_", menuName = "Data/ShopItemData", order = 1)]
public class ShopItemData : ScriptableObject
{
    [HorizontalGroup("ShopItem", Width = 64)]
    [SerializeField]
    [HideLabel]
    [PreviewField(Alignment = ObjectFieldAlignment.Left)]
    private Sprite itemThumbnail;
    public Sprite ItemThumbnail
    {
        get { return itemThumbnail; }
    }


    [HorizontalGroup("ShopItem")]
    [VerticalGroup("ShopItem/Right")]
    [SerializeField]
    private string itemName;
    public string ItemName
    {
        get { return itemName; }
    }

    [VerticalGroup("ShopItem/Right")]
    [SerializeField]
    int itemPrice;
    public int ItemPrice
    {
        get { return itemPrice; }
    }


    [SerializeField]
    [TextArea]
    private string itemDescription;
    public string ItemDescription
    {
        get { return itemDescription; }
    }


    [SerializeField]
    private ScriptableObject itemToUnlock;
    public ScriptableObject ItemToUnlock
    {
        get { return itemToUnlock; }
    }

    [SerializeField]
    private ScriptableObject databaseToLook;
    public ScriptableObject DatabaseToLook
    {
        get { return databaseToLook; }
    }

    public string GetUnlockID()
    {
        if (ItemToUnlock != null)
            return itemToUnlock.name;
        return "";
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



