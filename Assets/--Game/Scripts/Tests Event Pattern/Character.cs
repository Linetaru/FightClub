using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyAction(KeyCode keyCode)
    {
        if(keyCode == KeyCode.Mouse0)
        {
            Debug.Log("It's Working bitch !");
        }
    }
}
