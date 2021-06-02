using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class GrandSlamManager : MonoBehaviour
{
    [Expandable]
    public GameData gameData;

    [SerializeField]
    List<SlamMode> listGameModesValid;

    [Button]
    public void UpdateGameModeList()
    {
        listGameModesValid = new List<SlamMode>(GetComponentsInChildren<SlamMode>());
    }

    GameModeStateEnum gameMode;

    [SerializeField]
    private GameObject cameraObj;
    [SerializeField]
    private CameraSlam camSlam;

    [SerializeField]
    private Camera currentCam;

    [SerializeField]
    private GrandSlamUi canvasScore;

    /*
    [SerializeField]
    List<string> scenesClassic = new List<string>();
    [SerializeField]
    List<string> scenesBomb = new List<string>();
    [SerializeField]
    List<string> scenesFlappy = new List<string>();
    [SerializeField]
    List<string> scenesVolley = new List<string>();
    */

    List<string> listToPickFrom = new List<string>();

    bool firstRound = true;

    bool isUnloaded;
    bool isLoaded;

    bool moveCamera = false;
    [SerializeField]
    private float cameraResetSpeed = 1.0f;

    UnityEvent gameEndedEvent;


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

    void Start()
    {
        gameData.slamMode = true;
        AdjustModeList();
        StartCoroutine(LoadSceneAsync());
    }

    string GetRandomSceneFromList()
    {
        listToPickFrom = GetRandomModeList();
        string sceneName = listToPickFrom[Random.Range(0, listToPickFrom.Count)];
        Debug.Log("Next scene to load = " + sceneName);

        return sceneName;
    }

    void GoToScore()
    {
        // Camera rotate and draw score

        // Une fois la cam set sur le score
    }


    // Cette fonction sélectionne le mode et retourne la liste de scènes associée
    List<string> GetRandomModeList()
    {
        int randomKey = Random.Range(0, listGameModesValid.Count);

        gameMode = listGameModesValid[randomKey].gameMode;

        List<string> scenes = new List<string>(listGameModesValid[randomKey].scenes);

        listGameModesValid.RemoveAt(randomKey);

        return scenes;
    }


    // Cette fonction retire des modes de la liste en fonction du nombre de joueurs
    public void AdjustModeList()
    {
        List<SlamMode> copySlamMode = new List<SlamMode>(listGameModesValid);

        foreach(SlamMode slam in copySlamMode)
        {
            if (gameData.CharacterInfos.Count == 3 || slam.gameMode == GameModeStateEnum.Volley_Mode) // TMP CONDITION CAR VOLLEY PAS DISPO ::: A CHANGER AVEC && 
                listGameModesValid.Remove(slam);
            else if (gameData.CharacterInfos.Count == 2 && slam.gameMode == GameModeStateEnum.Bomb_Mode)
                listGameModesValid.Remove(slam);
        }
    }


    IEnumerator ManageEndMode()
    {
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

        BattleManager.Instance.ResetInstance();
        yield return new WaitForSeconds(4f);

        StartCoroutine(UnloadSceneAsync());

        while(!isUnloaded)
        {
            yield return null;
        }
        isUnloaded = false;

        if(!IsGameOver())
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

    void SetGame()
    {
        gameData.GameMode = gameMode;

        StartGame();
    }

    void StartGame()
    {
        currentCam = null;
        currentCam = BattleManager.Instance.cameraController.Camera;
        cameraObj.transform.position = currentCam.transform.position;
        currentCam.enabled = true;

        BattleManager.Instance.StartBattleManager();

        gameEndedEvent = BattleManager.Instance.gameEndedEvent;
        gameEndedEvent.AddListener(EndGame);

    }

    void EndGame()
    {
        StartCoroutine(ManageEndMode());
    }


    // Gestion de la fin du mode Grand Slam
    void ManageEndSlam()
    {

    }

    bool IsGameOver()
    {
        if (listGameModesValid.Count > 0)
            return false;
        return true;
    }

    IEnumerator LoadSceneAsync()
    {
        string sceneName = GetRandomSceneFromList();

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        async.completed += (AsyncOperation o) =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
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
    IEnumerator UnloadSceneAsync()
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
