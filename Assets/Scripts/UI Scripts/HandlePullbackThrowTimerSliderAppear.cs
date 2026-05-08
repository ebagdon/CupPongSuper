using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePullbackThrowTimerSliderAppear : MonoBehaviour
{
    
    private void Awake()
    {
        // if we are not in the pullback throw type then disable this ui
        if (!DataManager.instance.GetBool(DataManager.instance.ballThrowMode_PULLBACK_DATA))
            gameObject.SetActive(false);
    }

}