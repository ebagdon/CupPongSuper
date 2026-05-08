using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();
    }
}