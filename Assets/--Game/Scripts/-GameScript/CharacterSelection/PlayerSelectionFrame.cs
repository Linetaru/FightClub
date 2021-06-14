using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PlayerSelectionFrame : MonoBehaviour
{
    //[SerializeField]
    //CharacterSelectManager characterSelectManager;

    //-------------------- Cursor -----------------
    [Header("Cursor")]
    [SerializeField]
    GameObject playerCursor;

    public int currentCursorPosition = 0;

    //-------------------- Hologram Models -----------------
    //[Header("Hologram State")]

    [Header("Display")]
    [SerializeField]
    GameObject spotLights;

    public GameObject modelParent;

    CharacterModel charModel;

    [SerializeField]
    Material hologramMaterial;

    [Header("Display Decal")]

    [SerializeField]
    UnityEngine.Rendering.HighDefinition.DecalProjector playerDecal;

    [SerializeField]
    Material notConnectedDecalMaterial;

    [SerializeField]
    Material connectedDecalMaterial;

    [SerializeField]
    Material CPUDecalMaterial;

    [Header("Team Colors")]
    [SerializeField]
    MeshRenderer playerPodium;

    [SerializeField]
    Material[] teamColorMaterials;



    [Space]

    public int iD = 0;


    [HideInInspector]
    public bool isPlayerConnected = false;

    [HideInInspector]
    public bool isCharacterChoosed = false;

    [HideInInspector]
    public bool paramsChoosed = false;


    [Header("CharacterParams")]
    [SerializeField]
    GameObject characterParams;

    public Image choosableSkillIcon;

    public GameObject paramCursor;

    [SerializeField]
    TextMeshProUGUI colorChoiceText;

    [SerializeField]
    TextMeshProUGUI skillChoiceText;

    [SerializeField]
    TextMeshProUGUI configChoiceText;

    [HideInInspector]
    public int currentConfigChoice = 0;

    [HideInInspector]
    public int currentColorSkin = 0;

    [Title("Team Infos UI")]
    private Dictionary<int, string> teamColors = new Dictionary<int, string>() {
        {0, "#C8C8C8"},
        {1, "#800000"},
        {2, "#000E80"},
        {3, "#7E8000"},
        {4, "#00801A"}
    };
    // Teams
    [SerializeField]
    private TextMeshProUGUI teamText;
    [SerializeField]
    private Image teamBackground;
    [HideInInspector]
    public TeamEnum currentTeam = 0;
    private int teamEnumLength;

    public CharacterData currentChoosedCharacter = null;

    [HideInInspector]
    public int currentChoosableSkill = 0;

    [HideInInspector]
    public int currentParam = 0;

    public List<Transform> paramPositions;

    string[] choosableSkills = new string[6];
    Color[] choosableSkillsColor = new Color[6];
    List<string> choosableConfig = new List<string>();

    [Space]
    [SerializeField]
    public CharacterSelectionCell[] characterCells = new CharacterSelectionCell[5];

    Player player;

    //bool isPlayerConnected = false;
    public bool isPlayerReady = false;

    [HideInInspector]
    public bool joystickPushed = false;

    [HideInInspector]
    public bool isCPU = false;

    [HideInInspector]
    public int playerInControl;




    private void Awake()
    {
        //choosableSkills[0] = "Homing Dash";
        //choosableSkills[1] = "Burst";
        //choosableSkills[2] = "Attack Up";
        //choosableSkills[3] = "Defense Up";
        //choosableSkills[4] = "Infinite Jump";
        //choosableSkills[5] = "Earthquake";

        //choosableSkillsColor[0] = Color.green;
        //choosableSkillsColor[1] = Color.cyan;
        //choosableSkillsColor[2] = Color.red;
        //choosableSkillsColor[3] = Color.blue;
        //choosableSkillsColor[4] = Color.white;
        //choosableSkillsColor[5] = Color.yellow;

        teamEnumLength = System.Enum.GetValues(typeof(TeamEnum)).Length - 1;

        choosableConfig.Add("Classic");
        for (int i = 0; i < InputMappingDataStatic.inputMappingDataClassics.Count; i++)
        {
            choosableConfig.Add(InputMappingDataStatic.inputMappingDataClassics[i].profileName);
        }

        UpdateTeamColor();
    }

    //public void UpdateDisplay()
    //{
    //    if (isPlayerConnected)
    //    {
    //        spotLights.SetActive(true);
    //        playerCursor.SetActive(true);
    //        //currentCursorPosition = 0;
    //    }
    //    else
    //    {
    //        spotLights.SetActive(false);
    //        playerCursor.SetActive(false);
    //        //UpdateCharacterModel(null);
    //    }

    //    if (isPlayerConnected && isCharacterChoosed)
    //    {
    //        spotLights.SetActive(true);
    //        playerCursor.SetActive(false);
    //    }

    //    if (isCharacterChoosed && !paramsChoosed)
    //    {
    //        characterParams.SetActive(true);
    //        colorChoiceText.text = "Color " + currentChoosableSkill + 1;
    //        skillChoiceText.text = choosableSkills[currentChoosableSkill];
    //        choosableSkillIcon.color = choosableSkillsColor[currentChoosableSkill];

    //    }
    //}

    public void Connected(List<CharacterData> characterDatas)
    {
        if (isCPU)
            playerDecal.material = CPUDecalMaterial;
        else
            playerDecal.material = connectedDecalMaterial;

        isPlayerConnected = true;
        spotLights.SetActive(true);
        playerCursor.SetActive(true);
        currentCursorPosition = 0;

        CreateCharacterModel(characterDatas);
    }

    public void Disconnected()
    {
        playerDecal.material = notConnectedDecalMaterial;

        isCPU = false;
        isPlayerConnected = false;
        isCharacterChoosed = false;
        isPlayerReady = false;
        paramsChoosed = false;
        spotLights.SetActive(false);
        playerCursor.SetActive(false);

        //foreach (GameObject child in characterModel.transform)
        //{
        //    if(child != null)
        //    Destroy(child);
        //}

        if (charModel != null)
            Destroy(charModel.gameObject);
    }

    public void CreateCharacterModel(List<CharacterData> characterDatas)
    {
        //if (characterModel.GetComponentInChildren<GameObject>() != CharacterSelectManager._instance.characterDatas[currentCursorPosition].characterSelectionModel)
        //{
        if (charModel != null)
            Destroy(charModel.gameObject);

        if (characterDatas[currentCursorPosition] != null)
        {
            //Debug.LogError(characterDatas[currentCursorPosition]);
            charModel = Instantiate(characterDatas[currentCursorPosition].looserModel, modelParent.transform);
            //UpdateCharacterColor(CharacterSelectManager._instance.characterDatas[currentCursorPosition].characterMaterials);
            charModel.SetColor(0, hologramMaterial);
        }
        //}
        //else
        //{
        //    this.characterModel = null;
        //}
    }

    public void CreateCharacterModelRandom(CharacterData characterDatas)
    {
        if (charModel != null)
            Destroy(charModel.gameObject);

        if (characterDatas != null)
        {
            charModel = Instantiate(characterDatas.looserModel, modelParent.transform);
            //UpdateCharacterColor(CharacterSelectManager._instance.characterDatas[currentCursorPosition].characterMaterials);
            charModel.SetColor(0, hologramMaterial);
        }
    }

    public void ChangeCharacterModelColor(List<CharacterData> characterDatas)
    {
        //characterDatas[0].characterColors.GetUnlocked()
        //characterDatas[0].characterColors.Database[0].characterFace
        //characterDatas[0].characterColors.Database[0].material
        charModel.SetColor(0, characterDatas[currentCursorPosition].characterMaterials[currentColorSkin]);
    }

    public void ChangeCharacterModelColorRandom(CharacterData characterDatas)
    {
        if (charModel != null)
            charModel.SetColor(0, characterDatas.characterMaterials[currentColorSkin]);
    }

    //public void UpdateCharacterColor(List<Material> characterMaterials)
    //{
    //    if (!isCharacterChoosed)
    //    {
    //        foreach (SkinnedMeshRenderer mesh in characterModel.transform)
    //        {
    //            mesh.material = HologramMaterial;
    //        }
    //    }
    //    else
    //    {
    //        foreach (SkinnedMeshRenderer mesh in characterModel.transform)
    //        {
    //            mesh.material = characterMaterials[currentColorSkin];
    //        }
    //    }
    //}

    public void UpdateCursorPosition(bool goToRight, List<CharacterData> characterDatas)
    {
        if (goToRight)
        {
            if (currentCursorPosition < characterCells.Length - 1)
            {
                currentCursorPosition++;
            }
        }
        else
        {
            if (currentCursorPosition > 0)
            {
                currentCursorPosition--;
            }
        }
        playerCursor.transform.DOMove(characterCells[currentCursorPosition].transform.position, .2f);
        playerCursor.transform.DORotateQuaternion(characterCells[currentCursorPosition].transform.rotation, .2f);
        CreateCharacterModel(characterDatas);
    }

    public void ChooseCharacter(List<CharacterData> characterDatas)
    {
        currentChoosedCharacter = characterDatas[currentCursorPosition];
        isPlayerConnected = true;
        isCharacterChoosed = true;

        playerCursor.SetActive(false);
        characterParams.SetActive(true);

        UpdateParamsDisplay();
        ChangeCharacterModelColor(characterDatas);
    }

    public void UnchooseCharacter()
    {
        currentChoosedCharacter = null;
        isCharacterChoosed = false;

        playerCursor.SetActive(true);
        characterParams.SetActive(false);
    }

    public void UpdateParamsDisplay()
    {
        colorChoiceText.text = "Color " + (currentColorSkin + 1).ToString();
        //skillChoiceText.text = choosableSkills[currentChoosableSkill];
        //choosableSkillIcon.color = choosableSkillsColor[currentChoosableSkill];
        configChoiceText.text = choosableConfig[currentConfigChoice];
    }

    public void ChangeParam()
    {
        if (currentParam == 0)
        {
            currentParam = 2;

        }
        //else if (currentParam == 1)
        //    currentParam = 2;
        else if (currentParam == 2)
            currentParam = 0;

        paramCursor.transform.DOMove(paramPositions[currentParam].transform.position, .2f);
        paramCursor.transform.DORotate(paramPositions[currentParam].transform.rotation.eulerAngles, .2f);
        //paramCursor.transform.DORotate(transform.rotation.eulerAngles, .2f);
        UpdateParamsDisplay();
    }

    public void UpdateParam(bool goToRight, List<CharacterData> characterDatas)
    {

        //0 c'est les couleurs et 1 c'est les skills

        if (goToRight)
        {
            if (currentParam == 0)
            {
                if (currentColorSkin < currentChoosedCharacter.characterMaterials.Count - 1)
                {
                    ++currentColorSkin;
                }
                else
                {
                    currentColorSkin = 0;
                }
            }
            //else if (currentParam == 1)
            //{
            //    if (currentChoosableSkill < choosableSkills.Length - 1)
            //    {
            //        ++currentChoosableSkill;
            //    }
            //    else
            //    {
            //        currentChoosableSkill = 0;
            //    }
            //}
            else if (currentParam == 2)
            {
                if (currentConfigChoice < choosableConfig.Count - 1)
                {
                    ++currentConfigChoice;
                }
                else
                {
                    currentConfigChoice = 0;
                }
            }
        }
        else
        {
            if (currentParam == 0)
            {
                if (currentColorSkin > 0)
                {
                    --currentColorSkin;
                }
                else
                {
                    currentColorSkin = currentChoosedCharacter.characterMaterials.Count - 1;
                }
            }
            //else if (currentParam == 1)
            //{
            //    if (currentChoosableSkill > 0)
            //    {
            //        --currentChoosableSkill;
            //    }
            //    else
            //    {
            //        currentChoosableSkill = choosableSkills.Length - 1;
            //    }
            //}
            else if (currentParam == 2)
            {
                if (currentConfigChoice > 0)
                {
                    --currentConfigChoice;
                }
                else
                {
                    currentConfigChoice = choosableConfig.Count - 1;
                }
            }
        }

        ChangeCharacterModelColor(characterDatas);
        UpdateParamsDisplay();

    }

    public void Ready()
    {
        characterParams.SetActive(false);
        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = true;
        isPlayerReady = true;
    }

    public void NotReady()
    {
        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = false;
        isPlayerReady = false;
        characterParams.SetActive(true);
    }

    public void RandomReady(List<CharacterData> characterDatas)
    {
        if (isCPU)
            playerDecal.material = CPUDecalMaterial;
        else
            playerDecal.material = connectedDecalMaterial;

        characterParams.SetActive(false);
        playerCursor.SetActive(false);
        //int random = Random.Range(0, characterDatas.Count);
        CharacterData characterDatasRandom = null;

        List<CharacterData> characterDatasForRandom = new List<CharacterData>();

        foreach (CharacterData characterData in characterDatas)
        {
            if (characterData == null || characterData.characterName == "Random")
            {
                //Debug.LogError(characterData + " is NOT in random list");
            }
            else
            {
                //Debug.LogError(characterData + " IS in random list");
                characterDatasForRandom.Add(characterData);
            }
        }
        characterDatasRandom = characterDatasForRandom[Random.Range(0, characterDatasForRandom.Count)];
        CreateCharacterModelRandom(characterDatasRandom);
        //if (characterDatas[random] == null || random == 2)
        //switch (random)
        //{
        //    case 0:
        //        if (characterDatas[0] == null)
        //        {
        //            RandomReady(characterDatas);
        //            return;
        //        }
        //        characterDatasRandom = characterDatas[0];
        //        CreateCharacterModelRandom(characterDatasRandom);
        //        break;
        //    case 1:
        //        if (characterDatas[1] == null)
        //            return;
        //        characterDatasRandom = characterDatas[1];
        //        CreateCharacterModelRandom(characterDatasRandom);
        //        break;
        //    case 2:
        //        if (characterDatas[3] == null)
        //            return;
        //        characterDatasRandom = characterDatas[3];
        //        CreateCharacterModelRandom(characterDatasRandom);
        //        break;
        //}
        currentChoosedCharacter = characterDatasRandom;
        Debug.LogError(currentChoosedCharacter);

        currentColorSkin = Random.Range(0, characterDatasRandom.characterMaterials.Count);
        //currentChoosableSkill = Random.Range(0, choosableSkills.Length);
        currentConfigChoice = 0;
        ChangeCharacterModelColorRandom(characterDatasRandom);

        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = true;
        isPlayerReady = true;
    }

    public void RandomReadyCPU(List<CharacterData> characterDatas)
    {

        if (isCPU)
            playerDecal.material = CPUDecalMaterial;
        else
            playerDecal.material = connectedDecalMaterial;

        isCPU = true;
        characterParams.SetActive(false);
        playerCursor.SetActive(false);
        //int random = Random.Range(0, characterDatas.Count);
        CharacterData characterDatasRandom = null;

        List<CharacterData> characterDatasForRandom = new List<CharacterData>();

        foreach (CharacterData characterData in characterDatas)
        {
            if (characterData == null || characterData.characterName == "Random")
            {

            }
            else
            {
                characterDatasForRandom.Add(characterData);
            }
        }
        characterDatasRandom = characterDatasForRandom[Random.Range(0, characterDatasForRandom.Count)];
        CreateCharacterModelRandom(characterDatasRandom);

        currentChoosedCharacter = characterDatasRandom;
        Debug.LogError(currentChoosedCharacter);

        currentColorSkin = Random.Range(0, characterDatasRandom.characterMaterials.Count);

        currentConfigChoice = 0;
        ChangeCharacterModelColorRandom(characterDatasRandom);

        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = true;
        isPlayerReady = true;
    }

    public void CycleTeam(bool isVolleyBallMode)
    {
        teamText.SetText(currentTeam.ToString());
        if (isVolleyBallMode)
        {
            if ((int)currentTeam == 1)
                currentTeam = TeamEnum.Second_Team;
            else
                currentTeam = TeamEnum.First_Team;
        }
        else
        {
            if ((int)currentTeam < teamEnumLength)
                currentTeam++;
            else
                currentTeam = 0;
        }

        teamText.SetText(currentTeam.ToString());

        // UI Team Color
        UpdateTeamColor();
    }

    private void UpdateTeamColor()
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(teamColors[(int)currentTeam], out color);

        //teamBackground.color = color;
        //teamBackground.color = new Color(color.r, color.g, color.b, 0.6f);
        //standPodiumMaterial.color = color;
        playerPodium.material = teamColorMaterials[(int)currentTeam];
    }

    private void Start()
    {
        isPlayerReady = false;
        isPlayerConnected = false;
        isCharacterChoosed = false;
        paramsChoosed = false;
        spotLights.SetActive(false);
        playerCursor.SetActive(false);
        characterParams.SetActive(false);
        //HideHolograms();
    }
}