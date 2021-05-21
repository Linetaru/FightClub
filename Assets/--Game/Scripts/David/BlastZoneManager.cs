using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlastZoneManager : MonoBehaviour
{
    private static BlastZoneManager _instance;
    public static BlastZoneManager Instance { get { return _instance; } }

    public float timeBeforeRespawn = 3.0f;

    [SerializeField]
    private GameObject deathVFXPrefab;
    [SerializeField]
    private BoxCollider boxCollider;

    //private ParticleSystem deathVFX;

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
            //deathVFX = deathVFXPrefab.GetComponentInChildren<ParticleSystem>();
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
                playerCB.transform.position = spawnpoint.transform.position;


            }
            else
            {
                playerCB.Stats.Death = true;
                playerCB.SetState(playerCB.GetComponentInChildren<CharacterStateDeath>());
                gameEventCharacterFullDead.Raise(playerCB);
            }
    
            //Float Event to update Stock UI
            if (playerCB.tag == "Player1")
                gameEventStocks[0].Raise(playerCB);
            else if (playerCB.tag == "Player2")
                gameEventStocks[1].Raise(playerCB);
            else if (playerCB.tag == "Player3")
                gameEventStocks[2].Raise(playerCB);
            else if (playerCB.tag == "Player4")
                gameEventStocks[3].Raise(playerCB);
        }
    }

    public void ExplosionDeath(Collider other)
    {
        GameObject go = Instantiate(deathVFXPrefab, other.transform.position, Quaternion.identity);
        float angleZ = Mathf.Atan2(transform.position.y - go.transform.position.y, transform.position.x - go.transform.position.x) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, angleZ);
        Destroy(go, 3f);
    }

    private void OnDrawGizmos()
    {
        if(boxCollider != null)
        {
            Gizmos.DrawWireCube(boxCollider.center + this.transform.position, boxCollider.size);
        }
    }

}
