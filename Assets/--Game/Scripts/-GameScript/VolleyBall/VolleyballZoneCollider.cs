using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyballZoneCollider : MonoBehaviour
{
    public PackageCreator.Event.GameEvent eventScorePoint;

    [SerializeField]
    bool isRedTeam;
    [SerializeField]
    AK.Wwise.Event eventGoal;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            eventScorePoint.Raise();
            AkSoundEngine.PostEvent(eventGoal.Id, this.gameObject);
        }
    }
}
