using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;

public enum SpecialRound
{
    NoCurrentSpecialRound = 0,
    DoublePoint = 1,
    StealPoint = 2,
    OneMoreLife = 3,
}

public class GrandSlamManager : MonoBehaviour
{
    [Title("Data")]
    [SerializeField]
    private int scoreGoal2Players = 800;
    [SerializeField]
    private int scoreGoal3Players = 1200;
    [SerializeField]
    private int scoreGoal4Players = 1800;

    private int scoreToWin = 1500;
    [SerializeField]
    private float timeOnScore = 6f;


    [Title("GameData")]
    [Expandable]
    public GameData gameData;

    [Title("Lists")]
    [SerializeField]
    List<SlamMode> listGameModesValid = new List<SlamMode>();

    List<SlamMode> currentListGameModesValid;

    List<string> listToPickFrom = new List<string>();

    List<CharacterBase> podium = new List<CharacterBase>();

    SlamMode currentMode;

    // Dictionary<ControllerID, Score>
    Dictionary<int, int> playersScore = new Dictionary<int, int>();

    private int[] currentScoreArr = new int[4];

    [Button]
    public void UpdateGameModeList()
    {
        listGameModesValid = new List<SlamMode>(GetComponentsInChildren<SlamMode>());
    }

    GameModeStateEnum gameMode;

    [Title("Objects")]
    [SerializeField]
    private GameObject cameraObj;
    [SerializeField]
    private CameraSlam camSlam;
    [SerializeField]
    private GrandSlamUi canvasScore;
    [SerializeField]
    private SlamLogoMode slamLogoMode;
    [SerializeField]
    private BonusRoundPanel bonusPanel;

    private Camera currentCam;


    bool firstRound = true;

    bool isUnloaded;
    bool isLoaded;

    bool moveCamera = false;

    bool almostOver = false;

    bool pressToContinue = false;

    [Title("Values")]
    [SerializeField]
    private float cameraResetSpeed = 1.0f;

    UnityEvent gameEndedEvent;


    private string nextSceneName;

    // A PASSER EN ENUM PUBLIC A L'EXTERIEUR

    [SerializeField]
    int specialRoundsOcurrence = 2;
    [SerializeField] [ReadOnly]
    SpecialRound currentSpecialRound;
    int roundCount = 0;


    private void Awake()
    {

    }

    private void Update()
    {
        if(moveCamera)
        {
            float step = cameraResetSpeed * Time.deltaTime;
            cameraObj.transform.position = Vector3.MoveTowards(cameraObj.transform.position, currentCam.transform.position, step);

            if(Vector3.Distance(cameraObj.transform.position, currentCam.transform.position) < 0.001f)
            {
                cameraObj.transform.position = currentCam.transform.position;
                moveCamera = false;
            }
        }
    }

    private void Start()
    {
        gameData.GameMode = GameModeStateEnum.Special_Mode;

        gameData.SetSkipIntro(GameModeStateEnum.Special_Mode, false);

        gameData.SetGameSettings();

        gameData.slamMode = true;

        InitScoreGoal();
        InitScoreDictionary();
        AdjustModeList();

        InitFirstRound();

    }

    private void InitFirstRound()
    {
        roundCount++;

        listToPickFrom = listGameModesValid[0].scenes;
        nextSceneName = listToPickFrom[0];

        gameMode = listGameModesValid[0].gameMode;
        gameData.NumberOfLifes = listGameModesValid[0].nbLife;
        currentScoreArr = listGameModesValid[0].scoreArr;
        currentMode = listGameModesValid[0];

        StartCoroutine(LoadSceneAsync());
    }

    private void InitScoreGoal()
    {
        if(gameData.CharacterInfos.Count == 2)
        {
            scoreToWin = scoreGoal2Players;
        }
        else if (gameData.CharacterInfos.Count == 3)
        {
            scoreToWin = scoreGoal3Players;
        }
        else
        {
            scoreToWin = scoreGoal4Players;
        }

        canvasScore.InitProperty(scoreToWin, gameData);
    }

    // Récupère une scène parmi la liste du mode choisi
    private string GetRandomSceneFromList()
    {
        listToPickFrom = GetRandomModeList();
        string sceneName = listToPickFrom[UnityEngine.Random.Range(0, listToPickFrom.Count)];
        Debug.Log("Next scene to load = " + sceneName);

        return sceneName;
    }


