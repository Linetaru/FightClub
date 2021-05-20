using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrandSlamManager : MonoBehaviour
{
    GameModeStateEnum gameMode;

    [SerializeField]
    private Camera camSlam;
    
    [SerializeField]
    List<string> scenesBomb = new List<string>();
    [SerializeField]
    List<string> scenesFlappy = new List<string>();

    List<string> listToPickFrom = new List<string>();

    bool isUnloaded;

    private void Awake()
    {
    }

    private void Update()
    {

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
        yield return new WaitForSeconds(5f);


        StartCoroutine(UnloadSceneAsync());

        while(!isUnloaded)
        {
            yield return null;
        }
        isUnloaded = false;

        yield return new WaitForSeconds(0f);

        StartCoroutine(LoadSceneAsync());

    }

    void SetGame()
    {
        BattleManager.Instance.gameData.GameMode = gameMode;
        StartGame();
    }

    void StartGame()
    {
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

        SetGame();
        
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


}
