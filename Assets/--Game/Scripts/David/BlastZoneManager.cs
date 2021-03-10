using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZoneManager : MonoBehaviour
{
    private static BlastZoneManager _instance;

    public static BlastZoneManager Instance { get { return _instance; } }

    public float timeBeforeRespawn = 3.0f;

    private CharacterBase playerCB;

    public Transform spawnpoint;

    //Float Event to update Stock UI
    public PackageCreator.Event.GameEventFloat[] gameEventStocks;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log(other.gameObject.tag);

        string tag = other.gameObject.tag;

        if(other.transform.root.gameObject.GetComponent<CharacterBase>() != null)
        {
            float stocks = playerCB.Stats.LifeStocks;
            if (stocks - 1 >= 0)
            {
                // Respawn Manager
                playerCB = other.transform.root.gameObject.GetComponent<CharacterBase>();
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                playerCB.Stats.RespawnStats();


                //Float Event to update Stock UI
                if (tag == "Player1")
                    gameEventStocks[0].Raise(stocks);
                else if (tag == "Player2")
                    gameEventStocks[1].Raise(stocks);
                else if (tag == "Player3")
                    gameEventStocks[2].Raise(stocks);
                else if (tag == "Player4")
                    gameEventStocks[3].Raise(stocks);
            }
            else
            {
                // Handle Definitive death
            }
        }
    }

}
