using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_Knockback : AttackComponent
{

    [SerializeField]
    float hitStop = 0.1f;

    public override void StartComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        user.SetMotionSpeed(0, hitStop);
        target.SetMotionSpeed(0, hitStop);
    }

    public override void EndComponent(CharacterBase user)
    {

    }
}
