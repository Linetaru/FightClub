using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_CharaMovement : AttackComponent
{

    [HorizontalGroup("Movement")]
    [SerializeField]
    bool linkToCharacter = true;

    [HorizontalGroup("Movement")]
    [SerializeField]
    bool keepMomentum = false;

    public override void StartComponent(CharacterBase user)
    {
        if (keepMomentum == false)
            user.Movement.SetSpeed(0, 0);
        if (linkToCharacter == true)
            this.transform.SetParent(user.transform);
    }
    public override void OnHit(CharacterBase user, CharacterBase target)
    {

    }
    public override void EndComponent(CharacterBase user)
    {

    }
}
