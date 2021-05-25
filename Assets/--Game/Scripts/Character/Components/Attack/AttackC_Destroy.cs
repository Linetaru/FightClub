using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackC_Destroy : AttackComponent
{
    [SerializeField]
    List<GameObject> objToDestroy;

    public override void StartComponent(CharacterBase user)
    {

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
        for (int i = 0; i < objToDestroy.Count; i++)
        {
            Destroy(objToDestroy[i]);
        }
    }
}
