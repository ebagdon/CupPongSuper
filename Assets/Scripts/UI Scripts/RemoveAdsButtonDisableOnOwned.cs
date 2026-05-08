using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class RemoveAdsButtonDisableOnOwned : MonoBehaviour
{
    // our button
    [SerializeField] private IAPButton removeAdsIAPButton;

    private void Update()
    {
        // if we have the button and we bought the no ads then destroy the button component
        if (removeAdsIAPButton && !DataManager.instance.GetBool(DataManager.instance.playAdsData)) {
            Destroy(removeAdsIAPButton);
        }
    }
}