using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_FlashColor : StatusEffect
{

    [SerializeField]
    Color colorFlash = Color.green;
    [SerializeField]
    float time = 0.2f;
    [SerializeField]
    float interval = 0.3f;

    float t = 0f;


    public StatusEffect_FlashColor()
    {

    }
    public StatusEffect_FlashColor(Color c, float timeFlash, float timeInterval)
    {
        colorFlash = c;
        time = timeFlash;
        interval = timeInterval;
    }
    public override StatusEffect Copy()
    {
        return new StatusEffect_FlashColor(colorFlash, time, interval);
    }


    public override void UpdateEffect(CharacterBase character)
    {
        t -= Time.deltaTime * character.MotionSpeed;
        if (t <= 0)
        {
            character.Model.FlashModel(colorFlash, time);
            t = interval;
        }
    }
}
