using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData_Name", menuName = "Data/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public GameObject playerPrefab;
}