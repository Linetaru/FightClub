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


    private bool isStarted = false;



    public void UpdateControl(int ID, Input_Info input_Info)
    {
        if(isStarted) { return; }

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
            holograms[ID].Connected(characterDatas);
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
                holograms[ID].UpdateCursorPosition(true, characterDatas);
            }
            else if (input_Info.horizontal < -.5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].UpdateCursorPosition(false, characterDatas);
            }
            else if (Mathf.Abs(input_Info.horizontal) < .5f)
            {
                holograms[ID].joystickPushed = false;
            }

            if (input_Info.inputUiAction == InputConst.Interact)
            {
                if (holograms[ID].currentCursorPosition == 2)
                {
                    holograms[ID].RandomReady(characterDatas);
                }
                else
                {
                    if (characterDatas[holograms[ID].currentCursorPosition] != null)
                    {
                        holograms[ID].ChooseCharacter(characterDatas);
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
                holograms[ID].UpdateParam(true, characterDatas);
            }
            else if (input_Info.horizontal < -.5f && !holograms[ID].joystickPushed)
            {
                holograms[ID].joystickPushed = true;
                holograms[ID].UpdateParam(false, characterDatas);
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
            isStarted = true;
            gameLaunched = true;
            gameData.NumberOfLifes = playerStocks;



            int characterInfoNumber = 0;

            gameData.CharacterInfos.Clear();
            gameData.CharacterInfos = new List<Character_Info>(numberOfReadyPlayers);

            for (int i = 0; i < numberOfReadyPlayers; i++)
            {
                gameData.CharacterInfos.Add(new Character_Info());
            }
            cameraTransition.SetTrigger("Feedback");
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
                    if (gameData.CharacterInfos[i] != null)
                    {
                        if(holograms[i].currentChoosedCharacter != null)
                            gameData.CharacterInfos[i].CharacterData = holograms[i].currentChoosedCharacter;
                        gameData.CharacterInfos[i].CharacterColorID = holograms[i].currentColorSkin;
                    }
                }
            }
            readySlash.SetActive(true);

            StartCoroutine(GoToStageMenu());
            //SceneManager.LoadScene("MenuSelection_Stage");
        }
    }

    private IEnumerator GoToStageMenu()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("MenuSelection_Stage");
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("GP_Menu");
    }
}
