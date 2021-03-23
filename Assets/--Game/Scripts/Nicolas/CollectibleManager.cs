using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//State Machine for each boost
public enum Boost_Transition_State{
    OnStart,
    OnLife,
    OnWaitingToSpawn,
    OnTriggered,
}

//Class to add new boost spawnable on level
[System.Serializable]
public class Boost_Config
{
    //Current spawned Boost reference and Prefab boost reference
    [Title("Object")]
    [ReadOnly] public CollectibleObject currentSpawnedBoost;
    public GameObject prefabBoostObject;

    //Current State of this boost
    [Title("State")]
    [ReadOnly] public Boost_Transition_State state = Boost_Transition_State.OnStart;

    //All timer for each state
    [Title("Parameter")]
    public float timeOnStartBeforeSpawn;
    [HideInInspector] public float timerOnStart;
    public float lifeTime;
    [HideInInspector] public float timerLife;
    public float timeBeforeAnotherSpawn;
    [HideInInspector] public float timerSpawn;

    //Function to check if current spawned boost as reference
    public bool IsBoostNull()
    {
        if (currentSpawnedBoost == null)
            return true;
        else
            return false;
    }
}

//Collectible manager to spawn and manage boost
public class CollectibleManager : MonoBehaviour
{
    //Reference at cameraManager for access to each config
    public CameraManager cameraManager;

    //List of boost
    [Title("Boost Object")]
    public Boost_Config[] boost_Configs;

    //Debug Parameter
    //[Title("Parameter")]
    private int isAlreadyReset;

    // Update is called once per frame
    private void Update()
    {
        //Update for each boost in array
        for(int i = 0; i < boost_Configs.Length; i++)
        {
            //Reset Method on scrolling mode to spawn new boost on next config
            if (cameraManager.stateCamera == StateCamera.InMovingMode && isAlreadyReset == boost_Configs.Length)
            {
                ResetBoostToNextConfig(i);
            }
            //If Reset is done, restart value for active boost spawn
            else if(cameraManager.stateCamera == StateCamera.InFocusMode && isAlreadyReset == boost_Configs.Length)
            {
                isAlreadyReset = 0;
            }

            var b = boost_Configs[i];

            //If reset is not done return to not starting spawn
            if(isAlreadyReset != 0) { return; }

            switch (b.state)
            {
                //Spawn a boost in Start after a time
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

                //Manage life time of boost to be destroyed if no one take it, change state if a player take before life time is over
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

                //Timer to respawn boost after a boost taken or destroyed in time interval
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

                //Change State when we got triggered by a player
                case Boost_Transition_State.OnTriggered:
                    b.state = Boost_Transition_State.OnWaitingToSpawn;
                    break;
            }
        }
    }

    //Spawn New Boost for each config in array, Take a random platform in level in current config to fight, and instantiate randomly on this platform the prefab boost
    public void SpawnNewCollectible(int ID)
    {
        //Stock bounds of a random platform for current stage config to be used to intantiate randomly on the top of this platform a boost
        Bounds platformBounds = cameraManager.cam_Infos[cameraManager.positionID].platform[Random.Range(0, cameraManager.cam_Infos[cameraManager.positionID].platform.Count)].GetComponent<BoxCollider>().bounds;
        boost_Configs[ID].currentSpawnedBoost = Instantiate(boost_Configs[ID].prefabBoostObject, new Vector3(Random.Range(platformBounds.min.x, platformBounds.max.x), platformBounds.max.y + 0.5f, platformBounds.min.z + 1f), Quaternion.identity).GetComponent<CollectibleObject>();
    }

    //Reset all timer and state when scrolling start
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