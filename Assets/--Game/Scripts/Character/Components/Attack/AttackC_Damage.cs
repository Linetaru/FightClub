using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Damage : AttackComponent
{
    [Title("Damage Percent")]
    public float percentDamage;
    public float percentDamageOnGuard;
    public override void StartComponent(CharacterBase user)
    {
		
    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        float damage = (percentDamage * user.Stats.AttackMultiplier.Value) * target.Stats.DefenseMultiplier.Value;
        target.Stats.TakeDamage(damage);
        /*user.PowerGauge.AddPower(user.PowerGauge.powerGivenOnAttack);
        target.PowerGauge.AddPower(user.PowerGauge.powerGivenToHitTarget);*/
    }

    public override void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {
        if(guardRepel == true)
        {
            float damage = (percentDamageOnGuard * user.Stats.AttackMultiplier.Value) * target.Stats.DefenseMultiplier.Value;
            target.Stats.TakeDamage(damage);
        }
    }
    public override void OnClash(CharacterBase user, CharacterBase target)
    {

    }
    public override void EndComponent(CharacterBase user)
    {
		
    }
}
