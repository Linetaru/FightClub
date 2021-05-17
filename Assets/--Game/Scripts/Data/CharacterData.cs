using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Menu;

[CreateAssetMenu(fileName = "CharacterData_Name", menuName = "Data/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 160)]
    public GameObject playerPrefab;

    public string characterName;
    public CharacterModel characterSelectionModel;

    [Title("UI")]
    public Sprite characterSelectionSprite;

    [Title("Colors")]
    public List<Material> characterMaterials;

    [Title("Intro")]
    public IntroductionCharacter[] introductions;

    [Title("Victory")]
    public VictoryScreen victoryScreen;

    // Idealement avoir une reference au model du perso tout court et ne pas faire la distinction entre le model du perso et le model de defaite
    public CharacterModel looserModel;
}