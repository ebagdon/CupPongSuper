using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGravityIncreaser : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static BallGravityIncreaser instance;

    // ball pops
    private Vector3 ballPos;

    // the height at which we start to slow down the y velocity of the ball
    private float maxHeight = 17f;

    // speeds at which to change the y velocity of the ball
    private float currentChangeSpeedRate = 32f;
    private float rateToChangeSpeedRate = 14f;
    private float maxChangeSpeedRate = 80f;

    // masses for ball throwing
    [HideInInspector] public float currentMass = 3f;
    [HideInInspector] public float minMass = 3f;
    private float maxMass = 25f;

    // speed to change mass at
    private float massChangeSpeed = 25f;

    // the ball's rigidbody
    private Rigidbody rBody;

    private void Awake()
    {     
        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;

        // get rigidbody and initialize it's mass
        rBody = GetComponent<Rigidbody>();
        rBody.mass = minMass;
    }

    private void Update()
    {
        // run functions
        DecreaseYVelAtHeight();
        IncreaseMass();
    }

    void DecreaseYVelAtHeight()
    {
        // if the ball pos is above the max hight
        if (ballPos.y > maxHeight)
        {
            // increase the rate at which we slow down the ball and clamp that rate
            currentChangeSpeedRate += rateToChangeSpeedRate * Time.deltaTime;
            currentChangeSpeedRate = Mathf.Clamp(currentChangeSpeedRate, 5f, maxChangeSpeedRate);

            // slow down our y velocity
            rBody.velocity = new Vector3(rBody.velocity.x,
                rBody.velocity.y - currentChangeSpeedRate * Time.deltaTime, rBody.velocity.z);
        }
    }

    void IncreaseMass()
    {   
        // if the ball has been thrown
        if (BallClickController.instance.thrown)
        {  
            // if our mass is less than or equal to the max mass, add to our mass
            if (currentMass <= maxMass)
                currentMass += massChangeSpeed * Time.deltaTime;
        }

        // set our mass
        rBody.mass = currentMass;
    }
}