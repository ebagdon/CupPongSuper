using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHover : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static BallHover instance;

    // position variables
    private Vector3 originalPos;
    private Vector3 pos;

    // bool for if we can hover
    [HideInInspector] public bool hover = true;

    // variable for how far from original pos to move
    private float movementDistance = 0.0001f;

    // variables for hover speed and how fast to change that speed
    private float currentHoverSpeed;
    private float hoverSpeed = 2f;
    private float changeSpeed = 1.9f;

    // bool for if we are going up or down
    private bool positive = true;
    
    // components
    private Rigidbody ballBody;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;

        // initialize originalPos and currentHoverSpeed
        originalPos = GameObject.Find(ObjectNames.PLAYERS_BALL_SPAWN_POSITION_NAME).transform.position;
        currentHoverSpeed = hoverSpeed;

        // components
        ballBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // if we can hover
        if (hover)
            HoverBall();
    }

    void HoverBall()
    {
        // get pos
        pos = ballBody.transform.position;

        // if we have gone up to far
        if (pos.y >= originalPos.y + movementDistance)
        {   
            // decrease speed and set positive to false
            currentHoverSpeed -= changeSpeed * Time.fixedDeltaTime;
            positive = false;
        }
        else
        {   
            // if we are not supposed to be going up
            if (!positive)
            {
                // flip the speed
                currentHoverSpeed = -hoverSpeed;
            }
        }

        // if we have gone down to far
        if (pos.y <= originalPos.y - movementDistance)
        {
            // decrease speed and set positive to true
            currentHoverSpeed += changeSpeed * Time.fixedDeltaTime;
            positive = true;
        }
        else
        {
            // if we are supposed to be going up
            if (positive)
            {
                // flip the speed
                currentHoverSpeed = hoverSpeed;
            }
        }
        
        // add to the pos y
        pos.y += currentHoverSpeed * Time.fixedDeltaTime;

        // set pos
        ballBody.MovePosition(pos);
    }
}