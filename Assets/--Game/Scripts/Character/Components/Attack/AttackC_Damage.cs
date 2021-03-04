using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Damage : AttackComponent
{
    public float percentDamage;

    public override void StartComponent(CharacterBase user)
    {
		
    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        target.Stats.TakeDamage(percentDamage);
    }

    public override void EndComponent(CharacterBase user)
    {
		
    }
}
