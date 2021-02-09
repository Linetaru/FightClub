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

    public override void OnHit()
    {
        Debug.Log("TestHit");
    }
}
