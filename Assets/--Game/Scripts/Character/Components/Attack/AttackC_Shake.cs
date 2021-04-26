using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_Shake : AttackComponent
{
    [SerializeField]
    float shakePower = 0.1f;
    [SerializeField]
    float shakeTime = 0.1f;

    public override void StartComponent(CharacterBase user)
    {

    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        target.Knockback.ShakeEffect.Shake(shakePower, shakeTime);
        target.Model.FlashModel(new Color(1, 1, 1, 0.4f), shakeTime);
    }

    public override void EndComponent(CharacterBase user)
    {

    }
}
