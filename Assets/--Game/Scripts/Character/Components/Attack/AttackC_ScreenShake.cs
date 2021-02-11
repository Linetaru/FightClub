using System.Collections;
using System.Collections.Generic;
using Feedbacks;
using UnityEngine;

public class AttackC_ScreenShake : AttackComponent
{
    [SerializeField]
    float shakePower = 0.7f;
    [SerializeField]
    float shakeDuration = 0.2f;

    public override void StartComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        Camera.main.GetComponent<ScreenShake>().StartScreenShake(shakePower, shakeDuration);
    }

    public override void EndComponent(CharacterBase user)
    {

    }
}
