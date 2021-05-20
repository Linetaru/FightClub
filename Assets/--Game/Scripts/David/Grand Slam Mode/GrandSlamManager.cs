using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrandSlamManager : MonoBehaviour
{
    [SerializeField]
    List<string> scenesBomb = new List<string> { "BombModeScene", "BombModeScene1"};

    List<string> listToPickFrom = new List<string>();

    private void Awake()
    {
    }

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    string GetRandomSceneFromList(List<string> list)
    {
        listToPickFrom = GetRandomListToPickFrom();

        string sceneName = listToPickFrom[Random.Range(0, listToPickFrom.Count)];
        Debug.Log("Next scene to load = " + sceneName);

        return sceneName;
    }

    void GoToScore()
    {
        // Camera rotate and draw score

        // Une fois la cam set sur le score
    }

    IEnumerator ManageEndMode()
    {
        yield return null;
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }

    List<string> GetRandomListToPickFrom()
    {
        return scenesBomb;
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
        while(!async.isDone)
        {
            yield return null;
        }
    }


}
