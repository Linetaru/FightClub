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

    [Title("SpriteCharacterFace")]
    [HorizontalGroup]
    [HideLabel]
    [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 64)]
    public Sprite characterFace;

    [Title("SpriteLifeStocks")]
    [HorizontalGroup]
    [HideLabel]
    [PreviewField(Alignment = ObjectFieldAlignment.Left, Height = 64)]
    public Sprite characterLifeStocks;

    [Title("Colors")]
    public List<Material> characterMaterials;

    // utilisé pour les sauvegardes, je laisse la liste characterMaterials parce que c'est sans doute plus ergo pour le moment, mais si on doit changer d'autres
    // paramètres que le Material quand on change de couleur, on migrera sur les Database color
    public SODatabase_Color characterColors; 

    /*public List<Material> characterMaterials
    {
        get { return myVar; }
        set { myVar = value; }
    }*/


    [Title("Intro")]
    public IntroductionCharacter[] introductions;

    [Title("Victory")]
    public VictoryScreen victoryScreen;

    // Idealement avoir une reference au model du perso tout court et ne pas faire la distinction entre le model du perso et le model de defaite
    public CharacterModel looserModel;

    [Title("AI")]
    public AIBehavior[] aiBehavior;
}