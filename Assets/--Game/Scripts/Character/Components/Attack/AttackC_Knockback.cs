using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_Knockback : AttackComponent
{
    [SerializeField]
    float hitStop = 0.1f;
    [SerializeField]
    GameObject particle = null;

    public Vector2 knockbackAngle;

    public override void StartComponent(CharacterBase user)
    {

    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        target.Knockback.Launch(knockbackAngle * new Vector2(user.Movement.Direction, 1));

        user.SetMotionSpeed(0, hitStop);
        target.SetMotionSpeed(0, hitStop);

        if(particle != null)
            Instantiate(particle, target.Knockback.ContactPoint, Quaternion.identity);
    }

    public override void EndComponent(CharacterBase user)
    {

    }
}
