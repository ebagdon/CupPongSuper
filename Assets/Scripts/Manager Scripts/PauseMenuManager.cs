using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PauseMenuManager : MonoBehaviour
{ 
    // IMPORTANT NORMALTIME SHOULD ALWAYS BE ONE
    private float normalTime = 1f;

    // our gameplay and pause canvas
    [SerializeField] private Canvas gameplayCanvas, pauseCanvas;

    // components
    private Rigidbody ballBody;
    private LookTowardsBall lookTorwardsBall;

    private void Awake()
    {
        // get components
        ballBody = FindObjectOfType<BallName>().gameObject.GetComponent<Rigidbody>();
        lookTorwardsBall = FindObjectOfType<CinemachineVirtualCamera>().GetComponent<LookTowardsBall>();
    }

    public void PauseGame()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable throwing
        BallClickController.instance.canThrow = false;
        
        // pause the game and enable the pause canvas
        Time.timeScale = 0f;
        pauseCanvas.enabled = true;
    }

    public void ResumeGame()
    {   
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // TIMESCALE DO NOT CHANGE
        Time.timeScale = normalTime;
        // TIMESCALE DO NOT CHANGE

        // enable throwing
        BallClickController.instance.canThrow = true;

        // resume the game and disable the pause canvas
        Time.timeScale = normalTime;
        pauseCanvas.enabled = false;
    }

    public void BackToMainMenu()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // TIMESCALE DO NOT CHANGE
        Time.timeScale = normalTime;
        // TIMESCALE DO NOT CHANGE

        // make sure we can't respawn the ball, stop hovering, and freeze the ball's rigidbody
        BallRespawnManager.instance.canRespawn = false;
        BallHover.instance.hover = false;
        ballBody.useGravity = false;
        ballBody.constraints = RigidbodyConstraints.FreezePosition;
        ballBody.constraints = RigidbodyConstraints.FreezeRotation;

        // disable the look towards ball script
        lookTorwardsBall.enabled = false;

        // start the transition to the main menu
        SceneTransitionsManager.instance.SetSceneTransitionToMainMenu();

        // disable canvases
        pauseCanvas.enabled = false;
        gameplayCanvas.enabled = false;
    }

    public void RestartGame()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // TIMESCALE DO NOT CHANGE
        Time.timeScale = normalTime;
        // TIMESCALE DO NOT CHANGE

        // make sure we can't respawn the ball, stop hovering, and freeze the ball's rigidbody
        BallRespawnManager.instance.canRespawn = false;
        BallHover.instance.hover = false;
        ballBody.useGravity = false;
        ballBody.constraints = RigidbodyConstraints.FreezePosition;
        ballBody.constraints = RigidbodyConstraints.FreezeRotation;

        // disable the look towards ball script
        lookTorwardsBall.enabled = false;

        // based on what scene we are in set the scene transition to that scene
        if (SceneManager.GetActiveScene().name == SceneNames.CLASSIC_GAMEMODE_SCENE_NAME)
            SceneTransitionsManager.instance.SetSceneTransitionToClassicGamemode();
        else if (SceneManager.GetActiveScene().name == SceneNames.PARTY_GAMEMODE_SCENE_NAME)
            SceneTransitionsManager.instance.SetSceneTransitionToPartyGamemode();

        // disable canvases
        pauseCanvas.enabled = false;
        gameplayCanvas.enabled = false;
    }
}