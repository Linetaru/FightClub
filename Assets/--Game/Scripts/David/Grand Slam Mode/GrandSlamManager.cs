using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GrandSlamManager : MonoBehaviour
{
    [Title("Data")]
    [SerializeField]
    private int scoreToWin = 1500;
    [SerializeField]
    private float timeOnScore = 6f;


    [Title("GameData")]
    [Expandable]
    public GameData gameData;

    [Title("Lists")]
    [SerializeField]
    List<SlamMode> listGameModesValid;
    List<string> listToPickFrom = new List<string>();

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
        InitScoreDictionary();
        AdjustModeList();
        StartCoroutine(LoadSceneAsync());
    }

    private string GetRandomSceneFromList()
    {
        listToPickFrom = GetRandomModeList();
        string sceneName = listToPickFrom[Random.Range(0, listToPickFrom.Count)];
        Debug.Log("Next scene to load = " + sceneName);

        return sceneName;
    }


    // Cette fonction sélectionne le mode et retourne la liste de scènes associée
    private List<string> GetRandomModeList()
    {
        int randomKey = Random.Range(0, listGameModesValid.Count);

        gameMode = listGameModesValid[randomKey].gameMode;

        gameData.NumberOfLifes = listGameModesValid[randomKey].nbLife;

        currentScoreArr = listGameModesValid[randomKey].scoreArr;

        return listGameModesValid[randomKey].scenes;
    }


    // Cette fonction retire des modes de la liste en fonction du nombre de joueurs
    private void AdjustModeList()
    {
        List<SlamMode> copySlamMode = new List<SlamMode>(listGameModesValid);

        foreach(SlamMode slam in copySlamMode)
        {
            if (gameData.CharacterInfos.Count == 3 && slam.gameMode == GameModeStateEnum.Volley_Mode)
                listGameModesValid.Remove(slam);
            
            if (gameData.CharacterInfos.Count == 2 && slam.gameMode == GameModeStateEnum.Bomb_Mode)
                listGameModesValid.Remove(slam);
        }
    }


    /// Cette fonction retire le mode actuel de la list des modes valides
    private void RemoveCurrentGameModeFromList()
    {
        List<SlamMode> copySlamMode = new List<SlamMode>(listGameModesValid);

        foreach (SlamMode slam in copySlamMode)
        {
            if(slam.gameMode == gameMode)
                listGameModesValid.Remove(slam);
        }
    }
    private void InitScoreDictionary()
    {
        foreach(Character_Info character in gameData.CharacterInfos)
        {
            playersScore.Add(character.ControllerID, 0);
        }
    }

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

        canvasScore.DrawScores(scores, i);
        slamLogoMode.TriggerWheel();

    }


    private IEnumerator ManageEndMode()
    {
        //RemoveCurrentGameModeFromList();

        // Récup aléatoirement scène parmi gameMode aléatoire
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

        if (!IsGameOver())
        {
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

    private void SetGame()
    {
        gameData.GameMode = gameMode;

        StartGame();
    }

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

    }

    private bool IsGameOver()
    {
        if (listGameModesValid.Count > 0)
            return false;
        return true;
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
