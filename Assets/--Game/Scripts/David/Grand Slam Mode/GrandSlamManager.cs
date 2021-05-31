﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrandSlamManager : MonoBehaviour
{
    GameModeStateEnum gameMode;

    [SerializeField]
    private GameObject cameraObj;
    [SerializeField]
    private CameraSlam camSlam;

    [SerializeField]
    private Camera currentCam;
    
    [SerializeField]
    List<string> scenesBomb = new List<string>();
    [SerializeField]
    List<string> scenesFlappy = new List<string>();

    List<string> listToPickFrom = new List<string>();

    bool firstRound = true;

    bool isUnloaded;
    bool isLoaded;

    bool moveCamera = false;
    [SerializeField]
    private float cameraResetSpeed = 1.0f;


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
        StartCoroutine(LoadSceneAsync());
    }

    string GetRandomSceneFromList(List<string> list)
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

    List<string> GetRandomModeList()
    {
        int test = Random.Range(0, 2);

        if (test == 0 && gameMode != GameModeStateEnum.Bomb_Mode)
        {
            gameMode = GameModeStateEnum.Bomb_Mode;

            return scenesBomb;
        }
        else
        {
            gameMode = GameModeStateEnum.Flappy_Mode;

            return scenesFlappy;
        }
    }

    public void CameraTransitionScore()
    {

    }

    public void CameraTransitionGame()
    {

    }

    IEnumerator ManageEndMode()
    {
        yield return new WaitForSeconds(2f);

        currentCam = BattleManager.Instance.cameraController.Camera;
        cameraObj.transform.position = currentCam.transform.position;
        currentCam.enabled = false;

        camSlam.RotToScore();

        while(!camSlam.watchingScore)
        {
            yield return null;
        }

        BattleManager.Instance.ResetInstance();
        yield return new WaitForSeconds(4f);

        StartCoroutine(UnloadSceneAsync());

        while(!isUnloaded)
        {
            yield return null;
        }
        isUnloaded = false;

        StartCoroutine(LoadSceneAsync());

        while(!isLoaded)
        {
            yield return null;
        }
        isLoaded = false;

        camSlam.RemoveBackgroundBlur();

        yield return new WaitForSeconds(2f);

        currentCam = BattleManager.Instance.cameraController.Camera;
        moveCamera = true;
        /*
        cameraObj.transform.position = currentCam.transform.position;
        */

        yield return new WaitForSeconds(2f);

        camSlam.RotToGame();

        while(!camSlam.watchingGame)
        {
            yield return null;
        }

        SetGame();
    }

    void SetGame()
    {
        BattleManager.Instance.gameData.GameMode = gameMode;
        StartGame();
    }

    void StartGame()
    {
        currentCam = null;
        currentCam = BattleManager.Instance.cameraController.Camera;
        cameraObj.transform.position = currentCam.transform.position;
        currentCam.enabled = true;

        BattleManager.Instance.StartBattleManager();
        StartCoroutine(ManageEndMode());
    }

    IEnumerator LoadSceneAsync()
    {
        string sceneName = GetRandomSceneFromList(scenesBomb);

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

    private void LerpCamera(Camera start, Camera target)
    {

    }
}
