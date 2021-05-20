using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrandSlamManager : MonoBehaviour
{
    [SerializeField]
    private Camera camSlam;

    [SerializeField]
    List<string> scenesBomb = new List<string>();

    List<string> listToPickFrom = new List<string>();

    bool transition;

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
        return scenesBomb;
    }

    public void CameraTransition()
    {

    }

    IEnumerator ManageEndMode()
    {
        yield return new WaitForSeconds(10f);
        camSlam.enabled = true;
        Camera.main.enabled = false;
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
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
        camSlam.enabled = false;

        StartCoroutine(ManageEndMode());
    }


}
