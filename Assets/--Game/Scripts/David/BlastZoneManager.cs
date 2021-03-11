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

    //Character Event
    public PackageCreator.Event.GameEventCharacter[] gameEventStocks;
    //Character Event
    public PackageCreator.Event.GameEventCharacter gameEventCharacterFullDead;

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
        string tag = other.gameObject.tag;

        playerCB = other.transform.root.gameObject.GetComponent<CharacterBase>();

        if (playerCB != null)
        {
            float stocks = playerCB.Stats.LifeStocks;
            if (stocks - 1 >= 0)
            {
                // Respawn Manager
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                playerCB.Stats.RespawnStats();

                stocks = playerCB.Stats.LifeStocks;

                //Float Event to update Stock UI
                if (tag == "Player1")
                    gameEventStocks[0].Raise(playerCB);
                else if (tag == "Player2")
                    gameEventStocks[1].Raise(playerCB);
                else if (tag == "Player3")
                    gameEventStocks[2].Raise(playerCB);
                else if (tag == "Player4")
                    gameEventStocks[3].Raise(playerCB);
            }
            else
            {
                gameEventCharacterFullDead.Raise(playerCB);
            }
        }
    }

}
