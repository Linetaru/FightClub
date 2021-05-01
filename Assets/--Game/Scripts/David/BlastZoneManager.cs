using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZoneManager : MonoBehaviour
{
    private static BlastZoneManager _instance;
    public static BlastZoneManager Instance { get { return _instance; } }

    public float timeBeforeRespawn = 3.0f;

    [SerializeField]
    private GameObject deathVFXPrefab;
    private ParticleSystem deathVFX;

    private CharacterBase playerCB;

    public Transform spawnpoint;

    //Character Event
    public PackageCreator.Event.GameEventUICharacter[] gameEventStocks;
    //Character Event
    public PackageCreator.Event.GameEventCharacter gameEventCharacterFullDead;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
            deathVFX = deathVFXPrefab.GetComponentInChildren<ParticleSystem>();
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
            ExplosionDeath(other);
            float stocks = playerCB.Stats.LifeStocks;
            if (stocks - 1 > 0)
            {
                // Respawn Manager
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                playerCB.Stats.RespawnStats();

            }
            else
            {
                playerCB.Stats.Death = true;
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                gameEventCharacterFullDead.Raise(playerCB);
            }

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
    }

    public void OutOfCamera(GameObject other)
    {
        string tag = other.tag;
    
        playerCB = other.transform.root.gameObject.GetComponent<CharacterBase>();
    
        if (playerCB != null)
        {
            ExplosionDeath(other.GetComponent<Collider>());
    
            playerCB.Stats.LifeStocks--;
    
            float stocks = playerCB.Stats.LifeStocks;
    
            if (stocks > 0)
            {
                // Respawn Manager
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                playerCB.Stats.RespawnStats();
    
            }
            else
            {
                playerCB.Stats.Death = true;
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                gameEventCharacterFullDead.Raise(playerCB);
            }
    
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
    }
    
    private void ExplosionDeath(Collider other)
    {
        GameObject go = Instantiate(deathVFXPrefab, other.transform.position, Quaternion.identity);

        float angleZ = Mathf.Atan2(transform.position.y - go.transform.position.y, transform.position.x - go.transform.position.x) * Mathf.Rad2Deg;

        go.transform.rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, angleZ);

        Destroy(go, 3f);
    }

}
