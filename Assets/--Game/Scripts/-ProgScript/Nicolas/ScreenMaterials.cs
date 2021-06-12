using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMaterials : MonoBehaviour
{
    public List<Material> textures;
    public List<Material> materialScreen;
    int random = 0;
    int index = 0;
    float timerMax = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, materialScreen.Count);
        //random = 9;

        Material[] mats = this.GetComponent<MeshRenderer>().materials;
        mats[1] = materialScreen[random];
        this.GetComponent<MeshRenderer>().materials = mats;
    }

    // Update is called once per frame
    void Update()
    {
        if (random == 1)
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
