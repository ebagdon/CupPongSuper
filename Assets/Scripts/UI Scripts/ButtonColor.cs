using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{    
    // bools for what type of button we are
    [SerializeField] private bool isBallThrowTypeLegacyButton, isBallThrowTypePullbackButton;

    // current collors
    private Color imageColor;
    private Color textColor;

    // buttons colors
    private Color32 imageFullColor = new Color32(255, 255, 255, 255);
    private Color32 imageTransparentBallThrowTypeButtonColor = new Color32(255, 255, 255, 215);

    // color change speed
    private float colorChangeSpeed = 1f;

    // components
    [SerializeField] private Image image;

    private void Start()
    {
        // if we are a ball throw type legacy or pullback mode button and the
        // ball throw mode is not set to our ball throw mode
        if (isBallThrowTypeLegacyButton && !DataManager.instance.GetBool(DataManager.instance.ballThrowMode_LEGACY_DATA))
        {
            // set the image color
            image.color = imageTransparentBallThrowTypeButtonColor;
        }
        else if (isBallThrowTypePullbackButton && ! DataManager.instance.GetBool(DataManager.instance.ballThrowMode_PULLBACK_DATA)) {
            // set the image color
            image.color = imageTransparentBallThrowTypeButtonColor;
        }
    }

    private void Update()
    {
        // based on what throw type we are a button for run that function
        if (isBallThrowTypeLegacyButton)
            ChangeColorsBallThrowTypeLegacyButton();
        else if (isBallThrowTypePullbackButton)
            ChangeColorsBallThrowTypePullbackButton();
    }

    void ChangeColorsBallThrowTypeLegacyButton()
    {
        // get the image color
        imageColor = image.color;

        // if the legacy ball throw mode is on
        if (DataManager.instance.GetBool(DataManager.instance.ballThrowMode_LEGACY_DATA))
        {
            // if the alpha isn't full add to it
            if (imageColor.a < 1f)
                imageColor.a += colorChangeSpeed * Time.deltaTime;
        }
        else {
            // if the image alpha is above our transparent alpha decrease it
            if (imageColor.a > 0.88f)
                imageColor.a -= colorChangeSpeed * Time.deltaTime;
        }

        // set the image color
        image.color = imageColor;
    }

    void ChangeColorsBallThrowTypePullbackButton()
    {
        // get the image color
        imageColor = image.color;

        // if the pullback ball throw mode is on
        if (DataManager.instance.GetBool(DataManager.instance.ballThrowMode_PULLBACK_DATA))
        {
            // if the alpha isn't full add to it
            if (imageColor.a < 1f)
                imageColor.a += colorChangeSpeed * Time.deltaTime;
        }
        else {
            // if the image alpha is above our transparent alpha decrease it
            if (imageColor.a > 0.88f)
                imageColor.a -= colorChangeSpeed * Time.deltaTime;
        }

        // set the image color
        image.color = imageColor;
    }
}