using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using DG.Tweening;

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
    Animator cameraTransition;

    [HideInInspector]
    public List<CharacterData> characterDatas;

    [SerializeField]
    public SODatabase_Character databaseCharacter;

    [SerializeField]
    PlayerSelectionFrame[] holograms;

    [SerializeField]
    PlayerSelectionFrame[] inputControlableHolograms;

    private int numberCPU;

    private bool isStarted = false;

    public bool isVolleyBallMode = false;

    [Title("Scenes")]
    [Scene]
    public string beforeMenuSceneName;
    [Scene]
    public string afterMenuSceneSelectionStage;
    //[Scene]
    //public string afterMenuSceneNameBombMode;
    //[Scene]
    //public string afterMenuSceneNameVolleyMode;
    //[Scene]
    //public string afterMenuSceneNameFlappyMode;


    [Title("Canvas UI ")]
    public TextMeshProUGUI gameModeTextUi;
    [SerializeField]
    TextMeshProUGUI numberOfStocksText;
    public TextMeshProUGUI grandSlamSelectionPointText;
    public TextMeshProUGUI grandSlamSelectionBonusText;
    public TextMeshProUGUI volleyGoalText;
    public TextMeshProUGUI selectedObjectText;
    private int currentChoosenParameter = 0;
    public GameObject canvasParameter;
    private RectTransform selectedTransform;
    private Tweener tween;
    private Vector2 tweenVar = new Vector2(1.2f, 1.2f);

    [Title("Sound")]
    public AK.Wwise.Event eventPulse = null;
    public AK.Wwise.Event eventCrow = null;
    public AK.Wwise.Event eventCharacterAdded = null;
    public AK.Wwise.Event eventCharacterSelect = null;
    public AK.Wwise.Event eventColorSelect = null;
    public AK.Wwise.Event eventColorToInput = null;
    public AK.Wwise.Event eventCharacterSelected = null;
    public AK.Wwise.Event eventWhooshEnd = null;

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

        if (gameData.GameMode == GameModeStateEnum.Special_Mode)
        {
            grandSlamSelectionPointText.transform.parent.gameObject.SetActive(true);
            grandSlamSelectionBonusText.transform.parent.gameObject.SetActive(true);
            volleyGoalText.transform.parent.gameObject.SetActive(false);
            numberOfStocksText.transform.parent.gameObject.SetActive(true);
            selectedObjectText.text = "Number of life";
            selectedTransform = numberOfStocksText.transform.parent.GetComponent<RectTransform>();
        }
        else
        {
            grandSlamSelectionPointText.transform.parent.gameObject.SetActive(false);
            grandSlamSelectionBonusText.transform.parent.gameObject.SetActive(false);
            if (gameData.GameMode == GameModeStateEnum.Volley_Mode)
            {
                volleyGoalText.transform.parent.gameObject.SetActive(true);
                numberOfStocksText.transform.parent.gameObject.SetActive(false);
                selectedObjectText.text = "Number of goal";
                selectedTransform = volleyGoalText.transform.parent.GetComponent<RectTransform>();
            }
            else
            {
                volleyGoalText.transform.parent.gameObject.SetActive(false);
                numberOfStocksText.transform.parent.gameObject.SetActive(true);
                selectedObjectText.text = "Number of life";
                selectedTransform = numberOfStocksText.transform.parent.GetComponent<RectTransform>();
            }
        }

        tween = selectedTransform.DOScale(tweenVar, 0.5f).SetLoops(-1, LoopType.Yoyo).OnKill(() => selectedTransform.DOScale(new Vector2(1, 1), 0.01f));

        playerStocks = gameData.ConfigMode.numberOfLife;

        UpdateParameterText();

        switch (gameData.GameMode)
        {
            case GameModeStateEnum.Classic_Mode:
                gameModeTextUi.text = "Classic";
                break;
            case GameModeStateEnum.Special_Mode:
                gameModeTextUi.text = "Grand Slam";
                break;
            case GameModeStateEnum.Bomb_Mode:
                gameModeTextUi.text = "Bomb";
                break;
            case GameModeStateEnum.Volley_Mode:
                gameModeTextUi.text = "Volley";
                break;
            case GameModeStateEnum.Flappy_Mode:
                gameModeTextUi.text = "Wall Splash";
                break;
            case GameModeStateEnum.Training:
                gameModeTextUi.text = "Training";
                break;
            case GameModeStateEnum.Tutorial:
                gameModeTextUi.text = "Tutorial";
                break;
        }

        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i] == null || characterDatas[i].characterName == "Random")
                return;

            characterDatas[i].characterMaterials.Clear();
            UpdateCharacterDataMaterialList(characterDatas[i]);
        }

        
    }

    public void SetTween()
    {
        tween.Kill(selectedTransform);
        tween = selectedTransform.DOScale(tweenVar, 0.5f).SetLoops(-1, LoopType.Yoyo).OnKill(() => selectedTransform.DOScale(new Vector2(1, 1), 0.01f));
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

    public void UpdateParameterText()
    {
        grandSlamSelectionPointText.text = gameData.ConfigMode.numberOfGrandSlamPoint.ToString();
        grandSlamSelectionBonusText.text = gameData.ConfigMode.numberOfGrandSlamBonus.ToString();
        volleyGoalText.text = gameData.ConfigMode.numberOfGoal.ToString();
        numberOfStocksText.text = playerStocks.ToString();
    }
    public void UpdateParameterSelectedText()
    {
        switch (currentChoosenParameter)
        {
            case 0:
                selectedTransform = numberOfStocksText.transform.parent.GetComponent<RectTransform>();
                selectedObjectText.text = "Number of life";
                break;
            case 1:
                selectedTransform = grandSlamSelectionPointText.transform.parent.GetComponent<RectTransform>();
                selectedObjectText.text = "Victory Point";
                break;
            case 2:
                selectedTransform = grandSlamSelectionBonusText.transform.parent.GetComponent<RectTransform>();
                selectedObjectText.text = "Bonus Round";
                break;
        }
        SetTween();
    }

    public void UpdateParameter(bool isDown)
    {
        switch (currentChoosenParameter)
        {
            case 0:
                if (gameData.GameMode == GameModeStateEnum.Volley_Mode)
                {
                    gameData.ConfigMode.numberOfGoal = isDown ? gameData.ConfigMode.numberOfGoal - 1 : gameData.ConfigMode.numberOfGoal + 1;
                    if (gameData.ConfigMode.numberOfGoal < 1)
                        gameData.ConfigMode.numberOfGoal = 99;
                    else if(gameData.ConfigMode.numberOfGoal > 99)
                        gameData.ConfigMode.numberOfGoal = 1;
                }
                else
                {
                    playerStocks = isDown ? playerStocks - 1 : playerStocks + 1;
                    if (playerStocks < 1)
                        playerStocks = 99;
                    else if (playerStocks > 99)
                        playerStocks = 1;
                    gameData.ConfigMode.numberOfLife = playerStocks;
                    if(gameData.GameMode == GameModeStateEnum.Special_Mode)
                        gameData.ConfigMode.numberOfGoal = playerStocks + 1;
                }
                break;

            case 1:
                gameData.ConfigMode.numberOfGrandSlamPoint = isDown ? gameData.ConfigMode.numberOfGrandSlamPoint - 100 : gameData.ConfigMode.numberOfGrandSlamPoint + 100;
                if (gameData.ConfigMode.numberOfGrandSlamPoint < 100)
                    gameData.ConfigMode.numberOfGrandSlamPoint = 9900;
                else if (gameData.ConfigMode.numberOfGrandSlamPoint > 9900)
                    gameData.ConfigMode.numberOfGrandSlamPoint = 100;
                break;

            case 2:
                gameData.ConfigMode.numberOfGrandSlamBonus = isDown ? gameData.ConfigMode.numberOfGrandSlamBonus - 1 : gameData.ConfigMode.numberOfGrandSlamBonus + 1;
                if (gameData.ConfigMode.numberOfGrandSlamBonus < 0)
                    gameData.ConfigMode.numberOfGrandSlamBonus = 10;
                else if (gameData.ConfigMode.numberOfGrandSlamBonus > 10)
                    gameData.ConfigMode.numberOfGrandSlamBonus = 0;
                break;
        }

        UpdateParameterText();
    }

    public void UpdateControl(int ID, Input_Info input_Info)
    {
        if (isStarted) { return; }

        DisplayReadyBands();

        if (input_Info.inputUiAction == InputConst.LeftTaunt)
        {
            //input_Info.inputActions[0].timeValue = 0;
            input_Info.inputUiAction = null;
            if (gameData.GameMode == GameModeStateEnum.Special_Mode)
            {
                currentChoosenParameter--;
                if (currentChoosenParameter < 0)
                    currentChoosenParameter = 2;
                UpdateParameterSelectedText();
            }
        }
        else if (input_Info.inputUiAction == InputConst.RightTaunt)
        {
            //input_Info.inputActions[0].timeValue = 0;
            input_Info.inputUiAction = null;
            if (gameData.GameMode == GameModeStateEnum.Special_Mode)
            {
                currentChoosenParameter++;
                if (currentChoosenParameter > 2)
                    currentChoosenParameter = 0;
                UpdateParameterSelectedText();
            }
        }

        if (input_Info.inputUiAction == InputConst.DownTaunt)
        {
            UpdateParameter(true);
        }
        else if (input_Info.inputUiAction == InputConst.UpTaunt)
        {
            UpdateParameter(false);
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
            AkSoundEngine.PostEvent(eventCharacterAdded.Id, this.gameObject);
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
            if(!holograms[ID].isPlayerConnected || holograms[ID].isPlayerReady)
            {
                AkSoundEngine.PostEvent(eventCharacterAdded.Id, this.gameObject);

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
                AkSoundEngine.PostEvent(eventCharacterSelect.Id, this.gameObject);
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateCursorPosition(true, characterDatas);
            }
            else if (input_Info.horizontal < -.5f && !inputControlableHolograms[ID].joystickPushed)
            {
                AkSoundEngine.PostEvent(eventCharacterSelect.Id, this.gameObject);
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateCursorPosition(false, characterDatas);
            }
            else if (Mathf.Abs(input_Info.horizontal) < .5f)
            {
                inputControlableHolograms[ID].joystickPushed = false;
            }

            if (input_Info.inputUiAction == InputConst.Interact)
            {
                AkSoundEngine.PostEvent(eventCharacterSelected.Id, this.gameObject);
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

                // c'est pourrav ne jamais refaire ça, à changer un jour
                if(inputControlableHolograms[ID].currentChoosedCharacter.characterName == "Pulse")
                    AkSoundEngine.PostEvent(eventPulse.Id, this.gameObject);
                else if (inputControlableHolograms[ID].currentChoosedCharacter.characterName == "Crow")
                    AkSoundEngine.PostEvent(eventCrow.Id, this.gameObject);
            }
            // ================================================================== Pour l'IA
            /*else if (input_Info.inputUiAction == InputConst.Jump)
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
            }*/
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
                AkSoundEngine.PostEvent(eventColorToInput.Id, this.gameObject);
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].ChangeParam();
            }
            else if (input_Info.horizontal > .5f && !inputControlableHolograms[ID].joystickPushed)
            {
                AkSoundEngine.PostEvent(eventColorSelect.Id, this.gameObject);
                inputControlableHolograms[ID].joystickPushed = true;
                inputControlableHolograms[ID].UpdateParam(true, characterDatas);
            }
            else if (input_Info.horizontal < -.5f && !inputControlableHolograms[ID].joystickPushed)
            {
                AkSoundEngine.PostEvent(eventColorSelect.Id, this.gameObject);
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
                AkSoundEngine.PostEvent(eventCharacterSelected.Id, this.gameObject);
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
            canvasParameter.SetActive(false);

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
                        gameData.CharacterInfos[characterInfoNumber].ControllerID = -1 - numberCPU;
                    else
                        gameData.CharacterInfos[characterInfoNumber].ControllerID = holograms[i].iD;
                    characterInfoNumber++;
                    //}
                    if (holograms[i].isCPU)
                        numberCPU++;
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
        AkSoundEngine.PostEvent(eventWhooshEnd.Id, this.gameObject);
        yield return new WaitForSeconds(1.2f);

        StageData currentStage = null;

        for (int i = 0; i < gameData.GameSetting.StagesAvailable.Database.Count; i++)
        {
            if(gameData.GameSetting.StagesAvailable.GetUnlocked(i) && currentStage != null)
            {
                SceneManager.LoadScene(afterMenuSceneSelectionStage);
                yield break;
            }

            if (gameData.GameSetting.StagesAvailable.GetUnlocked(i))
            {
                currentStage = gameData.GameSetting.StagesAvailable.Database[i];
            }

        }

        if(currentStage == null)
            SceneManager.LoadScene(afterMenuSceneSelectionStage);
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
