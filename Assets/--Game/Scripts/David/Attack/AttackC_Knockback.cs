using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_Knockback : AttackComponent
{
    public override void StartComponent(CharacterBase user)
    {

    }
    public override void OnHit(CharacterKnockback target)
    {
        Debug.Log(target.gameObject.name);
    }
    public override void EndComponent(CharacterBase user)
    {

    }
}
