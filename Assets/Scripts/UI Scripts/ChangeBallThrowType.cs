using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBallThrowType : MonoBehaviour
{
    public void BallThrowTypeLegacy()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // set ball throw type to legacy
        DataManager.instance.SaveBool(DataManager.instance.ballThrowMode_PULLBACK_DATA, false);
        DataManager.instance.SaveBool(DataManager.instance.ballThrowMode_LEGACY_DATA, true);
    }

    public void BallThrowTypePullback()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // set ball throw type to pullback
        DataManager.instance.SaveBool(DataManager.instance.ballThrowMode_LEGACY_DATA, false);
        DataManager.instance.SaveBool(DataManager.instance.ballThrowMode_PULLBACK_DATA, true);
    }
}