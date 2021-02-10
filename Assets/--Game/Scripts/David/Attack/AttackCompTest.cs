using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCompTest : AttackComponent
{
    void Start()
    {

    }

    void Update()
    {

    }

    public override void OnHit(CharacterKnockback target)
    {
        Debug.Log("Aie !");
    }
}
