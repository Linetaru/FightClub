using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpdate_Knockback : StatusUpdate
{

    public override StatusUpdate Copy()
    {
        return new StatusUpdate_Knockback();
    }

    public override bool CanRemoveStatus(CharacterBase character)
    {
        return character.Knockback.KnockbackDuration > 0;
    }
}
