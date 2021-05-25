using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_FlashUpdate : AttackComponent
{
    [SerializeField]
    Color colorFlash;
    [SerializeField]
    float flashInterval = 0.1f;

    float t = 0f;

    public override void StartComponent(CharacterBase user)
    {
        t = 0f;
    }

    public override void UpdateComponent(CharacterBase user)
    {
        t += Time.deltaTime;
        if(t>flashInterval)
        {
            user.Model.FlashModel(colorFlash, flashInterval);
            t = 0f;
        }
    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        //target.Knockback.ShakeEffect.Shake(shakePower, shakeTime);
    }

    public override void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {

    }
    public override void OnClash(CharacterBase user, CharacterBase target)
    {

    }
    public override void EndComponent(CharacterBase user)
    {

    }
}
