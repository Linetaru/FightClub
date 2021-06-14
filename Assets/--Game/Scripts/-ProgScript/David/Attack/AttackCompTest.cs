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

    public override void StartComponent(CharacterBase user)
    {

    }
    public override void UpdateComponent(CharacterBase user)
    {

    }
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        Debug.Log("Aie !");
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