    // Cette fonction sélectionne le mode et retourne la liste de scènes associée
    // Retire le mode en cours de la liste pour le tirage
    private List<string> GetRandomModeList()
    {


        if (!almostOver && PlayerAboutToWin())
        {
            SlamMode slamMode = RemoveMode(GameModeStateEnum.Volley_Mode);

            if(slamMode == currentMode)
            {
                currentMode = null;
            }

            almostOver = true;
        }

        if(currentMode != null)
            listGameModesValid.Remove(currentMode);

        if (listGameModesValid.Count == 0)
        {
            listGameModesValid = new List<SlamMode>(currentListGameModesValid);
        }

        int randomKey = UnityEngine.Random.Range(0, listGameModesValid.Count);
        gameMode = listGameModesValid[randomKey].gameMode;

        gameData.NumberOfLifes = currentSpecialRound == SpecialRound.OneMoreLife ? listGameModesValid[randomKey].nbLife + 1 : listGameModesValid[randomKey].nbLife;
        //gameData.NumberOfLifes = listGameModesValid[randomKey].nbLife;

        currentScoreArr = listGameModesValid[randomKey].scoreArr;

        if (gameMode == GameModeStateEnum.Volley_Mode)
        {
            for (int i = 0; i < gameData.CharacterInfos.Count; i++)
            {
                gameData.CharacterInfos[i].Team = TeamEnum.First_Team;
            }
        }
        else
        {
            for (int i = 0; i < gameData.CharacterInfos.Count; i++)
            {
                gameData.CharacterInfos[i].Team = TeamEnum.No_Team;
            }
        }

        currentMode = listGameModesValid[randomKey];


        //Si mode Volley au lieu de donner une vie ca rajouter 1 goal dans l'objectif.
        if(gameMode == GameModeStateEnum.Volley_Mode)
        {
            if (currentSpecialRound == SpecialRound.OneMoreLife)
                gameData.SetModeScoreGoal(currentMode.gameMode, currentMode.scoreGoal + 1);
            else
                gameData.SetModeScoreGoal(currentMode.gameMode, currentMode.scoreGoal);

            canvasScore.logoTransition.SetScoreGoal(gameData.GetModeScoreGoal(GameModeStateEnum.Volley_Mode));
        }

        return currentMode.scenes;
    }

    private bool PlayerAboutToWin()
    {
        foreach(KeyValuePair<int, int> score in playersScore)
        {
            if (scoreToWin - score.Value <= currentMode.scoreArr[0]) // 300)
            {
                return true;
            }
        }
        return false;
    }

    private SlamMode RemoveMode(GameModeStateEnum gameMode)
    {
        List<SlamMode> copySlamMode = new List<SlamMode>(listGameModesValid);

        foreach (SlamMode slam in copySlamMode)
        {
             if(slam.gameMode == gameMode)
            {
                listGameModesValid.Remove(slam);
                return slam;
            }
        }
        return null;
    }


    // Cette fonction retire des modes de la liste en fonction du nombre de joueurs
    // On ajoute aussi des settings particulier à certains modes
    private void AdjustModeList()
    {
        List<SlamMode> copySlamMode = new List<SlamMode>(listGameModesValid);

        foreach (SlamMode slam in copySlamMode)
        {
            if (gameData.CharacterInfos.Count == 3 && slam.gameMode == GameModeStateEnum.Volley_Mode)
            {
                listGameModesValid.Remove(slam);
            }
            else if (slam.gameMode == GameModeStateEnum.Volley_Mode)
            {
                gameData.SetModeScoreGoal(slam.gameMode, slam.scoreGoal);
            }

            if (gameData.CharacterInfos.Count == 2 && slam.gameMode == GameModeStateEnum.Bomb_Mode)
            {
                listGameModesValid.Remove(slam);
            }
        }

        currentListGameModesValid = new List<SlamMode>(listGameModesValid);
    }

    private void InitScoreDictionary()
    {
        foreach(Character_Info character in gameData.CharacterInfos)
        {
            playersScore.Add(character.ControllerID, 0);
        }
    }


