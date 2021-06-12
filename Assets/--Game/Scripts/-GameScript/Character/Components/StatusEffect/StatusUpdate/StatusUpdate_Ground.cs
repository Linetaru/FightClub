using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpdate_Ground : StatusUpdate
{
    bool wasGrounded = false;

    public StatusUpdate_Ground()
    {
        wasGrounded = false;
    }

    public override StatusUpdate Copy()
    {
        return new StatusUpdate_Ground();
    }

    public override bool CanRemoveStatus(CharacterBase character)
    {
        if (character.Rigidbody.IsGrounded == false && wasGrounded == false)
            wasGrounded = true;
        if (character.Rigidbody.IsGrounded && wasGrounded)
            return true;
        return false;
    }
}
