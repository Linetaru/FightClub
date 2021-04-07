using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimPref : MonoBehaviour
{
    public AK.Wwise.Event soundEvent;

    public Material[] materials;
    public float fRate = 0.33f;

    MeshRenderer meshRenderer;
    float counter = 0f;
    int length;
    int index = 1;

    private void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180f);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = materials[0];
        length = materials.Length;

        soundEvent.Post(gameObject);
    }

    void Update()
    {
        counter += Time.deltaTime;
        if(counter >= fRate)
        {
            counter = 0f;
            meshRenderer.material = materials[index];
            index++;
            if (index == length - 1)
                index = 0;
        }
    }
}
