using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenFadeOut : MonoBehaviour
{
    // the image we want to fade out
    [SerializeField] private Image fadeImage;

    // the alpha, the time that has to pass before we start to fade it out, and the speed we fade it out at
    private float fadeImageAlpha;
    private float fadeAfterTime = 0.65f;
    private float fadeSpeed = 10f;

    private void Awake()
    {
        // enable the image
        fadeImage.enabled = true;
    }

    private void Update()
    {   
        // if enough time has passed start to fade it out
        if (Time.time >= fadeAfterTime)
            HandleFade();
    }

    void HandleFade()
    {
        // get the alpha
        fadeImageAlpha = fadeImage.color.a;

        // if the alpha is greater than 0 subtract from it 
        if (fadeImageAlpha > 0)
            fadeImageAlpha -= fadeSpeed * Time.deltaTime;
        else // if the alpha is 0 disable the canvas
            this.gameObject.GetComponent<Canvas>().enabled = false;

        // apply the alpha
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageAlpha);
    }
}