using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    /*
        THIS SCRIPT ONLY
        APPLIES FOR THE SLIDERS
        THAT ARE NOT THE MASTER SLIDER
    */

    // the audioMixer and ui slider
    [SerializeField] private AudioMixer audioMixer;
    private Slider slider;

    private void Awake()
    {
        // get the slider
        slider = GetComponentInParent<Slider>();
    }

    private void Start()
    {
        // get the value we have saved for the audio mixer
        float savedValue = 0f;
        if (transform.parent.name == UIObjectNames.MUSIC_VOLUME_SLIDER_NAME)
            savedValue = DataManager.instance.GetFloat(DataManager.instance.musicVolumeData);
        else if (transform.parent.name == UIObjectNames.SOUND_EFFECTS_VOLUME_SLIDER_NAME)
            savedValue = DataManager.instance.GetFloat(DataManager.instance.soundEffectsVolumeData);

        // set the audioMixer value and the ui slider value
        audioMixer.SetFloat(MixerParameters.MIXER_VOLUME_PARAMETER_NAME, savedValue);
        slider.value = savedValue;
    }

    public void SetVolume(float volume)
    {
        // set the audio mixer's volume
        audioMixer.SetFloat(MixerParameters.MIXER_VOLUME_PARAMETER_NAME, volume);

        // save the volume value
        if (transform.parent.name == UIObjectNames.MUSIC_VOLUME_SLIDER_NAME)
            DataManager.instance.SaveFloat(DataManager.instance.musicVolumeData, volume);
            
        if (transform.parent.name == UIObjectNames.SOUND_EFFECTS_VOLUME_SLIDER_NAME)
            DataManager.instance.SaveFloat(DataManager.instance.soundEffectsVolumeData, volume);
    }
}