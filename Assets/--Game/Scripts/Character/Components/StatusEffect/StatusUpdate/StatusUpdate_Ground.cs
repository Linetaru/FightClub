using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpdate_Ground : StatusUpdate
{

    public override StatusUpdate Copy()
    {
        return new StatusUpdate_Ground();
    }

    public override bool CanRemoveStatus(CharacterBase character)
    {
        return character.Rigidbody.IsGrounded;
    }
}
