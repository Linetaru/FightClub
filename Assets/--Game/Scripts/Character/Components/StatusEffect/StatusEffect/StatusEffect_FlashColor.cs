using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_FlashColor : StatusEffect
{

    [SerializeField]
    Color colorFlash = Color.green; 
    float t = 0f;


    public StatusEffect_FlashColor()
    {

    }
    public StatusEffect_FlashColor(Color c)
    {
        colorFlash = c;
    }
    public override StatusEffect Copy()
    {
        return new StatusEffect_FlashColor(colorFlash);
    }


    public override void UpdateEffect(CharacterBase character)
    {
        t -= Time.deltaTime * character.MotionSpeed;
        if (t <= 0)
        {
            character.Model.FlashModel(colorFlash, 0.2f);
            t = 0.3f;
        }
    }
}
