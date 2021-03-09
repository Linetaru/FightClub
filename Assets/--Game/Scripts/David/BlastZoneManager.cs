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
    public PackageCreator.Event.GameEventFloat gameEventStocksP1;
    public PackageCreator.Event.GameEventFloat gameEventStocksP2;
    public PackageCreator.Event.GameEventFloat gameEventStocksP3;
    public PackageCreator.Event.GameEventFloat gameEventStocksP4;

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
            // Respawn Manager
            playerCB = other.transform.root.gameObject.GetComponent<CharacterBase>();
            playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
            playerCB.Stats.RespawnStats();


            //Float Event to update Stock UI
            if (tag == "Player1")
                gameEventStocksP1.Raise(playerCB.Stats.LifeStocks);
            else if (tag == "Player2")
                gameEventStocksP2.Raise(playerCB.Stats.LifeStocks);
            else if (tag == "Player3")
                gameEventStocksP3.Raise(playerCB.Stats.LifeStocks);
            else if (tag == "Player4")
                gameEventStocksP4.Raise(playerCB.Stats.LifeStocks);
        }
    }

}
