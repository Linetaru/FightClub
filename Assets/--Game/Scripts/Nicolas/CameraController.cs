using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject[] playerTarget;

    public float yOffset = 2.0f;
    public float minDistance = 7.5f;

    private float xMin, xMax, yMin, yMax;

    private void Start()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        playerTarget = new GameObject[allPlayers.Length];
        for(int i = allPlayers.Length; i > 0; i--)
        {
            playerTarget[i - 1] = allPlayers[i - 1];
        }
    }

    private void LateUpdate()
    {
        if(playerTarget.Length == 0) { return;}

        xMin = xMax = playerTarget[0].transform.position.x;
        yMin = yMax = playerTarget[0].transform.position.y;

        for(int i = 1; i < playerTarget.Length; i++)
        {
            if (playerTarget[i].transform.position.x < xMin)
                xMin = playerTarget[i].transform.position.x;

            if (playerTarget[i].transform.position.x > xMax)
                xMax = playerTarget[i].transform.position.x;

            if (playerTarget[i].transform.position.x < yMin)
                yMin = playerTarget[i].transform.position.y;

            if (playerTarget[i].transform.position.x > yMax)
                yMax = playerTarget[i].transform.position.y;
        }

        float xMiddle = (xMin + xMax) / 2;
        float yMiddle = (yMin + yMax) / 2;
        float distance = xMax - xMin;

        if (distance < minDistance)
            distance = minDistance;

        transform.position = new Vector3(xMiddle, yMiddle, -distance);
    }
}