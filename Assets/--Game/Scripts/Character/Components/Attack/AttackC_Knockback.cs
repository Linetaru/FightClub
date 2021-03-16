using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum KnockbackAngleSetting
{
    StaticAngle,
    DynamicAngle
};


public class AttackC_Knockback : AttackComponent
{
    [SerializeField]
    float hitStop = 0.1f;

    [Space]
    [SerializeField]
    float knockbackAngle = 0;

    [SerializeField]
    float knockbackPower = 0;

    // L'angle dynamique signifie que l'angle de trajectoire se fait par rapport aux positions des personnages
    [SerializeField]
    KnockbackAngleSetting dynamicAngle = KnockbackAngleSetting.StaticAngle;





    public override void StartComponent(CharacterBase user)
    {

    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        Vector2 knockbackDirection;
        if (dynamicAngle == KnockbackAngleSetting.DynamicAngle)
        {
            Vector2 targetDirection = (target.transform.position - user.transform.position).normalized;
            float angle = Vector2.SignedAngle(targetDirection, Vector2.right);

            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (knockbackAngle + angle)), Mathf.Sin(Mathf.Deg2Rad * (knockbackAngle + angle)));
            knockbackDirection *= knockbackPower;
        }
        else
        {
            knockbackDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * knockbackAngle), Mathf.Sin(Mathf.Deg2Rad * knockbackAngle));
            knockbackDirection *= knockbackPower;
            knockbackDirection *= new Vector2(user.Movement.Direction, 1);
        }

        target.Knockback.Launch(knockbackDirection, target.Stats.LifePercentage);




        user.SetMotionSpeed(0, hitStop);
        target.SetMotionSpeed(0, hitStop);

        //if(particle != null)
        //    Instantiate(particle, target.Knockback.ContactPoint, Quaternion.identity);
    }

    public override void EndComponent(CharacterBase user)
    {

    }
}
