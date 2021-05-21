using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    [SerializeField]
    private GameObject InputsSettings;
    [SerializeField]
    private GameObject AudioSettings;

    public void EnableAudioSettings()
    {
        InputsSettings.SetActive(false);
        AudioSettings.SetActive(true);
    }

    public void EnableInputsSettings()
    {
        InputsSettings.SetActive(true);
        AudioSettings.SetActive(false);
    }
    

}
