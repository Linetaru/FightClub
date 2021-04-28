using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Damage : AttackComponent
{
    [Title("Damage Percent")]
    public float percentDamage;

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
        user.PowerGauge.AddPower(user.PowerGauge.powerGivenOnAttack);
        target.PowerGauge.AddPower(user.PowerGauge.powerGivenToHitTarget);
    }

    public override void EndComponent(CharacterBase user)
    {
		
    }
}
