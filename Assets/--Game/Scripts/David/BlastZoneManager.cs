using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZoneManager : MonoBehaviour
{
    public float timeBeforeRespawn = 3.0f;

    private CameraController cameraController;

    private GameObject playerGO;

    public Transform spawnpoint;

    private void Start()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Player3") || other.CompareTag("Player4"))
        {
            // Respawn Manager
            playerGO = other.transform.root.gameObject;
            playerGO.GetComponent<CharacterBase>().SetState(playerGO.GetComponentInChildren<CharacterStateDeath>());
        }
    }
}
