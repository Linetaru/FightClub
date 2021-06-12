using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_ShakeUpdate : AttackComponent
{
    [SerializeField]
    float shakePower = 0.1f;
    [SerializeField]
    float shakeInterval = 0.1f;

    float t = 0f;

    public override void StartComponent(CharacterBase user)
    {
        t = 0f;
    }

    public override void UpdateComponent(CharacterBase user)
    {
        t += Time.deltaTime;
        if (t > shakeInterval)
        {
            user.Knockback.ShakeEffect.Shake(shakePower, shakeInterval);
            t = 0f;
        }
    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
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
