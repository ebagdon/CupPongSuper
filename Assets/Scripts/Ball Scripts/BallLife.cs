using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLife : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static BallLife instance;

    // length of the ray and layerMask for the ray
    private float rayLength = 0.6f;
    [SerializeField] private LayerMask tableLayer;

    // how long the ball can be on the table before respawning
    private float timeOnTable;
    private float timeOnTableThreshold = 1.7f;

    // how long the ball can be stuck before respawning
    private float ballStuckTime;
    private float ballStuckTimeThreshold = 1f;

    // variable to respawn the ball
    [HideInInspector] public bool respawnBall;

    // components
    private CapsuleCollider col;
    private Rigidbody rBody;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;

        // get components
        col = GetComponent<CapsuleCollider>();
        rBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // if the ball respawn manager can respawn the ball
        if (BallRespawnManager.instance.canRespawn)
        {
            // run our functions
            HandleBallOnTable();
            HandleBallStuck();
        }
    }

    void HandleBallOnTable()
    {
        // raycast to check for collision with the table
        if (Physics.Raycast(col.transform.position, -Vector3.up, rayLength, tableLayer))
        {
            // if we are in the ramp shot event and the ramp throw time timer isn't done 
            if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent && 
                GameManager.instance.currentRampThrowTime < GameManager.instance.rampThrowTimeThreshold)
            {
                return;
            }

            // add to the timeOnTable and when the timer is done set the bool to respawn the ball
            timeOnTable += Time.deltaTime;
            if (timeOnTable >= timeOnTableThreshold)
            {
                respawnBall = true;
            }
        }
        else // we are not colliding with the table
        {
            // reset timer
            timeOnTable = 0f;
        }
    }

    // for when our ball gets stuck in between the cups 
    void HandleBallStuck()
    {   
        // if the ball is not moving, it was thrown, and it's position is <= -9f
        if (rBody.velocity == Vector3.zero && BallClickController.instance.thrown && transform.position.z <= -9f)
        {
            // add to the timer and when it is done set the bool to respawn the ball and reset the timer
            ballStuckTime += Time.deltaTime;
            if (ballStuckTime >= ballStuckTimeThreshold)
            {
                respawnBall = true;
                ballStuckTime = 0f;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // check for collision with the floor and then set the bool to respawn the ball
        if (collision.gameObject.CompareTag(Tags.FLOOR_TAG))
            respawnBall = true;
    }
}