using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeSlider : MonoBehaviour
{
    // our slider
    private Slider masterSlider;

    private void Awake()
    {
        // get the slider
        masterSlider = GetComponentInParent<Slider>();
    }

    private void Start()
    {
        // get the data that was saved, set the audio listener's volume, and the slider's value
        float savedValue = DataManager.instance.GetFloat(DataManager.instance.masterVolumeData);
        AudioListener.volume = savedValue;
        masterSlider.value = savedValue;
    }   

    // called whenever the sli
    public void AdjustMasterVolume(float volume)
    {   
        // set the audio listener's volume and save the data
        AudioListener.volume = volume;
        DataManager.instance.SaveFloat(DataManager.instance.masterVolumeData, volume);
    }
}