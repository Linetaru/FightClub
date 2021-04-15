using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using DG.Tweening;

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

    GameObject characterModel = null;

    [SerializeField]
    Material HologramMaterial;

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

    public string menuSceneName;


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
    }

    public void UpdateDisplay()
    {
        if (isPlayerConnected)
        {
            spotLights.SetActive(true);
            playerCursor.SetActive(true);
            currentCursorPosition = 0;
            UpdateCharacterColor(CharacterSelectManager._instance.characterDatas[currentCursorPosition].characterMaterials);
        }
        else
        {
            spotLights.SetActive(false);
            playerCursor.SetActive(false);
            UpdateCharacterModel(null);
        }

        if (isPlayerConnected && isCharacterChoosed)
        {
            spotLights.SetActive(true);
            playerCursor.SetActive(false);
        }

        if (isCharacterChoosed && !paramsChoosed)
        {
            characterParams.SetActive(true);
            colorChoiceText.text = "Color " + currentChoosableSkill + 1;
            skillChoiceText.text = choosableSkills[currentChoosableSkill];
            choosableSkillIcon.color = choosableSkillsColor[currentChoosableSkill];

        }
    }

    public void UpdateCharacterModel(GameObject characterModel)
    {
        if (characterModel != null || CharacterSelectManager._instance.characterDatas[currentCursorPosition] != null)
        {
            this.characterModel = Instantiate(characterModel, transform.position, new Quaternion(0, 180, 0, 0));
        }
        else
        {
            this.characterModel = null;
        }
    }

    public void UpdateCharacterColor(List<Material> characterMaterials)
    {
        if (!isCharacterChoosed && characterModel != null)
        {
            foreach (SkinnedMeshRenderer mesh in characterModel.transform)
            {
                mesh.material = HologramMaterial;
            }
        }
        else if (isCharacterChoosed)
        {
            foreach (SkinnedMeshRenderer mesh in characterModel.transform)
            {
                mesh.material = characterMaterials[currentColorSkin];
            }
        }
    }

    public void UpdateCursorPosition(bool goToRight)
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
    }

    public void ChooseCharacter()
    {
        currentChoosedCharacter = CharacterSelectManager._instance.characterDatas[currentCursorPosition];
        isPlayerConnected = true;
        isCharacterChoosed = true;
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
    }

    public void UpdateParam(bool goToRight)
    {
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

    }

    public void Ready()
    {
        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = true;
        isPlayerReady = true;
    }

    public void RandomReady()
    {
        currentChoosedCharacter = CharacterSelectManager._instance.characterDatas[0];
        currentColorSkin = Random.Range(0, CharacterSelectManager._instance.characterDatas[0].characterMaterials.Count);
        currentChoosableSkill = Random.Range(0, choosableSkills.Length);

        isPlayerConnected = true;
        isCharacterChoosed = true;
        paramsChoosed = true;
        isPlayerReady = true;
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