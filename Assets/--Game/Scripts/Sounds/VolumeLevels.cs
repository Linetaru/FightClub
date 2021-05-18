﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VolumeLevels : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image handleImage;

    private Selectable selectLeft;

    bool isSelected;
    bool isUpdating;

    private float masterVolume, musicVolume, voiceVolume, sfxVolume;

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        handleImage.color = new Color(0.53f, 0.53f, 0.53f, 1f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        handleImage.color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if(!isUpdating)
        {
            isUpdating = true;
            handleImage.color = new Color(0.43f, 0.43f, 0.43f, 1f);
            slider.interactable = true;
        }
        else
        {
            isUpdating = false;
            handleImage.color = new Color(0.53f, 0.53f, 0.53f, 1f);
            slider.interactable = false;
            slider.Select();
        }
    }

    public void SetVolume(string value)
    {
        if(value == "Master")
        {
            masterVolume = slider.value;
            //AkSoundEngine.SetRTPCVAlue("MasterVolume", masterVolume);

            Debug.Log("Le volume master est maintenant " + masterVolume);
        }

        if(value == "Music")
        {
            musicVolume = slider.value;
            //AkSoundEngine.SetRTPCVAlue("MusicVolume", musicVolume);

            Debug.Log("Le volume music est maintenant " + musicVolume);
        }
        if (value == "Voice")
        {
            voiceVolume = slider.value;
            //AkSoundEngine.SetRTPCVAlue("VoiceVolume", voiceVolume);

            Debug.Log("Le volume voice est maintenant " + voiceVolume);
        }

        if (value == "SFX")
        {
            sfxVolume = slider.value;
            //AkSoundEngine.SetRTPCVAlue("SFXVolume", sfxVolume);

            Debug.Log("Le volume sfx est maintenant " + sfxVolume);
        }
    }
}
