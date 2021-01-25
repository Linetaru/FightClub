using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PackageCreator.Event;

public class InputController : MonoBehaviour
{
    public GameEventKeycode eventKeycode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            eventKeycode.Raise(KeyCode.Mouse0);
        }
    }
}
