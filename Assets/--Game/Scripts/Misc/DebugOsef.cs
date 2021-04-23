using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOsef : MonoBehaviour
{

    [SerializeField]
    CharacterBase characterBase;
    Input_Info input;

    private void Start()
    {
        input = new Input_Info();
    }
    private void Update()
    {
        characterBase.UpdateControl(0, input);
    }
}
