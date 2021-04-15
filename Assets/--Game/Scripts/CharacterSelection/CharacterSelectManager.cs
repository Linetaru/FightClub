using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour, IControllable
{

    //[SerializeField]
    //PlayerSelectionFrame[] players;

    [SerializeField]
    GameObject readyBands;
    [SerializeField]
    GameObject readySlash;

    [SerializeField]
    GameData gameData;

    public int numberOfReadyPlayers = 0; //A changer et à faire avec des events
    public int numberOfConnectedPlayers = 0; //A changer et à faire avec des events

    public bool gameLaunched = false;

    int gameDuration = 8;
    [HideInInspector]
    public int playerStocks = 3;

    [SerializeField]
    TextMeshProUGUI numberOfStocksText;

    [SerializeField]
    Animator cameraTransition;


    [SerializeField]
    public CharacterData[] characterDatas;

    [SerializeField]
    PlayerSelectionFrame[] holograms;

    //bool[] playersReadyStates = new bool[4];

    public static CharacterSelectManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            //Rest of your Awake code
            UpdateStockText();

        }
        else
        {
            Destroy(this);
        }

    }

    //private void Start()
    //{

    //}

    public void UpdateControl(int ID, Input_Info input_Info)
    {
        DisplayReadyBands();

        if (input_Info.CheckAction(0, InputConst.LeftTaunt))
        {
            if (playerStocks > 1)
            {
                playerStocks--;
                UpdateStockText();
            }
        }
        else if (input_Info.CheckAction(0, InputConst.RightTaunt))
        {
            if (playerStocks < 99)
            {
                playerStocks++;
                UpdateStockText();
            }
        }

        //Quand le joueur n'est pas connecté


        if (!holograms[ID].isPlayerConnected && input_Info.inputUiAction == InputConst.Interact)
        {
            holograms[ID].isPlayerConnected = true;
            numberOfConnectedPlayers++;
            holograms[ID].Connected();
            //playersReadyStates[ID] = true;
            input_Info.inputUiAction = null;
        }

        if (!holograms[ID].isPlayerConnected && input_Info.inputUiAction == InputConst.Return)
        {
            foreach (PlayerSelectionFrame hologram in holograms)
            {
                hologram.Disconnected();
            }
            ReturnToMainMenu();
            input_Info.inputUiAction = null;
        }

        //Quand le joueur est connecté mais qu'il n'as pas choisi son perso

        if (holograms[ID].isPlayerConnected && !holograms[ID].isCharacterChoosed)
        {
            if (input_Info.horizontal > .5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].UpdateCursorPosition(true);
            }
            else if (input_Info.horizontal < -.5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].UpdateCursorPosition(false);
            }
            else if (Mathf.Abs(input_Info.horizontal) < .5f)
            {
                holograms[ID].joystickPushed = false;
            }

            if (input_Info.inputUiAction == InputConst.Interact)
            {
                if (holograms[ID].currentCursorPosition == 2)
                {
                    holograms[ID].RandomReady();
                }
                else
                {
                    if (characterDatas[holograms[ID].currentCursorPosition] != null)
                    {
                        holograms[ID].ChooseCharacter();
                    }
                }
                input_Info.inputUiAction = null;
            }

            if (input_Info.inputUiAction == InputConst.Return)
            {
                holograms[ID].Disconnected();
                numberOfConnectedPlayers--;
                input_Info.inputUiAction = null;
            }
        }

        //Quand le joueur a choisi son perso mais pas sa couleur et son skill

        if (holograms[ID].isCharacterChoosed && !holograms[ID].paramsChoosed)
        {
            if (Mathf.Abs(input_Info.vertical) > .5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].ChangeParam();
            }
            else if (input_Info.horizontal > .5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].UpdateParam(true);
            }
            else if (input_Info.horizontal < -.5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].UpdateParam(false);
            }
            else if (Mathf.Abs(input_Info.vertical) < .5f && (Mathf.Abs(input_Info.horizontal) < .5f))
            {
                holograms[ID].joystickPushed = false;
            }
            //else if (Mathf.Abs(input_Info.horizontal) < .5f)
            //{
            //    holograms[ID].joystickPushed = false;
            //}

            if (input_Info.inputUiAction == InputConst.Interact)
            {
                holograms[ID].Ready();
                numberOfReadyPlayers++;
                input_Info.inputUiAction = null;
            }

            if (input_Info.inputUiAction == InputConst.Return)
            {
                holograms[ID].UnchooseCharacter();
                input_Info.inputUiAction = null;
            }
        }

        // Quand le joueur est prêt

        if ((holograms[ID].isCharacterChoosed && holograms[ID].paramsChoosed && holograms[ID].isPlayerReady))
        {
            if (input_Info.inputUiAction == InputConst.Pause)
            {
                PlayReadySlashAnimation();
            }

            if (input_Info.inputUiAction == InputConst.Return)
            {
                HideReadyBands();
                holograms[ID].NotReady();
            }
        }

    }

    public void UpdateStockText()
    {
        numberOfStocksText.text = playerStocks.ToString();
    }

    public void DisplayReadyBands()
    {
        if (numberOfConnectedPlayers > 1 && numberOfReadyPlayers == numberOfConnectedPlayers)
            readyBands.SetActive(true);
    }

    public void HideReadyBands()
    {
        readyBands.SetActive(false);
    }

    public void PlayReadySlashAnimation()
    {
        if (!gameLaunched && numberOfReadyPlayers == numberOfConnectedPlayers && numberOfConnectedPlayers > 1)
        {
            gameLaunched = true;
            readySlash.SetActive(true);
            gameData.NumberOfLifes = playerStocks;

            StartCoroutine(GoToStageMenu());

            int characterInfoNumber = 0;

            gameData.CharacterInfos.Clear();

            for (int i = 0; i < numberOfReadyPlayers; i++)
            {
                gameData.CharacterInfos.Add(new Character_Info());
            }
            //Debug.Log(players.Length);

            //for (int i = 0; i < players.Length; i++)
            //{
            //    Debug.Log(players[i].currentCursorPosition);

            //    if (players[i].isPlayerReady)
            //    {
            //        Debug.Log(players[i].currentCursorPosition);
            //        gameData.CharacterInfos[characterInfoNumber].CharacterData = players[i].characterCells[players[i].currentCursorPosition].characterData;
            //        gameData.CharacterInfos[characterInfoNumber].CharacterColorID = players[i].currentColorSkin;
            //        characterInfoNumber++;
            //        //gameData.CharacterInfos[characterInfoNumber].CharacterData.
            //    }
            //}
            for (int i = 0; i < holograms.Length; i++)
            {

                if (holograms[i].isPlayerReady)
                {
                    gameData.CharacterInfos[characterInfoNumber].CharacterData = holograms[i].currentChoosedCharacter;
                    gameData.CharacterInfos[characterInfoNumber].CharacterColorID = holograms[i].currentColorSkin;
                    characterInfoNumber++;
                    //gameData.CharacterInfos[characterInfoNumber].CharacterData.
                }
            }
        }
    }

    IEnumerator GoToStageMenu()
    {
        cameraTransition.SetTrigger("Feedback");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("MenuSelection_Stage");
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("GP_Menu");
    }
}
