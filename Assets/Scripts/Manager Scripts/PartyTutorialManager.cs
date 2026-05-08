using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyTutorialManager : MonoBehaviour
{
    // our camera
    private Camera mainCam;

    // our tutorial canvas and after tutorial canvas
    [SerializeField] private Canvas tutorialCanvas, afterTutorialCanvas;
    
    // texts
    [SerializeField] private Text tapToContinueText, tapToPlayText;

    // texts for our different "states"
    [SerializeField] private Text state1Text, state2Text, state3Text;

    // pause button and slider for our pullback ball throw mode
    [SerializeField] private Button pauseButton;
    [SerializeField] private Slider pullbackThrowSlider;

    // bank shot boards, tutorial targets, and the ramp
    [SerializeField] private GameObject[] bankShotBoards, tutorialTargets;
    [SerializeField] private GameObject ramp;

    // bank shot board mat, ramp mat, and target mats
    [SerializeField] private Material bankShotBoardMat, rampMat, targetRedMat, targetWhiteMat;

    // camera pos for the tutorial
    [SerializeField] private Transform tutorialCameraPos;

    // which state of the tutorial we are in
    private int tutorialState;

    private void Start()
    {
        // get our camera
        mainCam = Camera.main;

        // if we have not done this tutorial before
        if (!DataManager.instance.GetBool(DataManager.instance.partyGamemodeTutorialDoneData))
        {
            // enable the tutorial canvas
            tutorialCanvas.enabled = true;

            // disable the pause button and pull back ball throw mode slider
            pauseButton.gameObject.SetActive(false);
            pullbackThrowSlider.gameObject.SetActive(false);

            // change the FOV and position of the camera
            mainCam.fieldOfView = 72.8f;
            mainCam.transform.position = tutorialCameraPos.position;

            // go through all of the bank shot boards and activate them
            for (int i = 0; i < bankShotBoards.Length; i++)
            {
                bankShotBoards[i].SetActive(true);
            }

            // set all the materials' alpha to 1
            bankShotBoardMat.color = new Color(bankShotBoardMat.color.r, bankShotBoardMat.color.g,
                bankShotBoardMat.color.b, 1);
            rampMat.color = new Color(rampMat.color.r, rampMat.color.g,
                rampMat.color.b, 1);
            targetRedMat.color = new Color(targetRedMat.color.r, targetRedMat.color.g,
                targetRedMat.color.b, 1);
            targetWhiteMat.color = new Color(targetWhiteMat.color.r, targetWhiteMat.color.g,
                targetWhiteMat.color.b, 1);

            // make it so we can't throw the ball
            BallClickController.instance.canThrow = false;
        }
    }

    private void Update()
    {
        // if the tutorial canvas is enabled run the tutorial
        if (tutorialCanvas.enabled)
            HandleTutorial();
    }

    void HandleTutorial()
    {
        // if we click, we aren't in the second part of a scene transition, and the tutorial state is less than 3
        if (Input.GetMouseButtonDown(0) && !SceneTransitionsManager.instance.secondPartSceneTransition &&
            tutorialState < 3)
        {
            // play the button sound
            SoundManager.instance.PlayButtonClickSound();

            // add one to the tutorial state
            tutorialState++;

            if (tutorialState == 1)
            {
                // go through all the bank shot boards and disable them
                for (int i = 0; i < bankShotBoards.Length; i++) {
                    bankShotBoards[i].SetActive(false);
                }
                
                // go through all of the targets and enable them
                for (int i = 0; i < tutorialTargets.Length; i++) {
                    tutorialTargets[i].SetActive(true);
                }

                // disable and enable texts
                state1Text.enabled = false;
                state2Text.enabled = true;
            }
            else if (tutorialState == 2)
            {
                // go through all of the targets and disable them
                for (int i = 0; i < tutorialTargets.Length; i++) {
                    tutorialTargets[i].SetActive(false);
                }
                
                // enable the ramp
                ramp.SetActive(true);

                // disable and enable texts
                state2Text.enabled = false;
                state3Text.enabled = true;
            }

            if (tutorialState > 2)
            {
                // // save the data so we know we have done this tutorial
                DataManager.instance.SaveBool(DataManager.instance.partyGamemodeTutorialDoneData, true);

                // disable and enable canvas
                tutorialCanvas.enabled = false;
                afterTutorialCanvas.enabled = true;

                // start the scene transition to the party gamemode
                SceneTransitionsManager.instance.SetSceneTransitionToPartyGamemode();
            }
        }

        if (tutorialState >= 2 && tapToContinueText.enabled)
        {
            // disable and enable texts
            tapToContinueText.enabled = false;
            tapToPlayText.enabled = true;
        }
    }
}