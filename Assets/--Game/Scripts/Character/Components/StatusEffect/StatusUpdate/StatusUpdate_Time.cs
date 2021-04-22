using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpdate_Time : StatusUpdate
{
    [SerializeField]
    float time = 3;

    float t = 0f;

    public StatusUpdate_Time()
    {
        t = 0f;
    }

    public StatusUpdate_Time(float time)
    {
        t = 0f;
        this.time = time;
    }

    public override StatusUpdate Copy()
    {
        return new StatusUpdate_Time();
    }


    public override bool CanRemoveStatus(CharacterBase character)
    {
        time -= Time.deltaTime * character.MotionSpeed;
        if (time <= 0)
            return true;
        return false;
    }
}
