using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    private bool isEnabled;
    public bool IsEnabled
    {
        get { return isEnabled; }
        set { isEnabled = value; }
    }
    public virtual void SwitchIcon()
    {
    }
    public virtual void DestroySelf()
    {
    }
}
