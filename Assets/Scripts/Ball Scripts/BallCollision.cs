using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class BallCollision : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static BallCollision instance;

    // the name of our scene
    private string sceneName;

    // cooldown timer for playing the table hit sound
    private float tableHitSoundCooldownTimer;
    private float tableHitSoundCooldownTimerThreshold = 0.15f;

    // cooldown timer for playing the cup hit sound
    private float cupSoundCooldownTimer;
    private float cupSoundCooldownTimerThreshold = 0.15f;
    
    // cooldown timer for playing the ramp hit sound and bool for if we should use this timer
    private float timeSinceHitRamp;
    private float timeSinceHitRampThreshold = 0.6f;
    private bool countTimeSinceHitRamp;

    // bool for if we have hit the ramp
    [HideInInspector] public bool hitRamp;

    // list of cups that we have hit and the cup that we last made
    [HideInInspector] public List<CupCollector> cupsHit = new List<CupCollector>();
    [HideInInspector] public GameObject lastMadeCup;

    // our ramp
    private GameObject ramp;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;

        // get the name of our scene
        sceneName = SceneManager.GetActiveScene().name;

        // get the ramp
        ramp = GameObject.FindWithTag(Tags.RAMP_TAG);
    }

    private void Update()
    {   
        // if we should do the timer and the timer is not finished, add to the timer
        if (countTimeSinceHitRamp && timeSinceHitRamp < timeSinceHitRampThreshold)
            timeSinceHitRamp += Time.deltaTime;
        
        // if we are not in the ramp shot event
        if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent)
        {
            // reset timer
            timeSinceHitRamp = 0f;
            countTimeSinceHitRamp = false;

            // reset hitRamp
            hitRamp = false;
        }
    }

    void CheckToPlayTableHitSound()
    {
        // if the timer is done
        if (Time.time >= tableHitSoundCooldownTimer)
        {   
            // play the sound and add to the cooldown timer
            SoundManager.instance.PlayTableHitSound();
            tableHitSoundCooldownTimer = Time.time + tableHitSoundCooldownTimerThreshold;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // tell the ballLife to respawn the ball if we collide with the room
        if (collision.gameObject.CompareTag(Tags.ROOM_TAG) && gameObject.activeInHierarchy)
            BallLife.instance.respawnBall = true;

        // if we collide with a cup
        if (collision.gameObject.CompareTag(Tags.CUP_TAG))
        {
            // if the timer is done and the ball is active
            if (Time.time >= cupSoundCooldownTimer && gameObject.activeInHierarchy)
            {
                // play the sound, play the default collision feedback, and add to the cooldown timer
                SoundManager.instance.PlayCupHitSound();
                cupSoundCooldownTimer = Time.time + cupSoundCooldownTimerThreshold;
            }

            // check to see if the cup we just hit is already in the cupsHit list
            bool sameCupFound = false;
            for (int i = 0; i < cupsHit.Count; i++)
            {
                if (collision.gameObject.name == cupsHit[i].transform.parent.gameObject.name)
                    sameCupFound = true;
            }

            // if this cup hasn't already been hit by us add it to the cupsHit list
            if (!sameCupFound)
                cupsHit.Add(collision.gameObject.GetComponentInChildren<CupCollector>());
        }

        // if we collided with the table and the ball is active
        if (collision.gameObject.CompareTag(Tags.TABLE_TAG) && gameObject.activeInHierarchy)
        {
            // switch based on scene name
            // based on scenes and current events check to see if we should check to play the
            // table hit sound
            switch (sceneName) {
                case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                    CheckToPlayTableHitSound();
                    break;
                
                case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                    if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent) {
                        CheckToPlayTableHitSound();
                    } else if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent && hitRamp) {
                        CheckToPlayTableHitSound();
                    }
                    break;
            }
        }

        // if we collide with a bank shot board and our ball is still active
        if (collision.gameObject.CompareTag(Tags.BANK_SHOT_BOARD_TAG) && gameObject.activeInHierarchy)
        {   
            // check if we should play the table hit sound
            CheckToPlayTableHitSound();

            // set the didBankShot bool to true
            GameManager.instance.didBankShot = true;
        }

        // if we collide with the ramp and our ball is still acitve 
        if (collision.gameObject.CompareTag(Tags.RAMP_TAG) && gameObject.activeInHierarchy)
        {  
            // if the timeSinceHitRamp timer is up 
            if (timeSinceHitRamp >= timeSinceHitRampThreshold)
            {
                // check if we should play the table hit sound
                CheckToPlayTableHitSound();
            }

            // set bools to true
            hitRamp = true;
            countTimeSinceHitRamp = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // if we collide with a cup tag set the lastMadeCup
        if (collision.CompareTag(Tags.CUP_TAG))
            lastMadeCup = collision.gameObject.transform.parent.gameObject;
    }
}