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

    [Space]
    [SerializeField]
    public CharacterSelectionCell[] characterCells = new CharacterSelectionCell[5];

    Player player;

    //bool isPlayerConnected = false;
    public bool isPlayerReady = false;

    [HideInInspector]
    public bool joystickPushed = false;


    private void Awake()
    {
        choosableSkills[0] = "Homing Dash";
        choosableSkills[1] = "Burst";
        choosableSkills[2] = "Attack Up";
        choosableSkills[3] = "Defense Up";
        choosableSkills[4] = "Infinite Jump";
        choosableSkills[5] = "Earthquake";

        choosableSkillsColor[0] = Color.green;
        choosableSkillsColor[1] = Color.cyan;
        choosableSkillsColor[2] = Color.red;
        choosableSkillsColor[3] = Color.blue;
        choosableSkillsColor[4] = Color.white;
        choosableSkillsColor[5] = Color.yellow;

        teamEnumLength = System.Enum.GetValues(typeof(TeamEnum)).Length - 1;

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

    public void Connected(CharacterData[] characterDatas)
    {
        isPlayerConnected = true;
        spotLights.SetActive(true);
        playerCursor.SetActive(true);
        currentCursorPosition = 0;

        CreateCharacterModel(characterDatas);
    }

    public void Disconnected()
    {
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

    public void CreateCharacterModel(CharacterData[] characterDatas)
    {
        //if (characterModel.GetComponentInChildren<GameObject>() != CharacterSelectManager._instance.characterDatas[currentCursorPosition].characterSelectionModel)
        //{
            if (charModel != null)
                Destroy(charModel.gameObject);

            if (characterDatas[currentCursorPosition] != null)
            {
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

    public void ChangeCharacterModelColor(CharacterData[] characterDatas)
    {
        charModel.SetColor(0, characterDatas[currentCursorPosition].characterMaterials[currentColorSkin]);
    }

    public void ChangeCharacterModelColorRandom(CharacterData characterDatas)
    {
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

    public void UpdateCursorPosition(bool goToRight, CharacterData[] characterDatas)
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

    public void ChooseCharacter(CharacterData[] characterDatas)
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
        skillChoiceText.text = choosableSkills[currentChoosableSkill];
        choosableSkillIcon.color = choosableSkillsColor[currentChoosableSkill];
    }

    public void ChangeParam()
    {
        if (currentParam == 0)
        {
            currentParam = 1;

        }
        else if (currentParam == 1)
            currentParam = 0;

        paramCursor.transform.DOMove(paramPositions[currentParam].transform.position, .2f);
        UpdateParamsDisplay();
    }

    public void UpdateParam(bool goToRight, CharacterData[] characterDatas)
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
            else if (currentParam == 1)
            {
                if (currentChoosableSkill < choosableSkills.Length - 1)
                {
                    ++currentChoosableSkill;
                }
                else
                {
                    currentChoosableSkill = 0;
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
            else if (currentParam == 1)
            {
                if (currentChoosableSkill > 0)
                {
                    --currentChoosableSkill;
                }
                else
                {
                    currentChoosableSkill = choosableSkills.Length - 1;
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

    public void RandomReady(CharacterData[] characterDatas)
    {
        characterParams.SetActive(false);
        playerCursor.SetActive(false);
        int random = Random.Range(0, 2);
        CharacterData characterDatasRandom = new CharacterData();
        switch (random)
        {
            case 0:
                characterDatasRandom = characterDatas[0];
                CreateCharacterModelRandom(characterDatasRandom);
                break;
            case 1:
                characterDatasRandom = characterDatas[1];
                CreateCharacterModelRandom(characterDatasRandom);
                break;
        }
        currentChoosedCharacter = characterDatasRandom;
        
        currentColorSkin = Random.Range(0, characterDatasRandom.characterMaterials.Count);
        currentChoosableSkill = Random.Range(0, choosableSkills.Length);
        ChangeCharacterModelColorRandom(characterDatasRandom);

        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = true;
        isPlayerReady = true;
    }

    public void CycleTeam()
    {
        if ((int)currentTeam < teamEnumLength)
            currentTeam++;
        else
            currentTeam = 0;

        teamText.SetText(currentTeam.ToString());

        // UI Team Color
        UpdateTeamColor();
    }

    private void UpdateTeamColor()
    {
        Color color = new Color();
        ColorUtility.TryParseHtmlString(teamColors[(int)currentTeam], out color);

        teamBackground.color = color;
        teamBackground.color = new Color(color.r, color.g, color.b, 0.6f);
    }


    //private void Awake()
    //{
    //    //rectTransform = GetComponent<RectTransform>();

    //    switch (playerInput)
    //    {
    //        case PlayerInput.One:
    //            iD = 0;
    //            break;

    //        case PlayerInput.Two:
    //            iD = 1;
    //            break;

    //        case PlayerInput.Three:
    //            iD = 2;
    //            break;

    //        case PlayerInput.Four:
    //            iD = 3;
    //            break;

    //        default:
    //            player = null;
    //            break;
    //    }
    //}

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



    //private void Update()
    //{
    //    if (player.GetButtonDown("LeftTaunt"))
    //    {
    //        if (characterSelectManager.playerStocks > 1)
    //        {
    //            --characterSelectManager.playerStocks;
    //            characterSelectManager.UpdateStockText();
    //        }
    //    }
    //    else if (player.GetButtonDown("RightTaunt"))
    //    {
    //        if (characterSelectManager.playerStocks < 999)
    //        {
    //            ++characterSelectManager.playerStocks;
    //            characterSelectManager.UpdateStockText();
    //        }
    //    }

    //    if (isPlayerConnected && player.GetButtonDown("RightShoulder"))
    //    {
    //        if (currentColorSkin < characterCells[currentCursorPosition].characterData.characterMaterials.Count - 1)
    //        {
    //            ++currentColorSkin;
    //        }
    //        else
    //        {
    //            currentColorSkin = 0;
    //        }
    //        colorChoiceText.text = "Color " + (currentColorSkin + 1);
    //        DisplayHologram();
    //    }
    //    else if (isPlayerConnected && player.GetButtonDown("LeftShoulder"))
    //    {
    //        if (currentColorSkin > 0)
    //        {
    //            --currentColorSkin;
    //        }
    //        else
    //        {
    //            currentColorSkin = characterCells[currentCursorPosition].characterData.characterMaterials.Count - 1;
    //        }
    //        colorChoiceText.text = "Color " + (currentColorSkin + 1);
    //        DisplayHologram();
    //    }

    //    if (!isPlayerReady)
    //    {
    //        if (!isPlayerConnected)
    //        {
    //            if (player.GetButtonDown("Interact"))
    //            {
    //                spotLights.SetActive(true);
    //                playerCursor.gameObject.SetActive(true);

    //                currentCursorPosition = 2;
    //                DisplayHologram();

    //                playerCursor.transform.position = characterCells[2].transform.position;
    //                playerCursor.transform.rotation = characterCells[2].transform.rotation;

    //                characterSelectManager.numberOfConnectedPlayers++;
    //                isPlayerConnected = true;
    //            }
    //            else if (player.GetButtonDown("Return"))
    //            {
    //                //Return to previous menu
    //                UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
    //            }
    //        }
    //        else
    //        {
    //            if (player.GetButtonDown("Interact"))
    //            {
    //                playerCursor.gameObject.SetActive(false);
    //                isPlayerReady = true;

    //                characterSelectManager.numberOfReadyPlayers++;
    //                characterSelectManager.DisplayReadyBands();

    //                DisplayHologram();
    //                if (hologramModels[currentCursorPosition].GetComponent<Animator>() != null)
    //                {
    //                    hologramModels[currentCursorPosition].GetComponent<Animator>().SetTrigger("SelectionReady");

    //                }
    //            }
    //            else if (player.GetButtonDown("Return"))
    //            {
    //                spotLights.SetActive(false);
    //                playerCursor.gameObject.SetActive(false);

    //                HideHolograms();

    //                characterSelectManager.numberOfConnectedPlayers--;
    //                isPlayerConnected = false;
    //            }
    //            else if (player.GetAxis("Horizontal") > .5f && !joystickPushed)
    //            {
    //                joystickPushed = true;
    //                if (currentCursorPosition < 4)
    //                {
    //                    ++currentCursorPosition;
    //                    DisplayHologram();

    //                    playerCursor.transform.DOMove(characterCells[currentCursorPosition].transform.position, .2f);
    //                    playerCursor.transform.DORotateQuaternion(characterCells[currentCursorPosition].transform.rotation, .2f);
    //                    //playerCursor.transform.position = characterCells[actualCursorPosition].transform.position;
    //                    //playerCursor.transform.rotation = characterCells[actualCursorPosition].transform.rotation;
    //                }
    //            }
    //            else if (player.GetAxis("Horizontal") < -.5f && !joystickPushed)
    //            {
    //                joystickPushed = true;
    //                if (currentCursorPosition > 0)
    //                {
    //                    --currentCursorPosition;
    //                    DisplayHologram();

    //                    playerCursor.transform.DOMove(characterCells[currentCursorPosition].transform.position, .2f);
    //                    playerCursor.transform.DORotateQuaternion(characterCells[currentCursorPosition].transform.rotation, .2f);
    //                    //playerCursor.transform.position = characterCells[actualCursorPosition].transform.position;
    //                    //playerCursor.transform.rotation = characterCells[actualCursorPosition].transform.rotation;
    //                }
    //            }
    //            else if (Mathf.Abs(player.GetAxis("Horizontal")) < 0.5f && playerCursor.gameObject.activeSelf)
    //            {
    //                joystickPushed = false;
    //            }

    //        }
    //    }
    //    else
    //    {
    //        if (player.GetButtonDown("Return"))
    //        {
    //            playerCursor.gameObject.SetActive(true);
    //            isPlayerReady = false;
    //            DisplayHologram();
    //            characterSelectManager.numberOfReadyPlayers--;
    //            characterSelectManager.HideReadyBands();
    //        }
    //        else if (player.GetButtonDown("Pause"))
    //        {
    //            characterSelectManager.PlayReadySlashAnimation();
    //        }
    //    }
    //}

    //void HideHolograms()
    //{
    //    bernardModel.SetActive(false);
    //    robotioModel.SetActive(false);
    //    ninjaMuraiModel.SetActive(false);
    //    katarinaModel.SetActive(false);
    //    randomModel.SetActive(false);

    //    colorChoiceObject.SetActive(false);
    //}

    //void DisplayHologram()
    //{
    //    if (!colorChoiceObject.activeSelf)
    //        colorChoiceObject.SetActive(true);

    //    foreach (GameObject hologramModel in hologramModels)
    //    {
    //        if (hologramModel.activeSelf)
    //            hologramModel.SetActive(false);
    //    }

    //    if (!hologramModels[currentCursorPosition].activeSelf)
    //        hologramModels[currentCursorPosition].SetActive(true);

    //    switch (currentCursorPosition)
    //    {
    //        case 0:

    //            if (!isPlayerReady)
    //            {
    //                bernardBody.material = hologramMaterial;
    //                bernardHair.material = hologramMaterial;
    //            }
    //            else
    //            {
    //                bernardBody.material = characterCells[currentCursorPosition].characterData.characterMaterials[currentColorSkin];
    //                bernardHair.material = characterCells[currentCursorPosition].characterData.characterMaterials[currentColorSkin];
    //            }

    //            break;

    //        case 1:
    //            break;

    //        case 2:
    //            break;

    //        case 3:
    //            break;

    //        case 4:
    //            break;
    //    }
    //}
}