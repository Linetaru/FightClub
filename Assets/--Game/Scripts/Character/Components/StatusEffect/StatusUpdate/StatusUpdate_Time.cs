using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpdate_Time : StatusUpdate
{

    public override bool CanRemoveStatus(CharacterBase character)
    {
        return false;
    }
}
