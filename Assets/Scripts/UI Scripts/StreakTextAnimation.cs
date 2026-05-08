using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreakTextAnimation : MonoBehaviour
{
    // the text size and speed to change that size
    private int fullTextSize = 160;
    private int textSizeChangeSpeed = 8;
    
    // timer for the time at the text's full size
    private float timeAtFullTextSize;
    private float timeAtFullTextSizeThreshold = 0.55f;

    // our z rotation and speed to rotate it
    private float zRotation;
    private float zRotationSpeed = 67f;

    // our min and max z rotation for the text
    private float minZRotation = 13f;
    private float maxZRotation = -13f;

    // bools for the animation
    private bool firstPartAnimDone;
    private bool readyForSecondPartAnim;
    private bool secondPartAnimDone;
    private bool anim;

    // our text
    private Text text;

    private void Awake()
    {
        // get our text
        text = GetComponent<Text>();

        // disable the text and set the size to 0
        text.enabled = false;
        text.fontSize = 0;
    }

    private void Update()
    {
        // if anim is true play the animation
        if (anim) {
            PlayStreakTextAnimation();
        }
    }

    void PlayStreakTextAnimation()
    {
        // handle the rotation of the text
        Rotate();

        // if the first part of the animation is not done and the size of the text isn't at it's full size
        if (text.fontSize < fullTextSize - 10 && !firstPartAnimDone)
        {
            // enable the text
            text.enabled = true;

            // increase the text size
            text.fontSize = (int)Mathf.Lerp(text.fontSize, fullTextSize, textSizeChangeSpeed * Time.deltaTime);
        }  
        else { // we are done growing the text
            firstPartAnimDone = true;
        }

        // if the first part of the animation is done
        if (firstPartAnimDone)
        {
            // add to the timer
            timeAtFullTextSize += Time.deltaTime;

            // when the timer is done
            if (timeAtFullTextSize >= timeAtFullTextSizeThreshold) {
                // set readyForSecondPartAnim to true and reset timer
                readyForSecondPartAnim = true;
                timeAtFullTextSize = 0f;
            }
        }

        // if we can still see the text due to it's size and we are ready for the second part of the animation
        if (text.fontSize > 0 && readyForSecondPartAnim)
        {
            // decrease the text size
            text.fontSize = (int)Mathf.Lerp(text.fontSize, 0, textSizeChangeSpeed * Time.deltaTime);
        }

        // if we are in the second part of the animation and the font size is less than 2
        if (text.fontSize <= 2 && readyForSecondPartAnim) { // if the text size is 0
            // set secondPartAnimDone to true
            secondPartAnimDone = true;
        }

        // if the second part of the animation is done
        if (secondPartAnimDone)
        {
            // disable the text
            text.enabled = false;

            // reset bools
            firstPartAnimDone = false;
            readyForSecondPartAnim = false;
            secondPartAnimDone = false;
            anim = false;
        }
    }

    void Rotate()
    {
        // if the zRotation goes beyond the bounds then reverse the rotation speed
        if (zRotation >= minZRotation)
            zRotationSpeed *= -1f;
        else if (zRotation <= maxZRotation)
            zRotationSpeed *= -1f;

        // add to the zRotation and apply the rotation
        zRotation += zRotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, zRotation);
    }

    public void InitiateAnim()
    {
        // set anim to true
        anim = true;
    }
}