    // Calcule les scores et demande au canvas de les afficher
    private void CalculateScore()
    {
        int[] scoreArr = (int[]) currentScoreArr.Clone();


        if(currentSpecialRound != SpecialRound.NoCurrentSpecialRound)
        {
            if (currentSpecialRound == SpecialRound.DoublePoint)
            {
                for (int i = 0; i < scoreArr.Length; i++)
                {
                    scoreArr[i] *= 2;
                }
            }
            else if (currentSpecialRound == SpecialRound.StealPoint)
            {
                scoreArr[0] = 36;
                
                for(int i = 1; i < scoreArr.Length; i++)
                {
                    scoreArr[i] = -(36 / (gameData.CharacterInfos.Count - 1));
                }
            }
        }


        canvasScore.SetCurrentModeInfo(gameMode);
        if(gameMode == GameModeStateEnum.Volley_Mode)
        {
            // Condition points volley
            // Si bleus gagne - Joueur 1 et 3 gagnent les points de currentScoreArr[0] et Joueur 2 et 4 gagnent les points de currentScoreArr[1]
            int winnerPoints = scoreArr[0];
            int loserPoints = scoreArr[1];

            int winnerTeam = BattleManager.Instance.currentWinningTeam;

            for(int i = 0; i < gameData.CharacterInfos.Count; i++)
            {
                if(winnerTeam == 0)
                {
                    if(i == 0 || i == 2)
                        playersScore[gameData.CharacterInfos[i].ControllerID] += winnerPoints;
                    else
                        playersScore[gameData.CharacterInfos[i].ControllerID] += loserPoints;
                }
                else
                {
                    if (i == 1 || i == 3)
                        playersScore[gameData.CharacterInfos[i].ControllerID] += winnerPoints;
                    else
                        playersScore[gameData.CharacterInfos[i].ControllerID] += loserPoints;
                }

                if (playersScore[gameData.CharacterInfos[i].ControllerID] < 0)
                    playersScore[gameData.CharacterInfos[i].ControllerID] = 0;
            }
        }
        else
        {
            List<CharacterBase> listCharacter = new List<CharacterBase>(BattleManager.Instance.characterFullDead);

            for (int i = 0; i < gameData.CharacterInfos.Count; i++)
            {
                playersScore[listCharacter[i].ControllerID] += scoreArr[i];

                if (playersScore[listCharacter[i].ControllerID] < 0)
                    playersScore[listCharacter[i].ControllerID] = 0;
            }
        }

        canvasScore.DrawScores(playersScore, gameData, currentSpecialRound);
        slamLogoMode.TriggerWheel();
    }

    // Gère toute la transition de la fin du mode en cours au début du prochain mode
    private IEnumerator ManageEndMode()
    {
        Time.timeScale = 0.2f;
        if(gameMode != GameModeStateEnum.Volley_Mode)
            yield return new WaitForSecondsRealtime(2f);
        else
            yield return new WaitForSecondsRealtime(0.8f);
        Time.timeScale = 1.0f;

        currentCam = BattleManager.Instance.cameraController.Camera;
        cameraObj.transform.position = currentCam.transform.position;
        currentCam.enabled = false;

        camSlam.RotToScore();

        while(!camSlam.watchingScore)
        {
            yield return null;
        }

        canvasScore.GetBonusRound(currentSpecialRound);
        canvasScore.ActivePanelScore();

        CalculateScore();

        if (!IsGameOver())
        {
            roundCount++;

            //Bonus Round Checking
            currentSpecialRound = roundCount % specialRoundsOcurrence == 0 ? (SpecialRound)UnityEngine.Random.Range(1, Enum.GetNames(typeof(SpecialRound)).Length) : SpecialRound.NoCurrentSpecialRound;


            nextSceneName = GetRandomSceneFromList();
            yield return new WaitForSeconds(timeOnScore);

            slamLogoMode.DrawLogo(gameMode);

            yield return new WaitForSeconds(1f);

            // DISPLAY "A TO CONTINUE"
            canvasScore.DisplayContinue();

            while (!pressToContinue)
            {
                if (BattleManager.Instance.inputController.playerInputs[0].CheckActionAbsolute(0, InputConst.Attack))
                {
                    BattleManager.Instance.inputController.playerInputs[0].inputActions[0].timeValue = 0;
                    pressToContinue = true;
                }
                yield return null;
            }
            pressToContinue = false;


            BattleManager.Instance.ResetInstance();

            StartCoroutine(UnloadSceneAsync());

            while (!isUnloaded)
            {
                yield return null;
            }
            isUnloaded = false;

            StartCoroutine(LoadSceneAsync());

            while (!isLoaded)
            {
                yield return null;
            }
            isLoaded = false;

            canvasScore.HideContinue();

            while (BattleManager.Instance == null)
            {
                yield return null;
            }

            camSlam.RemoveBackgroundBlur();



            if (currentSpecialRound != SpecialRound.NoCurrentSpecialRound)
            {
                canvasScore.DisplaySpecialRules(currentSpecialRound);

                bonusPanel.animationOver = false;

                while(!bonusPanel.animationOver)
                {
                    yield return null;
                }
            }


            yield return new WaitForSeconds(1f);



            canvasScore.DeactivePanelScore();


            currentCam = BattleManager.Instance.cameraController.Camera;
            moveCamera = true;

            /*
            cameraObj.transform.position = currentCam.transform.position;
            */

            canvasScore.StartTransitionLogo(gameMode);

            yield return new WaitForSeconds(1.5f);

            camSlam.RotToGame();

            while (!camSlam.watchingGame)
            {
                yield return null;
            }

            SetGame();
        }
        else
        {
            ManageEndSlam();
        }
    }


