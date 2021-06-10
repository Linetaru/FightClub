using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMaterials : MonoBehaviour
{
    public Material material1; 
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;
    public Material material6;
    public Material material7;
    public Material material8;
    public Material material9;
    public List<Material> textures;
    int random = 0;
    int index = 0;
    float timerMax = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, 11);
        //random = 9;

        if (random == 5)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material2;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 2)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material3;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 3)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material7;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 8)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material8;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 9)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material9;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 7)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material4;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 4)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material6;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else if (random == 10)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material5;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
        else
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material1;
            this.GetComponent<MeshRenderer>().materials = mats;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (random == 5)
        {
            if (timerMax <= 0)
            {
                if (index >= textures.Count - 1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                Material[] mats = this.GetComponent<MeshRenderer>().materials;
                mats[1] = textures[index];
                this.GetComponent<MeshRenderer>().materials = mats;
                timerMax = 0.1f;
            }
            else
            {
                timerMax -= Time.deltaTime;
            }
        }
    }
}
