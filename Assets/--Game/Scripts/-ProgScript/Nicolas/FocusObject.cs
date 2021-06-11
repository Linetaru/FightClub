using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class FocusObject : MonoBehaviour
{
    BoxCollider boxCollider;

    [Scene]
    public string scene;

    private void Reset()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
            boxCollider.hideFlags = HideFlags.HideInInspector;
    }

    [Button]
    public void HideFlag()
    {
        if (boxCollider != null)
            if(boxCollider.hideFlags == HideFlags.HideInInspector)
                boxCollider.hideFlags = HideFlags.None;
            else
                boxCollider.hideFlags = HideFlags.HideInInspector;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
