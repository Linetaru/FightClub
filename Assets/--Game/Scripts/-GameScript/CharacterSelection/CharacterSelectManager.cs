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

    [HideInInspector]
    public List<CharacterData> characterDatas;

    [SerializeField]
    public SODatabase_Character databaseCharacter;

    [SerializeField]
    PlayerSelectionFrame[] holograms;

    [SerializeField]
    PlayerSelectionFrame[] inputControlableHolograms;


    private bool isStarted = false;

    public bool isVolleyBallMode = false;

    [Scene]
    public string beforeMenuSceneName;
    [Scene]
    public string afterMenuSceneNameClassicMode;
    [Scene]
    public string afterMenuSceneNameBombMode;
    [Scene]
    public string afterMenuSceneNameVolleyMode;
    [Scene]
    public string afterMenuSceneNameFlappyMode;

    private void Start()
    {
        inputControlableHolograms = new PlayerSelectionFrame[4];
        inputControlableHolograms[0] = holograms[0];
        inputControlableHolograms[1] = holograms[1];
        inputControlableHolograms[2] = holograms[2];
        inputControlableHolograms[3] = holograms[3];
        characterDatas.Clear();
        for (int i = 0; i < databaseCharacter.Database.Count; i++)
        {
            if (databaseCharacter.GetUnlocked(i))
            {
                //Debug.LogError(databaseCharacter.Database[i]);
                characterDatas.Add(databaseCharacter.Database[i]);
            }
            else
            {
                characterDatas.Add(null);
            }
        }

        //foreach(CharacterData characterData1 in characterDatas)
        //{
        //        Debug.LogError(characterData1);

        //}

        //var tmp = characterDatas[2];
        //characterDatas[2] = characterDatas[3];
        //characterDatas[3] = tmp;

        //foreach (CharacterData characterData in characterDatas)
        //{
        //    //Debug.LogError(characterData);

        //    if (characterData.name != "Random")
        //    {
        //        characterData.characterMaterials.Clear();
        //        UpdateCharacterDataMaterialList(characterData);

        //    }
        //}

        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i] == null || characterDatas[i].characterName == "Random")
                return;

            characterDatas[i].characterMaterials.Clear();
            UpdateCharacterDataMaterialList(characterDatas[i]);

        }
    }

    private void UpdateCharacterDataMaterialList(CharacterData characterData)
    {
        if (characterData.characterName == "Random" || characterData == null)
            return;
        for (int i = 0; i < characterData.characterColors.Database.Count; i++)
        {
            if (characterData.characterColors.GetUnlocked(i))
            {
                characterData.characterMaterials.Add(characterData.characterColors.Database[i].material);
            }
        }

        //foreach(ColorData colorData in characterData.characterColors.)
        //{
        //    colorData.
        //}
    }


    public void UpdateControl(int ID, Input_Info input_Info)
    {
        if (isStarted) { return; }

        DisplayReadyBands();

        if (input_Info.inputUiAction == InputConst.LeftTaunt)
        {
            if (playerStocks > 1)
            {
                //input_Info.inputActions[0].timeValue = 0;
                input_Info.inputUiAction = null;
                playerStocks--;
                UpdateStockText();
            }
        }
        else if (input_Info.inputUiAction == InputConst.RightTaunt)
        {
            if (playerStocks < 99)
            {
                //input_Info.inputActions[0].timeValue = 0;
                input_Info.inputUiAction = null;
                playerStocks++;
                UpdateStockText();
            }
        }
        else if (input_Info.inputUiAction == InputConst.LeftTrigger)
        {
            //input_Info.inputActions[0].timeValue = 0;
            input_Info.inputUiAction = null;

            for (int i = 0; i < holograms.Length; i++)
            {
                if (!holograms[i].isPlayerConnected)
                {
                    numberOfConnectedPlayers++;
                    numberOfReadyPlayers++;
                    holograms[i].RandomReadyCPU(characterDatas);
                    break;
                }
            }
        }
        else if (input_Info.inputUiAction == InputConst.RightTrigger)
        {
            //input_Info.inputActions[0].timeValue = 0;
            input_Info.inputUiAction = null;

            for (int i = holograms.Length - 1; i >= 0; i--)
            {
                if (holograms[i].isPlayerConnected && holograms[i].isCPU)
                {
                    numberOfConnectedPlayers--;
                    numberOfReadyPlayers--;
                    holograms[i].Disconnected();
                    break;
                }
            }

            if (numberOfReadyPlayers < 2)
            {
                HideReadyBands();
            }
        }

        //Quand le joueur n'est pas connecté

        if (!holograms[ID].isPlayerConnected && input_Info.inputUiAction == InputConst.Interact)
        {
            //input_Info.inputActions[0].timeValue = 0;
            holograms[ID].isPlayerConnected = true;
            numberOfConnectedPlayers++;
            holograms[ID].Connected(characterDatas);
            inputControlableHolograms[ID] = holograms[ID];
            //playersReadyStates[ID] = true;
            input_Info.inputUiAction = null;

            HideReadyBands();
        }

        if (/*!holograms[ID].isPlayerConnected && */input_Info.inputUiAction == InputConst.RightShoulder)
        {
            //input_Info.inputActions[0].timeValue = 0;
            Debug.LogError("Pressed R1");

            HideReadyBands();

            for (int i = 0; i < holograms.Length; i++)
            {
                if (!holograms[i].isPlayerConnected)
                {
                    numberOfConnectedPlayers++;
                    if (holograms[i] != inputControlableHolograms[ID])
                    {
                        inputControlableHolograms[ID] = holograms[i];
                        //holograms[ID] = null;
                    }
                    inputControlableHolograms[ID].isCPU = true;
                    inputControlableHolograms[ID].Connected(characterDatas);
                    //holograms[i].RandomReadyCPU(characterDatas);
                    break;
                }
            }
            input_Info.inputUiAction = null;
        }


        if (!holograms[ID].isPlayerConnected && input_Info.inputUiAction == InputConst.Return)
        {
            foreach (PlayerSelectionFrame hologram in holograms)
            {
                if (hologram.isCPU)
                    hologram.isCPU = false;
                hologram.Disconnected();
            }
            ReturnToMainMenu();
            input_Info.inputUiAction = null;
            //input_Info.inputActions[0].timeValue = 0;
        }

        //Quand le joueur est connecté mais qu'il n'as pas choisi son perso

        //A PARTIR D'ICI J'AI REMPLACE "hologram" PAR "inputControlableHologram"

        if (inputControlableHolograms[ID].isPlayerConnected && !inputControlableHolograms[ID].isCharacterChoosed)
        {
            if (input_Info.horizontal > .5f && !inputControlableHolograms[ID].joystickPushed)
            {
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateCursorPosition(true, characterDatas);
            }
            else if (input_Info.horizontal < -.5f && !inputControlableHolograms[ID].joystickPushed)
            {
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateCursorPosition(false, characterDatas);
            }
            else if (Mathf.Abs(input_Info.horizontal) < .5f)
            {
                inputControlableHolograms[ID].joystickPushed = false;
            }

            if (input_Info.inputUiAction == InputConst.Interact)
            {
                if (inputControlableHolograms[ID].currentCursorPosition == 2)
                {
                    inputControlableHolograms[ID].RandomReady(characterDatas);
                    numberOfReadyPlayers++;
                }
                else
                {
                    if (characterDatas[inputControlableHolograms[ID].currentCursorPosition] != null)
                    {
                        inputControlableHolograms[ID].ChooseCharacter(characterDatas);
                    }
                }
                input_Info.inputUiAction = null;
                inputControlableHolograms[ID].iD = ID;
            }
            // ================================================================== Pour l'IA
            else if (input_Info.inputUiAction == InputConst.Jump)
            {
                if (inputControlableHolograms[ID].currentCursorPosition == 2)
                {
                    inputControlableHolograms[ID].RandomReady(characterDatas);
                    numberOfReadyPlayers++;
                }
                else
                {
                    if (characterDatas[inputControlableHolograms[ID].currentCursorPosition] != null)
                    {
                        inputControlableHolograms[ID].ChooseCharacter(characterDatas);
                    }
                }
                input_Info.inputUiAction = null;
                inputControlableHolograms[ID].iD = -1;
            }
            // ==================================================================
            if (input_Info.inputUiAction == InputConst.Return)
            {
                if (holograms[ID] != inputControlableHolograms[ID])
                {
                    for (int i = 0; i < inputControlableHolograms.Length; i++)
                    {
                        if (inputControlableHolograms[i] == null)
                            inputControlableHolograms[i] = inputControlableHolograms[ID];
                    }
                    inputControlableHolograms[ID] = holograms[ID];
                    inputControlableHolograms[ID].isCPU = false;

                }
                holograms[ID].Disconnected();
                numberOfConnectedPlayers--;
                input_Info.inputUiAction = null;
            }
        }

        //Quand le joueur a choisi son perso mais pas sa couleur et son skill

        if (inputControlableHolograms[ID].isCharacterChoosed && !inputControlableHolograms[ID].paramsChoosed)
        {
            if (Mathf.Abs(input_Info.vertical) > .5f && !inputControlableHolograms[ID].joystickPushed)
            {
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].ChangeParam();
            }
            else if (input_Info.horizontal > .5f && !inputControlableHolograms[ID].joystickPushed)
            {
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateParam(true, characterDatas);
            }
            else if (input_Info.horizontal < -.5f && !inputControlableHolograms[ID].joystickPushed)
            {
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateParam(false, characterDatas);
            }
            else if (Mathf.Abs(input_Info.vertical) < .5f && (Mathf.Abs(input_Info.horizontal) < .5f))
            {
                inputControlableHolograms[ID].joystickPushed = false;
            }
            //else if (Mathf.Abs(input_Info.horizontal) < .5f)
            //{
            //    holograms[ID].joystickPushed = false;
            //}


            // Change Team
            if (input_Info.inputUiAction == InputConst.Jump)
            {
                // Cycle team
                inputControlableHolograms[ID].CycleTeam(isVolleyBallMode);
            }

            if (input_Info.inputUiAction == InputConst.Interact)
            {
                inputControlableHolograms[ID].Ready();
                numberOfReadyPlayers++;
                input_Info.inputUiAction = null;
            }

            if (input_Info.inputUiAction == InputConst.Return)
            {
                inputControlableHolograms[ID].UnchooseCharacter();
                input_Info.inputUiAction = null;
            }
        }

        // Quand le joueur est prêt

        if ((inputControlableHolograms[ID].isCharacterChoosed && inputControlableHolograms[ID].paramsChoosed && inputControlableHolograms[ID].isPlayerReady))
        {
            if (input_Info.inputUiAction == InputConst.Pause && PlayersDifferentTeam())
            {
                PlayReadySlashAnimation();
            }

            if (input_Info.inputUiAction == InputConst.Return)
            {
                numberOfReadyPlayers--;
                HideReadyBands();
                inputControlableHolograms[ID].NotReady();
            }
        }

    }

    public void UpdateStockText()
    {
        numberOfStocksText.text = playerStocks.ToString();
    }

    public void DisplayReadyBands()
    {
        if (numberOfConnectedPlayers > 1 && numberOfReadyPlayers == numberOfConnectedPlayers && PlayersDifferentTeam())
            readyBands.SetActive(true);
    }

    public void HideReadyBands()
    {
        readyBands.SetActive(false);
    }

    public void PlayReadySlashAnimation()
    {
        if (!gameLaunched && numberOfReadyPlayers == numberOfConnectedPlayers && numberOfConnectedPlayers > 1 && PlayersDifferentTeam())
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
                    //if (gameData.CharacterInfos.Count > i)
                    //{
                    gameData.CharacterInfos[characterInfoNumber].CharacterData = holograms[i].currentChoosedCharacter;
                    gameData.CharacterInfos[characterInfoNumber].CharacterColorID = holograms[i].currentColorSkin;
                    if (holograms[i].currentConfigChoice == 0)
                        gameData.CharacterInfos[characterInfoNumber].InputMapping = new InputMappingDataClassic("classic");
                    else
                    {
                        gameData.CharacterInfos[characterInfoNumber].InputMapping = InputMappingDataStatic.inputMappingDataClassics[holograms[i].currentConfigChoice - 1];
                        gameData.CharacterInfos[characterInfoNumber].InputMapping.isUsed = true;
                    }

                    //Debug.Log(holograms[i].currentChoosedCharacter.name);

                    //Assign Team
                    gameData.CharacterInfos[characterInfoNumber].Team = holograms[i].currentTeam;
                    if (holograms[i].isCPU)
                        gameData.CharacterInfos[characterInfoNumber].ControllerID = -1;
                    else
                        gameData.CharacterInfos[characterInfoNumber].ControllerID = holograms[i].iD;
                    characterInfoNumber++;
                    //}
                }
            }
            readySlash.SetActive(true);

            StartCoroutine(GoToStageMenu());
            //SceneManager.LoadScene("MenuSelection_Stage");
        }
    }

    // Vérifie qu'au moins 2 joueurs soit dans des teams différentes
    private bool PlayersDifferentTeam()
    {
        int noTeamCounter = 0;
        for (int i = 0; i < numberOfReadyPlayers - 1; i++)
        {
            if (holograms[i].currentTeam != holograms[i + 1].currentTeam)
            {
                return true;
            }
            else if (holograms[i].currentTeam == TeamEnum.No_Team)
                noTeamCounter++;
        }
        if (noTeamCounter == numberOfReadyPlayers - 1)
            return true;
        return false;
    }

    private IEnumerator GoToStageMenu()
    {
        yield return new WaitForSeconds(1.2f);

        StageData currentStage = null;

        for (int i = 0; i < gameData.GameSetting.StagesAvailable.Database.Count; i++)
        {
            if(gameData.GameSetting.StagesAvailable.GetUnlocked(i) && currentStage != null)
            {
                SceneManager.LoadScene(afterMenuSceneNameClassicMode);
                yield break;
            }

            if (gameData.GameSetting.StagesAvailable.GetUnlocked(i))
            {
                currentStage = gameData.GameSetting.StagesAvailable.Database[i];
            }

        }

        if(currentStage == null)
            SceneManager.LoadScene(afterMenuSceneNameClassicMode);
        else
            SceneManager.LoadScene(currentStage.SceneName);

        //if (gameData.GameMode == GameModeStateEnum.Classic_Mode || gameData.GameMode == GameModeStateEnum.Training)
        //    SceneManager.LoadScene(afterMenuSceneNameClassicMode);
        //else if (gameData.GameMode == GameModeStateEnum.Bomb_Mode)
        //    SceneManager.LoadScene(afterMenuSceneNameBombMode);
        //else if (gameData.GameMode == GameModeStateEnum.Volley_Mode)
        //    SceneManager.LoadScene(afterMenuSceneNameVolleyMode);
        //else if (gameData.GameMode == GameModeStateEnum.Flappy_Mode)
        //    SceneManager.LoadScene(afterMenuSceneNameFlappyMode);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(beforeMenuSceneName);
    }
}
