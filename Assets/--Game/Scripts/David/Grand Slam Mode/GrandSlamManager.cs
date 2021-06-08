using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GrandSlamManager : MonoBehaviour
{
    [Title("Data")]
    [SerializeField]
    private int scoreGoal2Players = 800;
    [SerializeField]
    private int scoreGoal3Players = 1200;
    [SerializeField]
    private int scoreGoal4Players = 1800;

    public int scoreToWin = 1500;
    [SerializeField]
    private float timeOnScore = 6f;


    [Title("GameData")]
    [Expandable]
    public GameData gameData;

    [Title("Lists")]
    [SerializeField]
    List<SlamMode> listGameModesValid = new List<SlamMode>();
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

    private Camera currentCam;


    bool firstRound = true;

    bool isUnloaded;
    bool isLoaded;

    bool moveCamera = false;

    [Title("Values")]
    [SerializeField]
    private float cameraResetSpeed = 1.0f;

    UnityEvent gameEndedEvent;


    private string nextSceneName;


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
        nextSceneName = GetRandomSceneFromList();
        gameData.slamMode = true;

        InitScoreGoal();
        InitScoreDictionary();
        AdjustModeList();
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
    }

    // Récupère une scène parmi la liste du mode choisi
    private string GetRandomSceneFromList()
    {
        listToPickFrom = GetRandomModeList();
        string sceneName = listToPickFrom[Random.Range(0, listToPickFrom.Count)];
        Debug.Log("Next scene to load = " + sceneName);

        return sceneName;
    }


    // Cette fonction sélectionne le mode et retourne la liste de scènes associée
    // Retire le mode en cours de la liste pour le tirage
    private List<string> GetRandomModeList()
    {
        if(currentMode != null)
            listGameModesValid.Remove(currentMode);

        int randomKey = Random.Range(0, listGameModesValid.Count);
        gameMode = listGameModesValid[randomKey].gameMode;
        gameData.NumberOfLifes = listGameModesValid[randomKey].nbLife;
        currentScoreArr = listGameModesValid[randomKey].scoreArr;

        if(currentMode != null)
            listGameModesValid.Add(currentMode);

        currentMode = listGameModesValid[randomKey];

        return listGameModesValid[randomKey].scenes;
    }


    // Cette fonction retire des modes de la liste en fonction du nombre de joueurs
    private void AdjustModeList()
    {
        List<SlamMode> copySlamMode = new List<SlamMode>(listGameModesValid);

        foreach (SlamMode slam in copySlamMode)
        {
            if (gameData.CharacterInfos.Count == 3 && slam.gameMode == GameModeStateEnum.Volley_Mode)
            {
                Debug.Log("JE RETIRE " + slam.gameMode);
                listGameModesValid.Remove(slam);
            }

            if (gameData.CharacterInfos.Count == 2 && slam.gameMode == GameModeStateEnum.Bomb_Mode)
            {
                Debug.Log("JE RETIRE " + slam.gameMode);
                listGameModesValid.Remove(slam);
            }
        }

        Debug.Log("LISTE MODES VALIDES : ");

        foreach(SlamMode slam in listGameModesValid)
        {
            Debug.Log(slam.gameMode);
        }
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
        int[] scores = new int[4];

        int i = 0;

        foreach (CharacterBase character in BattleManager.Instance.characterFullDead)
        {
            playersScore[character.ControllerID] += currentScoreArr[i];

            scores[character.PlayerID] = playersScore[character.ControllerID];

            i++;
        }

        canvasScore.DrawScores(scores, gameData.CharacterInfos.Count);
        slamLogoMode.TriggerWheel();

    }

    // Gère toute la transition de la fin du mode en cours au début du prochain mode
    private IEnumerator ManageEndMode()
    {
        nextSceneName = GetRandomSceneFromList();

        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1.0f;

        currentCam = BattleManager.Instance.cameraController.Camera;
        cameraObj.transform.position = currentCam.transform.position;
        currentCam.enabled = false;

        camSlam.RotToScore();

        while(!camSlam.watchingScore)
        {
            yield return null;
        }

        canvasScore.ActivePanelScore();

        CalculateScore();

        if (!IsGameOver())
        {
            BattleManager.Instance.ResetInstance();
            yield return new WaitForSeconds(timeOnScore);

            slamLogoMode.DrawLogo(gameMode);

            yield return new WaitForSeconds(2f);

            StartCoroutine(UnloadSceneAsync());

            while(!isUnloaded)
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

            camSlam.RemoveBackgroundBlur();

            yield return new WaitForSeconds(2f);

            canvasScore.DeactivePanelScore();

            currentCam = BattleManager.Instance.cameraController.Camera;
            moveCamera = true;
            /*
            cameraObj.transform.position = currentCam.transform.position;
            */

            canvasScore.StartTransitionLogo(gameMode);

            yield return new WaitForSeconds(2f);

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
        gameData.GameMode = gameMode;

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
