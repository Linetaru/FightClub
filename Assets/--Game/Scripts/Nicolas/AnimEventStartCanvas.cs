using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventStartCanvas : MonoBehaviour
{
    public Menu.MenuMain menuMain;
    [HideInInspector] public bool delayed;

    public void SetCanvasStartAgain()
    {
        menuMain.SetCanvasStartAgain(delayed);
    }
}
