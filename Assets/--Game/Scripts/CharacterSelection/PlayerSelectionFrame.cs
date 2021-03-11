using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerSelectionFrame : MonoBehaviour
{
    [SerializeField]
    CharacterSelectManager characterSelectManager;

    //-------------------- Cursor -----------------
    [Header("Cursor")]
    [SerializeField]
    PlayerSelectionCoin playerCursor;

    public int actualCursorPosition = 0;

    //-------------------- Hologram Models -----------------
    [Header("Holograms")]
    [SerializeField]
    GameObject bernardModel;

    [SerializeField]
    SkinnedMeshRenderer bernardHair;
    [SerializeField]
    SkinnedMeshRenderer bernardBody;

    [SerializeField]
    GameObject robotioModel;

    [SerializeField]
    GameObject ninjaMuraiModel;

    [SerializeField]
    GameObject katarinaModel;

    [SerializeField]
    GameObject randomModel;

    [SerializeField]
    GameObject[] hologramModels;

    [SerializeField]
    GameObject spotLights;

    [SerializeField]
    Material hologramMaterial;

    [HideInInspector]
    public int actualColorSkin = 0;

    [Header("Skin Color")]
    [SerializeField]
    GameObject colorChoiceObject;

    [SerializeField]
    TextMeshProUGUI colorChoiceText;

    [Space]
    [SerializeField]
    public CharacterSelectionCell[] characterCells = new CharacterSelectionCell[5];

    Player player;

    bool isPlayerConnected = false;
    public bool isPlayerReady = false;

    bool joystickPushed = false;

    enum PlayerInput
    {
        One,
        Two,
        Three,
        Four
    }

    [Space]
    [SerializeField]
    PlayerInput playerInput;

    private void Awake()
    {
        //rectTransform = GetComponent<RectTransform>();

        switch (playerInput)
        {
            case PlayerInput.One:
                player = ReInput.players.GetPlayer(0);
                break;

            case PlayerInput.Two:
                player = ReInput.players.GetPlayer(1);
                break;

            case PlayerInput.Three:
                player = ReInput.players.GetPlayer(2);
                break;

            case PlayerInput.Four:
                player = ReInput.players.GetPlayer(3);
                break;

            default:
                player = null;
                break;
        }
    }

    private void Start()
    {
        isPlayerReady = false;
        isPlayerConnected = false;
        spotLights.SetActive(false);
        playerCursor.gameObject.SetActive(false);
        HideHolograms();
    }

    private void Update()
    {
        if (player.GetButtonDown("LeftTaunt"))
        {
            if(characterSelectManager.playerStocks > 1)
            {
                --characterSelectManager.playerStocks;
                characterSelectManager.UpdateStockText();
            }
        }
        else if (player.GetButtonDown("RightTaunt"))
        {
            if (characterSelectManager.playerStocks < 999)
            {
                ++characterSelectManager.playerStocks;
                characterSelectManager.UpdateStockText();
            }
        }

        if (isPlayerConnected && player.GetButtonDown("RightShoulder"))
        {
            if (actualColorSkin < characterCells[actualCursorPosition].characterData.characterMaterials.Count - 1)
            {
                ++actualColorSkin;
            }
            else
            {
                actualColorSkin = 0;
            }
            colorChoiceText.text = "Color " + (actualColorSkin + 1);
            DisplayHologram();
        }
        else if (isPlayerConnected && player.GetButtonDown("LeftShoulder"))
        {
            if (actualColorSkin > 0)
            {
                --actualColorSkin;
            }
            else
            {
                actualColorSkin = characterCells[actualCursorPosition].characterData.characterMaterials.Count - 1;
            }
            colorChoiceText.text = "Color " + (actualColorSkin + 1);
            DisplayHologram();
        }

        if (!isPlayerReady)
        {
            if (!isPlayerConnected)
            {
                if (player.GetButtonDown("Interact"))
                {
                    spotLights.SetActive(true);
                    playerCursor.gameObject.SetActive(true);

                    actualCursorPosition = 2;
                    DisplayHologram();

                    playerCursor.transform.position = characterCells[2].transform.position;
                    playerCursor.transform.rotation = characterCells[2].transform.rotation;

                    characterSelectManager.numberOfConnectedPlayers++;
                    isPlayerConnected = true;
                }
                else if (player.GetButtonDown("Return"))
                {
                    //Return to previous menu
                }
            }
            else
            {
                if (player.GetButtonDown("Interact"))
                {
                    playerCursor.gameObject.SetActive(false);
                    isPlayerReady = true;

                    characterSelectManager.numberOfReadyPlayers++;
                    characterSelectManager.DisplayReadyBands();

                    DisplayHologram();
                    if (hologramModels[actualCursorPosition].GetComponent<Animator>() != null)
                    {
                        hologramModels[actualCursorPosition].GetComponent<Animator>().SetTrigger("SelectionReady");

                    }
                }
                else if (player.GetButtonDown("Return"))
                {
                    spotLights.SetActive(false);
                    playerCursor.gameObject.SetActive(false);

                    HideHolograms();

                    characterSelectManager.numberOfConnectedPlayers--;
                    isPlayerConnected = false;
                }
                else if (player.GetAxis("Horizontal") > .5f && !joystickPushed)
                {
                    joystickPushed = true;
                    if (actualCursorPosition < 4)
                    {
                        ++actualCursorPosition;
                        DisplayHologram();

                        playerCursor.transform.DOMove(characterCells[actualCursorPosition].transform.position, .2f);
                        playerCursor.transform.DORotateQuaternion(characterCells[actualCursorPosition].transform.rotation, .2f);
                        //playerCursor.transform.position = characterCells[actualCursorPosition].transform.position;
                        //playerCursor.transform.rotation = characterCells[actualCursorPosition].transform.rotation;
                    }
                }
                else if (player.GetAxis("Horizontal") < -.5f && !joystickPushed)
                {
                    joystickPushed = true;
                    if (actualCursorPosition > 0)
                    {
                        --actualCursorPosition;
                        DisplayHologram();

                        playerCursor.transform.DOMove(characterCells[actualCursorPosition].transform.position, .2f);
                        playerCursor.transform.DORotateQuaternion(characterCells[actualCursorPosition].transform.rotation, .2f);
                        //playerCursor.transform.position = characterCells[actualCursorPosition].transform.position;
                        //playerCursor.transform.rotation = characterCells[actualCursorPosition].transform.rotation;
                    }
                }
                else if (Mathf.Abs(player.GetAxis("Horizontal")) < 0.5f && playerCursor.gameObject.activeSelf)
                {
                    joystickPushed = false;
                }

            }
        }
        else
        {
            if (player.GetButtonDown("Return"))
            {
                playerCursor.gameObject.SetActive(true);
                isPlayerReady = false;
                DisplayHologram();
                characterSelectManager.numberOfReadyPlayers--;
                characterSelectManager.HideReadyBands();
            }
            else if (player.GetButtonDown("Pause"))
            {
                characterSelectManager.PlayReadySlashAnimation();
            }
        }
    }

    void HideHolograms()
    {
        bernardModel.SetActive(false);
        robotioModel.SetActive(false);
        ninjaMuraiModel.SetActive(false);
        katarinaModel.SetActive(false);
        randomModel.SetActive(false);

        colorChoiceObject.SetActive(false);
    }

    void DisplayHologram()
    {
        if (!colorChoiceObject.activeSelf)
            colorChoiceObject.SetActive(true);

        foreach (GameObject hologramModel in hologramModels)
        {
            if (hologramModel.activeSelf)
                hologramModel.SetActive(false);
        }

        if (!hologramModels[actualCursorPosition].activeSelf)
            hologramModels[actualCursorPosition].SetActive(true);

        switch (actualCursorPosition)
        {
            case 0:

                if (!isPlayerReady)
                {
                    bernardBody.material = hologramMaterial;
                    bernardHair.material = hologramMaterial;
                }
                else
                {
                    bernardBody.material = characterCells[actualCursorPosition].characterData.characterMaterials[actualColorSkin];
                    bernardHair.material = characterCells[actualCursorPosition].characterData.characterMaterials[actualColorSkin];
                }

                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;
        }
    }
}