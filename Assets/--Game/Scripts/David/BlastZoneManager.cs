using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZoneManager : MonoBehaviour
{
    private static BlastZoneManager _instance;

    public static BlastZoneManager Instance { get { return _instance; } }

    public float timeBeforeRespawn = 3.0f;

    private GameObject playerGO;

    public Transform spawnpoint;

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
        if(other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Player3") || other.CompareTag("Player4"))
        {
            // Respawn Manager
            playerGO = other.transform.root.gameObject;
            playerGO.GetComponent<CharacterBase>().SetState(playerGO.GetComponentInChildren<CharacterStateDeath>());
            playerGO.GetComponent<CharacterBase>().Stats.RespawnStats();
        }
    }
}
