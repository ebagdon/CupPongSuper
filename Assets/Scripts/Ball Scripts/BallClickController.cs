using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BallClickController : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static BallClickController instance;

    // the name of our scene
    private string sceneName;

    // slider for our pullback ball throw mode
    [SerializeField] private Slider pullbackThrowTimerSlider;
    
    // player ball and it's rigidbody
    private GameObject playerBall;
    private Rigidbody playerBallBody;

    // forces and velocities for the ball throw
    private float defaultXForce = 3.15f, defaultYForce = 4.05f, defaultZForce = 2.05f;
    private float angularVelocityX = 0.23f, angularVelocityZ = 0.18f, angularVelocityY = 0.18f;

    // the position of our mouse, where it went down, and where it went up
    private Vector2 currentMousePos;
    private float mouseDownPos_Y;
    private float mouseUpPos_Y;
    private float mouseDownPos_X;
    private float mouseUpPos_X;

    // the side to side movement of the mouse and
    // up and down movement of the mouse from mouse down to mouse up
    [HideInInspector] public float throwDistanceX;
    [HideInInspector] public float attemptedThrowDistanceY;

    // variable for how far the swipe should go up the screen
    private float yThrowThreshold = 100f;

    // timers for how long we've been attempting to throw
    private float currentThrowingTime;
    private float validClassicThrowTimeThreshold = 0.5f;
    private float validPullbackThrowTimeThreshold = 1f;

    // bool for attempting to throw
    private bool attemptingThrowing;

    // bools for which ball throw mode we are using
    private bool legacyBallThrow;
    private bool pullbackBallThrow;

    // bool for if we should call ThrowBall()
    private bool callThrowBall;

    // bools for if we can throw and if we have thrown
    [HideInInspector] public bool canThrow = true;
    [HideInInspector] public bool thrown;

    // bool for if the ball has rebounded
    private bool ballRebounded;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;

        // get the name of our scene
        sceneName = SceneManager.GetActiveScene().name;

        // get the ball throw mode
        legacyBallThrow = DataManager.instance.GetBool(DataManager.instance.ballThrowMode_LEGACY_DATA);
        pullbackBallThrow = DataManager.instance.GetBool(DataManager.instance.ballThrowMode_PULLBACK_DATA);
    }

    private void Start()
    {
        // get the ball and it's rigidbody
        playerBall = GameObject.FindWithTag(Tags.PLAYERS_BALL_TAG);
        playerBallBody = playerBall.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // if the gameOverUI has respawnedAfterAd
        if (GameOverManager.instance.respawnedAfterAd)
        {
            canThrow = true;
            thrown = false;
        }

        // if we can throw
        if (canThrow)
        {
            // handle different ball throw modes
            if (legacyBallThrow)
                CheckForValidLegacyBallThrow();
            else if (pullbackBallThrow)
                HandlePullbackBallThrowType();
        }
        else { // if we can't throw set attemptingThrowing to false
            attemptingThrowing = false;
        }

        // if callThrowBall then ThrowBall()
        if (callThrowBall)
        {
            ThrowBall(attemptedThrowDistanceY);
            callThrowBall = false;
        }

        // if the ball y pos is greater than 28 then rebound the ball
        if (playerBall.transform.position.y > 28f && !ballRebounded)
        {   
            playerBallBody.velocity = new Vector3(playerBallBody.velocity.x, -playerBallBody.velocity.y * 0.4f,
                playerBallBody.velocity.z * 1.15f);
            
            ballRebounded = true;
        }
    }

    void CheckForValidLegacyBallThrow()
    {
        // get the current mouse pos
        currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);     

        // when we click/touch
        if(Input.GetMouseButtonDown(0))
        {   
            // get the mouse down pos and set the attemptingThrowing to true
            mouseDownPos_Y = currentMousePos.y;
            mouseDownPos_X = currentMousePos.x;
            attemptingThrowing = true;
        }

        // if we are attempting to throw add to the timer
        if (attemptingThrowing)
        {
            currentThrowingTime += Time.deltaTime;
        }

        // when we release a click/touch and we are attempting to throw
        if(Input.GetMouseButtonUp(0) && attemptingThrowing)
        {   
            // set the mouse up pos
            mouseUpPos_Y = currentMousePos.y;
            mouseUpPos_X = currentMousePos.x;

            // calculate the y and x throw distances
            attemptedThrowDistanceY = mouseUpPos_Y - mouseDownPos_Y;
            throwDistanceX = mouseUpPos_X - mouseDownPos_X;

            // if the timer is up and the attemptedThrowDistanceY is above the yThrowThreshold
            // then set callThrowBall to true
            if(currentThrowingTime <= validClassicThrowTimeThreshold && attemptedThrowDistanceY >= yThrowThreshold)
            {
                canThrow = false;
                callThrowBall = true;
            }

            // reset the timer and set attemptingThrowing to false
            currentThrowingTime = 0f;
            attemptingThrowing = false;
        }
    }

    void HandlePullbackBallThrowType()
    {
        // get the current mouse pos  
        currentMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        // when we click/touch
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            // get the mouseDownPos and set attemptingThrowing to true
            mouseDownPos_Y = currentMousePos.y;
            mouseDownPos_X = currentMousePos.x;
            attemptingThrowing = true;
        }

        // if we are attempting to throw add to the timer
        if (attemptingThrowing)
        {
            currentThrowingTime += Time.deltaTime;
        }

        // set the slider's value
        pullbackThrowTimerSlider.value = 1f - currentThrowingTime / validPullbackThrowTimeThreshold;

        // if the timer is done
        if (currentThrowingTime >= validPullbackThrowTimeThreshold)
        {   
            // set the mouseUpPos
            mouseUpPos_Y = currentMousePos.y;
            mouseUpPos_X = currentMousePos.x;

            // calculate the y and x throw distances
            attemptedThrowDistanceY = mouseUpPos_Y - mouseDownPos_Y;
            throwDistanceX = mouseUpPos_X - mouseDownPos_X;
            attemptedThrowDistanceY *= -1.1125f;
            throwDistanceX *= -1.1125f;

            // throw ther ball
            if (attemptedThrowDistanceY >= 0f)
                ThrowBall(attemptedThrowDistanceY);
            else
                ThrowBall(1f);

            // set bool to false
            GameOverManager.instance.respawnedAfterAd = false;

            // reset timer and bools
            currentThrowingTime = 0f;
            canThrow = false;
            attemptingThrowing = false;
        }

        // if we release the click/touch and we are attempting to throw
        if (Input.GetMouseButtonUp(0) && attemptingThrowing)
        {
            // get the mouse up pos
            mouseUpPos_Y = currentMousePos.y;
            mouseUpPos_X = currentMousePos.x;
            
            // calculate the y and x throw distances
            attemptedThrowDistanceY = mouseUpPos_Y - mouseDownPos_Y;
            throwDistanceX = mouseUpPos_X - mouseDownPos_X;
            attemptedThrowDistanceY *= -1.1f;
            throwDistanceX *= -1.1f;

            // throw the ball
            if (attemptedThrowDistanceY >= 0f)
                ThrowBall(attemptedThrowDistanceY);
            else
                ThrowBall(1f);

            // set bool to false
            GameOverManager.instance.respawnedAfterAd = false;

            // reset timer and bools
            currentThrowingTime = 0f;
            canThrow = false;
            attemptingThrowing = false;
        }
    }

    void ThrowBall(float throwDistanceY)
    {
        // play the ball throw sound
        SoundManager.instance.PlayBallThrowSound();

        // stop and reset the streakSound
        SoundManager.instance.streakSound.Stop();
        SoundManager.instance.streakSound.time = 0f;

        // get the forces needed to throw the ball
        float xForce = defaultXForce;
        float yForce = defaultYForce;
        float zForce = defaultZForce;

        // set a specific y and z force if we are in the rampShotEvent
        if ( GameManager.instance.currentEvent == GameManager.instance.rampShotEvent)
        {
            yForce *= 0f;
            zForce *= 2.205128205128205f;
        }

        // switch based on throwDistanceY
        bool caseFound = false;
        switch (throwDistanceY)
        {
            // based on throwDistanceY add force to the ball
            case >= 1050f:
                playerBallBody.AddForce(new Vector3(-xForce * throwDistanceX / 100f, yForce * throwDistanceY / yThrowThreshold, -zForce * throwDistanceY / yThrowThreshold), ForceMode.Impulse);
                caseFound = true;
                break;
            case >= 850f:
                playerBallBody.AddForce(new Vector3(-xForce * throwDistanceX / 100f, yForce * throwDistanceY / yThrowThreshold * 1.28f, -zForce * throwDistanceY / yThrowThreshold * 1.28f), ForceMode.Impulse);
                caseFound = true;
                break;
            case >= 450f:
                playerBallBody.AddForce(new Vector3(-xForce * throwDistanceX / 100f, yForce * throwDistanceY / yThrowThreshold * 1.45f, -zForce * throwDistanceY / yThrowThreshold * 1.45f), ForceMode.Impulse);
                caseFound = true;
                break;
            case >= 310f:
                playerBallBody.AddForce(new Vector3(-xForce * throwDistanceX / 100f, yForce * throwDistanceY / (yThrowThreshold * 0.64f), -zForce * throwDistanceY / (yThrowThreshold * 0.6f) * 1.15f), ForceMode.Impulse);
                caseFound = true;
                break;
            case >= 200f:
                playerBallBody.AddForce(new Vector3(-xForce * throwDistanceX / 100f, yForce * throwDistanceY / yThrowThreshold * 2.15f, -zForce * throwDistanceY / yThrowThreshold * 2.15f), ForceMode.Impulse);
                caseFound = true;
                break;
        }
        if (!caseFound) playerBallBody.AddForce(new Vector3(-xForce * throwDistanceX / 100f, yForce * throwDistanceY / yThrowThreshold * 3.05f, -zForce * throwDistanceY / yThrowThreshold * 3.05f), ForceMode.Impulse);

        // apply an angular force to the ball
        playerBallBody.angularVelocity = new Vector3(-angularVelocityX / 18f * throwDistanceY / 0.0195f * Time.deltaTime, angularVelocityY / 18f * throwDistanceX / 0.0195f * Time.deltaTime, angularVelocityZ / 18f * throwDistanceX / 0.0195f * Time.deltaTime);

        // set gameOverUI.respawnedAfterAd to false
        GameOverManager.instance.respawnedAfterAd = false;

        // disable the ball hover and make it so the ball can respawn
        BallHover.instance.hover = false;
        BallRespawnManager.instance.canRespawn = true;

        // set thrown to true
        thrown = true;

        // if we are in a ramp shot event set the GameManager.instance.addRampThrowTime to true
        if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent)
        {
            GameManager.instance.addRampThrowTime = true;
        }

        // make it so the ball uses gravity
        playerBallBody.useGravity = true;      
    }
}