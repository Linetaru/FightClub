using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverGonnaGiveYouUp : MonoBehaviour
{
    public Material material1; 
    public Material material2;
    public List<Material> textures;
    int random = 0;
    int index = 0;
    float timerMax = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, 11);
        random = 5;

        if(random == 5)
        {
            Material[] mats = this.GetComponent<MeshRenderer>().materials;
            mats[1] = material2;
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
