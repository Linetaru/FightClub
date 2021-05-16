using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


// Comme on peut pas générer des SO generic, y'a une dizaine de classe database qui hérite de cette clase
// Chaque enfant de SODatabase doit être son propre fichier sinon Unity ne reconnait pas le SO comme valide
public class SODatabase<T> : ScriptableObject
{
    [SerializeField]
    bool autoPopulate = true;
    [SerializeField]
    [AssetList(AutoPopulate = true)]
    List <T> database;

}

