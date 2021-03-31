using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Particule : AttackComponent
{
    [HorizontalGroup("1")]
    [HideLabel]
    public GameObject particuleObject;
    [HorizontalGroup("1")]
    public float timeBeforeDestroying;


    public override void StartComponent(CharacterBase user)
    {
		
    }

    public override void UpdateComponent(CharacterBase user)
    {

    }

    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        GameObject go = Instantiate(particuleObject, target.Knockback.ContactPoint, Quaternion.identity);
        go.name = particuleObject.name;
        Destroy(go, timeBeforeDestroying);
    }

    public override void EndComponent(CharacterBase user)
    {
		
    }
}
