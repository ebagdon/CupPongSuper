using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonSize : MonoBehaviour
{
    // current size
    private Vector3 currentSize;

    // speeds for changing size
    private float sizeChangeSpeed = 3.5f;
    private float originalSizeChangeSpeed = 3.5f;
    private float rateToChangeSpeed = 1.75f;

    // what sizes to start changing speed
    private float startSlowDownMin = 0.998f;
    private float startSlowDownMax = 1.01f;

    private void FixedUpdate()
    {
        // handle the size change
        HandleSizeChange();
    }

    void HandleSizeChange()
    {
        // get the current size
        currentSize = transform.localScale;

        // if the size is becoming too big and the speed to fast decrease the speed
        if (currentSize.x >= startSlowDownMax && sizeChangeSpeed > -originalSizeChangeSpeed)
        {
            sizeChangeSpeed -= rateToChangeSpeed * Time.deltaTime;
        }
        else if (currentSize.x <= startSlowDownMin && sizeChangeSpeed < originalSizeChangeSpeed) {
            // if the size is becoming too small and the speed is to fast decrease the speed
            sizeChangeSpeed += rateToChangeSpeed * Time.deltaTime;
        }

        // add to the size
        currentSize.x += sizeChangeSpeed / 20 * Time.deltaTime;
        currentSize.y += sizeChangeSpeed / 20 * Time.deltaTime;

        // set the size
        transform.localScale = currentSize;
    }
}