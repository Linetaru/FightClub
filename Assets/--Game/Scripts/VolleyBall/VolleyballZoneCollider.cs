using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyballZoneCollider : MonoBehaviour
{
    public PackageCreator.Event.GameEvent eventScorePoint;

    [SerializeField]
    bool isRedTeam;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            eventScorePoint.Raise();
        }
    }
}
