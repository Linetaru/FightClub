using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_GaugeConsume : AttackComponent
{
    public int consume = 20;

    public override void StartComponent(CharacterBase user)
    {
        user.PowerGauge.ForceAddPower(-consume);
    }

    public override void UpdateComponent(CharacterBase user)
    {

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
