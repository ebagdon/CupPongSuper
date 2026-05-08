using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassicTutorialManager : MonoBehaviour
{
    // our camera
    private Camera mainCam;

    // our tutorial canvas
    [SerializeField] private Canvas tutorialCanvas;

    // our dark overlay
    [SerializeField] private Image darkOverlayImage;
    
    // texts
    [SerializeField] private Text tapToContinueText, tapToPlayText;

    // texts for our different "states"
    [SerializeField] private Text state1Text, state2Text, state3Text, state4Text;

    // pause button and slider for our pullback ball throw mode
    [SerializeField] private Button pauseButton;
    [SerializeField] private Slider pullbackThrowSlider;

    // our image alpha and alpha change speed
    private float darkOverlayImageAlpha;
    private float darkOverlayImageAlphaChangeSpeed = 50f;

    // which state of the tutorial we are in
    private int tutorialState;

    // if we should close the tutorial
    private bool closeTutorial;

    private void Start()
    {   
        // get our camera
        mainCam = Camera.main;

        // if we have not done this tutorial before
        if (!DataManager.instance.GetBool(DataManager.instance.classicGamemodeTutorialDoneData))
        {
            // enable the tutorial canvas
            tutorialCanvas.enabled = true;

            // disable the pause button and pull back ball throw mode slider
            pauseButton.gameObject.SetActive(false);
            pullbackThrowSlider.gameObject.SetActive(false);

            // disable throwing the ball
            BallClickController.instance.canThrow = false;

            // enable the tutorial canvas
            tutorialCanvas.enabled = true;
        }
    }

    private void Update()
    {
        // run the tutorial if the canvas is open
        if (tutorialCanvas.enabled)
            HandleTutorial();
    }

    void HandleTutorial()
    {   
        // if we click, we aren't in the second part of a scene transition, and the tutorial state is less than 4
        if (Input.GetMouseButtonDown(0) && !SceneTransitionsManager.instance.secondPartSceneTransition &&
            tutorialState < 4)
        {
            // play the button sound
            SoundManager.instance.PlayButtonClickSound();

            // add one to the tutorial state
            tutorialState++;

            if (tutorialState == 1)
            {
                // disable and enable texts
                state1Text.enabled = false;
                state2Text.enabled = true;
            }
            else if (tutorialState == 2)
            {
                // disable and enable texts
                state2Text.enabled = false;
                state3Text.enabled = true;
            }
            else if (tutorialState == 3)
            {
                // disable and enable texts
                state3Text.enabled = false;
                state4Text.enabled = true;

                // disable and enable texts
                tapToContinueText.enabled = false;
                tapToPlayText.enabled = true;
            }

            // if the tutorialState is greater than 3 finish the tutorial
            if (tutorialState > 3)
            {
                // save the data so we know we have done this tutorial
                DataManager.instance.SaveBool(DataManager.instance.classicGamemodeTutorialDoneData, true);

                // disable texts
                state4Text.enabled = false;
                tapToPlayText.enabled = false;

                // set closeTutorial to true
                closeTutorial = true;
            }
        }
        
        // if we should close the tutorial handle closing it
        if (closeTutorial)
            HandleCloseTutorial();
    }

    void HandleCloseTutorial()
    {
        // set the alpha variable
        darkOverlayImageAlpha = darkOverlayImage.color.a;

        // if the dark overlay isn't completely invisible
        if (darkOverlayImageAlpha > 0)
        {
            // subtract from the alpha
            darkOverlayImageAlpha -= darkOverlayImageAlphaChangeSpeed / 50 * Time.deltaTime;
        }
        else if (darkOverlayImageAlpha <= 0) // if the dark overlay is completely invisible
        {
            // enable throwing the ball
            BallClickController.instance.canThrow = true;

            // enable the pause button and pull back ball throw mode slider if we are using the
            // pullback ball throw mode
            pauseButton.gameObject.SetActive(true);
            if (DataManager.instance.GetBool(DataManager.instance.ballThrowMode_PULLBACK_DATA))
                pullbackThrowSlider.gameObject.SetActive(true);

            // set closeTutorial to false and close the tutorialCanvas
            closeTutorial = false;
            tutorialCanvas.enabled = false;
        }

        // set the alpha of the image
        darkOverlayImage.color = new Color(darkOverlayImage.color.r, darkOverlayImage.color.g,
            darkOverlayImage.color.b, darkOverlayImageAlpha);
    }
}