    // Paramètre le gameData
    private void SetGame()
    {
        gameData.SetSkipIntro(GameModeStateEnum.Special_Mode, true);

        StartGame();
    }

    // Récupère quelques infos du Battlemanager et le démarre
    private void StartGame()
    {
        currentCam = null;
        currentCam = BattleManager.Instance.cameraController.Camera;
        cameraObj.transform.position = currentCam.transform.position;
        currentCam.enabled = true;

        BattleManager.Instance.StartBattleManager();

        gameEndedEvent = BattleManager.Instance.gameEndedEvent;
        gameEndedEvent.AddListener(EndGame);

    }

    private void EndGame()
    {
        StartCoroutine(ManageEndMode());
    }


    // Gestion de la fin du mode Grand Slam
    private void ManageEndSlam()
    {
        Debug.Log("BIEN JOUÉ C'EST LA FIN");

        List<int> sortedControllerID = new List<int>();
        List<int> sortedScores = new List<int>();

        foreach (KeyValuePair<int, int> item in playersScore.OrderBy(key => key.Value))
        {
            sortedControllerID.Add(item.Key);
            sortedScores.Add(item.Value);
        }

        sortedControllerID.Reverse();
        sortedScores.Reverse();

        CharacterBase[] podiumArr = new CharacterBase[4];

        for (int i = 0; i < gameData.CharacterInfos.Count; i++)
        {
            CharacterBase cb = BattleManager.Instance.characterFullDead[i];
            int index = sortedControllerID.IndexOf(cb.ControllerID);
            podiumArr[index] = cb;
        }

        /*
        foreach (CharacterBase cb in BattleManager.Instance.characterFullDead) 
        { 
            int index = sortedControllerID.IndexOf(cb.ControllerID);
            podiumArr[index] = cb;
        } 
        */
        for(int i = 0; i < gameData.CharacterInfos.Count; i++)
        {
            podium.Add(podiumArr[i]);
        }

        camSlam.camera.enabled = false;
        BattleManager.Instance.cameraController.Camera.enabled = false;
        canvasScore.DeactivePanelScore();

        //menuWin.InitializeWin(podium); 
        BattleManager.Instance.MenuWin.InitializeWin(podium);
    }

    private bool IsGameOver()
    {
        foreach(KeyValuePair<int, int> scores in playersScore)
        {
            if (scores.Value >= scoreToWin)
                return true;
        }

        return false;
    }

    private IEnumerator LoadSceneAsync()
    {
        gameData.GameMode = gameMode;

        AsyncOperation async = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        async.completed += (AsyncOperation o) =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextSceneName));
        };

        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            yield return null;
        }
        isLoaded = true;

        BattleManager.Instance.cameraController.Camera.enabled = false;

        if (firstRound)
        {
            firstRound = false;
            SetGame();
        }
    }

    private IEnumerator UnloadSceneAsync()
    {
        AsyncOperation async = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!async.isDone)
        {
            yield return null;
        }
        isUnloaded = true;
    }

    private void OnDestroy()
    {
        gameData.slamMode = false;
    }

}
