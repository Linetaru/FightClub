using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnlockable
{
    bool GetUnlocked(int i);

    bool GetUnlocked(string id);

    void SetUnlocked(int i, bool b);

    void SetUnlocked(string item, bool b);
}
