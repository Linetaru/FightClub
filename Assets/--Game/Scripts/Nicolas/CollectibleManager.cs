using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Boost_Transition_State{
    OnStart,
    OnLife,
    OnWaitingToSpawn,
    OnTriggered,
}

[System.Serializable]
public class Boost_Config
{
    [Title("Object")]
    [ReadOnly] public CollectibleObject currentSpawnedBoost;
    public GameObject prefabBoostObject;

    [Title("State")]
    [ReadOnly] public Boost_Transition_State state = Boost_Transition_State.OnStart;

    [Title("Parameter")]
    public float timeOnStartBeforeSpawn;
    [HideInInspector] public float timerOnStart;
    public float lifeTime;
    [HideInInspector] public float timerLife;
    public float timeBeforeAnotherSpawn;
    [HideInInspector] public float timerSpawn;

    public bool IsBoostNull()
    {
        if (currentSpawnedBoost == null)
            return true;
        else
            return false;
    }
}

public class CollectibleManager : MonoBehaviour
{
    public CameraManager cameraManager;

    [Title("Boost Object")]
    public Boost_Config[] boost_Configs;

    //[Title("Parameter")]
    private int isAlreadyReset;

    // Update is called once per frame
    private void Update()
    {
        for(int i = 0; i < boost_Configs.Length; i++)
        {
            if (cameraManager.stateCamera == StateCamera.InMovingMode && isAlreadyReset == boost_Configs.Length)
            {
                ResetBoostToNextConfig(i);
            }
            else if(cameraManager.stateCamera == StateCamera.InFocusMode && isAlreadyReset == boost_Configs.Length)
            {
                isAlreadyReset = 0;
            }

            var b = boost_Configs[i];

            if(isAlreadyReset != 0) { return; }

            switch (b.state)
            {
                case Boost_Transition_State.OnStart:
                    if (b.timerOnStart < b.timeOnStartBeforeSpawn)
                        b.timerOnStart += Time.deltaTime;
                    else if(b.timerOnStart >= b.timeOnStartBeforeSpawn)
                    {
                        SpawnNewCollectible(i);
                        b.timerOnStart = 0;
                        b.state = Boost_Transition_State.OnLife;
                    }
                    break;

                case Boost_Transition_State.OnLife:

                    if (b.IsBoostNull())
                    {
                        b.timerLife = 0;
                        b.state = Boost_Transition_State.OnTriggered;
                        break;
                    }

                    if(b.timerLife < b.lifeTime)
                        b.timerLife += Time.deltaTime;
                    else if(b.timerLife >= b.lifeTime)
                    {
                        Destroy(b.currentSpawnedBoost.gameObject);
                        b.timerLife = 0;
                        b.state = Boost_Transition_State.OnWaitingToSpawn;
                    }
                    break;

                case Boost_Transition_State.OnWaitingToSpawn:

                    if(b.timerSpawn < b.timeBeforeAnotherSpawn)
                        b.timerSpawn += Time.deltaTime;
                    else if (b.timerSpawn >= b.timeBeforeAnotherSpawn)
                    {
                        SpawnNewCollectible(i);
                        b.state = Boost_Transition_State.OnLife;
                        b.timerSpawn = 0;
                    }
                    break;

                case Boost_Transition_State.OnTriggered:
                    b.state = Boost_Transition_State.OnWaitingToSpawn;
                    break;
            }
        }
    }

    public void SpawnNewCollectible(int ID)
    {
       Bounds platformBounds = cameraManager.cam_Infos[cameraManager.positionID].platform[Random.Range(0, cameraManager.cam_Infos[cameraManager.positionID].platform.Count)].GetComponent<BoxCollider>().bounds;
       boost_Configs[ID].currentSpawnedBoost = Instantiate(boost_Configs[ID].prefabBoostObject, new Vector3(Random.Range(platformBounds.min.x, platformBounds.max.x), platformBounds.max.y + 0.5f, platformBounds.min.z + 1f), Quaternion.identity).GetComponent<CollectibleObject>();
    }

    public void ResetBoostToNextConfig(int ID)
    {
        var b = boost_Configs[ID];

        b.timerOnStart = 0;
        b.timerLife = 0;
        b.timerSpawn = 0;
        b.state = Boost_Transition_State.OnStart;

        isAlreadyReset++;
    }
}