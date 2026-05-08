using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallRespawnManager : MonoBehaviour
{
    // public instance that can be accessed anywhere in my code
    public static BallRespawnManager instance;

    // the name of our scene
    private string sceneName;

    // our player's ball, it's rigidbody and collider
    private GameObject ball;
    private Rigidbody ballBody;
    private CapsuleCollider ballCollider;

    // ball spawn positions
    [SerializeField] private Transform ballSpawnPosition, rampShotEventBallSpawnPosition;

    // timer variables for respawing
    private float respawnTimeWaited;
    private float respawnWaitTimeThresholdRespawnCalled = 0.15f; // ball is not in cup
    private float respawnWaitTimeThresholdBallDisabled = 0.35f; // ball is in cup

    // speed at which we reposition ball if it is not in a cup
    private float ballMoveRespawnSpeed = 55f;
    private bool ballDoneRepositioning;

    // if we have subtracted a miss
    private bool missSubtracted;

    // variables for if the cups can collect the ball and if we can respawn the ball
    [HideInInspector] public bool canCollect = true, canRespawn = true;

    // if we started a new event
    private bool startedNewEvent;

    private void Awake()
    {
        // public instance that can be accessed anywhere in my code
        if (instance == null)
            instance = this;

        // the name of our scene
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        // get the ball, it's rigidbody and collider
        ball = GameObject.FindWithTag(Tags.PLAYERS_BALL_TAG);
        ballBody = ball.GetComponent<Rigidbody>();
        ballCollider = ball.GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        // if we can respawn the ball check to respawn it
        if (canRespawn)
            CheckToRespawnBall();
    }

    void CheckToRespawnBall()
    {
        // respawnBall is for times when the ball is not in a cup
        if (BallLife.instance.respawnBall)
        {
            // add to timer
            respawnTimeWaited += Time.deltaTime;

            // if the timer is donw
            if (respawnTimeWaited >= respawnWaitTimeThresholdRespawnCalled)
            {  
                // stop using gravity and cancel velocities
                ballBody.useGravity = false;
                ballBody.velocity = Vector3.zero;
                ballBody.angularVelocity = Vector3.zero;

                // make the ballCollider a trigger
                ballCollider.isTrigger = true;

                // if we have not removed a miss yet
                if (!missSubtracted)
                {
                    // remove a miss, reset our streak, and remove from our earned coins
                    GameManager.instance.missesLeft--;
                    GameManager.instance.ResetStreak();
                    GameManager.instance.RemoveEarnedCoins();

                    // we will still be able to get the 0 missesLeft text that we want
                    if (GameManager.instance.missesLeft > 0)
                        GameManager.instance.UpdateMissesLeftText();

                    // if we have 0 misses left
                    if (GameManager.instance.missesLeft <= 0)
                    {   
                        // make sure we can't respawn and return
                        canRespawn = false;
                        return;
                    }

                    // set missSubtracted to true
                    missSubtracted = true;
                }

                // if we have not ended the game by winning
                if (GameManager.instance.winGameOver)
                    return;

                // if we are in the party gamemode and we have not started a new event
                if (sceneName == SceneNames.PARTY_GAMEMODE_SCENE_NAME && !startedNewEvent)
                {
                    // start a new event
                    GameManager.instance.NewEvent();
                    startedNewEvent = true;

                    // if we are in the ramp shot event make the ball use gravity
                    if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent)
                        ballBody.useGravity = true;
                }

                // switch based on the sceneName
                switch (sceneName)
                {
                    // if we are in the classic gamemode move the ball, reset the rotation and enable hover when it's done
                    case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                        if (Vector3.Distance(ball.transform.position, ballSpawnPosition.position) > 0.01f)
                            MoveBall(ballSpawnPosition);
                        else {
                            BallHover.instance.hover = true;
                            ballBody.rotation = GameManager.instance.initialBallRotation;
                            ballDoneRepositioning = true;
                        }
                        break;

                    // if we are in the party gamemode move the ball, reset the rotation
                    case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                        if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent && Vector3.Distance(ball.transform.position, ballSpawnPosition.position) > 0.01f)
                        {
                            MoveBall(ballSpawnPosition);
                        }
                        else if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent && Vector3.Distance(ball.transform.position, rampShotEventBallSpawnPosition.position) > 0.01f) {
                            MoveBall(rampShotEventBallSpawnPosition);
                        }
                        else {
                            // if we aren't in the ramp shot event enable the ball hover
                            if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent)
                                BallHover.instance.hover = true;

                            // reset ball rotation and say we are done repositioning
                            ballBody.rotation = GameManager.instance.initialBallRotation;
                            ballDoneRepositioning = true;
                        }
                        break;
                }

                // if the ball is done repositioning
                if (ballDoneRepositioning)
                {
                    // reset bool
                    missSubtracted = false;

                    // clear the cupsHit list and reset a bool
                    BallCollision.instance.cupsHit.Clear();
                    BallCollision.instance.hitRamp = false;

                    // if we are not in the ramp shot event enable the ball hover
                    if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent)
                        ball.GetComponent<BallHover>().enabled = true;
                    
                    // make it so cups can collect the ball again
                    canCollect = true;

                    // make the collider a solid one again
                    ballCollider.isTrigger = false;
                    
                    // reset the ball mass
                    BallGravityIncreaser.instance.currentMass = BallGravityIncreaser.instance.minMass;

                    // reset bools
                    startedNewEvent = false;
                    BallClickController.instance.thrown = false;
                    BallClickController.instance.canThrow = true;

                    // reset timer
                    GameManager.instance.addRampThrowTime = false;
                    GameManager.instance.currentRampThrowTime = 0f;

                    // reset timer
                    respawnTimeWaited = 0f;

                    // reset bools
                    BallLife.instance.respawnBall = false;
                    ballDoneRepositioning = false;
                }
            }
        }

        // our ball made it in a cup
        if (!ball.activeInHierarchy)
        { 
            // add to timer
            respawnTimeWaited += Time.deltaTime;

            // if timer is done
            if (respawnTimeWaited >= respawnWaitTimeThresholdBallDisabled)
            {
                // bool for missed and checking if we are in the party gamemode
                bool missed = false;
                if (sceneName == SceneNames.PARTY_GAMEMODE_SCENE_NAME)
                {
                    // switch based on the currentEvent
                    switch (GameManager.instance.currentEvent)
                    {   
                        // if we are in the bank shot event and we didn't bank it then we miss
                        case "BANK SHOT":
                            if (!GameManager.instance.didBankShot) {
                                missed = true;
                                Miss();
                            }
                            break;

                        // if we are in the target event and we didn't hit a target then we miss
                        case "TARGET":
                            if (!GameManager.instance.hitTarget) {
                                missed = true;
                                Miss();
                            }
                            break;
                    }
                }

                // switched based on scene name
                // we are gonna go through scene names and events
                // and determine if we should get a chance of getting a miss back
                switch (sceneName)
                {
                    case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                        if (!missed)
                            AddMissOnChance();
                        break;

                    case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                        if (GameManager.instance.currentEvent != GameManager.instance.bankShotEvent && !missed)
                            AddMissOnChance();
                        else if (GameManager.instance.currentEvent == GameManager.instance.bankShotEvent && GameManager.instance.didBankShot && !missed)
                            AddMissOnChance();
                        break;
                }

                // if we are in the party gamemode and have not started a new event
                if (sceneName == SceneNames.PARTY_GAMEMODE_SCENE_NAME && !startedNewEvent)
                {
                    // start a new event
                    GameManager.instance.NewEvent();
                    startedNewEvent = true;
                }

                // if we are not in the ramp shot event don't let the ball use gravity
                if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent)
                    ballBody.useGravity = false;

                // reset velocities on ball
                ballBody.velocity = Vector3.zero;
                ballBody.angularVelocity = Vector3.zero;

                // make the collider a trigger
                ballCollider.isTrigger = true;

                // switch based on sceneName
                switch (sceneName)
                {
                    // if we are in the classic gamemode reset ball position,
                    // enable ball hover, and enable the ball
                    case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                        ballBody.transform.position = ballSpawnPosition.position;
                        BallHover.instance.hover = true;
                        ball.SetActive(true);
                        break;
                    
                    // if we are in the party gamemode
                    case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                        // if we are not in the ramp shot event move the ball to the normal spawn position,
                        // enable the ball, and enable the hover
                        if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent) {
                            ballBody.transform.position = ballSpawnPosition.position;
                            BallHover.instance.hover = true;
                            ball.SetActive(true);
                        }
                        // if we are in the ramp shot event move the ball to the ramp shot spawn position
                        // and enable the ball
                        else if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent) {
                            ballBody.transform.position = rampShotEventBallSpawnPosition.position;
                            ball.SetActive(true);
                        }
                        break;
                }

                // make the ball's collider solid
                ballCollider.isTrigger = false; 

                // reset the ball's rotation
                ballBody.rotation = GameManager.instance.initialBallRotation;

                // clear the cupsHit list and reset the hitRamp bool
                BallCollision.instance.cupsHit.Clear();
                BallCollision.instance.hitRamp = false;

                // if we aren't in the ramp shot event enable the ball hover
                if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent)
                    ball.GetComponent<BallHover>().enabled = true;

                // reset ball mass
                BallGravityIncreaser.instance.currentMass = BallGravityIncreaser.instance.minMass;

                // reset bools
                startedNewEvent = false;
                BallClickController.instance.thrown = false;
                BallClickController.instance.canThrow = true;
                
                // reset timer
                GameManager.instance.addRampThrowTime = false;
                GameManager.instance.currentRampThrowTime = 0f;

                // reset timer
                respawnTimeWaited = 0f;

                // reset bool
                BallLife.instance.respawnBall = false;
            }
        }
    }

    void MoveBall(Transform finalPosition)
    {
        // make it so the cups can't collect the ball
        canCollect = false;

        // move the ball to the spawn position
        Vector3 pos = ball.transform.position;
        pos = Vector3.MoveTowards(ball.transform.position, finalPosition.position,
                ballMoveRespawnSpeed * Time.fixedDeltaTime);
        ballBody.MovePosition(pos);
    }

    void Miss()
    {
        // remove a miss, reset the streak, subtract from our earned coins, and update the missesLeft text
        GameManager.instance.missesLeft--;
        GameManager.instance.ResetStreak();
        GameManager.instance.RemoveEarnedCoins();
        GameManager.instance.UpdateMissesLeftText();
    }

    void AddMissOnChance()
    {   
        // 50% chance
        if (Random.Range(0, 2) > 0)
        {
            // add a miss and update the missesLeft text
            GameManager.instance.missesLeft++;
            GameManager.instance.UpdateMissesLeftText();
        }
    }

    public void RespawnBallAfterAd()
    {
        // add a miss and update the missesLeft text
        GameManager.instance.missesLeft++;
        GameManager.instance.UpdateMissesLeftText();

        // make the collider a trigger
        ballCollider.isTrigger = true;

        // switch based on sceneName
        switch (sceneName)
        {
            // if we are in the classic gamemode reset ball position,
            // enable ball hover, and enable the ball
            case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                ballBody.transform.position = ballSpawnPosition.position;
                BallHover.instance.hover = true;
                ball.SetActive(true);
                break;
                    
            // if we are in the party gamemode
            case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                // if we are not in the ramp shot event move the ball to the normal spawn position,
                // enable the ball, and enable the hover
                if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent) {
                    ballBody.transform.position = ballSpawnPosition.position;
                    BallHover.instance.hover = true;
                    ball.SetActive(true);
                }
                // if we are in the ramp shot event move the ball to the ramp shot spawn position
                // and enable the ball
                else if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent) {
                    ballBody.transform.position = rampShotEventBallSpawnPosition.position;
                    ball.SetActive(true);
                }
                break;
        }

        // make the ball's collider solid
        ballCollider.isTrigger = false; 

        // reset the ball's rotation
        ballBody.rotation = GameManager.instance.initialBallRotation;

        // clear the cupsHit list and reset the hitRamp bool
        BallCollision.instance.cupsHit.Clear();
        BallCollision.instance.hitRamp = false;

        // if we aren't in the ramp shot event enable the ball hover
        if (GameManager.instance.currentEvent != GameManager.instance.rampShotEvent)
            ball.GetComponent<BallHover>().enabled = true;

        // reset ball mass
        BallGravityIncreaser.instance.currentMass = BallGravityIncreaser.instance.minMass;

        // reset bools
        startedNewEvent = false;
        BallClickController.instance.thrown = false;
        BallClickController.instance.canThrow = true;
                
        // reset timer
        GameManager.instance.addRampThrowTime = false;
        GameManager.instance.currentRampThrowTime = 0f;

        // reset timer
        respawnTimeWaited = 0f;

        // reset bool
        BallLife.instance.respawnBall = false;
    }
}