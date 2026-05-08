using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHover : MonoBehaviour
{
    // positions
    private Vector3 originalPos;
    private Vector3 pos;

    // for timer
    private float currentTimeWaited;
    private float waitThreshold;

    // movement distance before slowing down
    private float movementDistance = 0.0001f;

    // variables for speed
    private float currentHoverSpeed;
    private float hoverSpeed = 0.4f;
    private float changeSpeed = 1.5f;

    // if we want to add speed
    private bool positive = true;

    private void Awake()
    {
        // randomize the time we wait before starting to hover
        waitThreshold = Random.Range(0f, 0.2f);

        // initialize variables
        originalPos = transform.position;
        currentHoverSpeed = hoverSpeed;
    }

    private void Update()
    {
        // add to the timer if we should still be waiting
        if (currentTimeWaited < waitThreshold)
            currentTimeWaited += Time.deltaTime;
    }

    private void FixedUpdate()
    {   
        // if we are done waiting hover
        if (currentTimeWaited >= waitThreshold)
            Hover();
    }

    void Hover()
    {
        // get pos
        pos = transform.position;

        /// if we are too high slow down
        if (pos.y >= originalPos.y + movementDistance)
        {
            currentHoverSpeed -= changeSpeed * Time.deltaTime;
            positive = false;
        }
        else
        {
            if (!positive)
            {
                // reverse the speed
                currentHoverSpeed = -hoverSpeed;
            }
        }

        /// if we are too low
        if (pos.y <= originalPos.y - movementDistance)
        {
            currentHoverSpeed += changeSpeed * Time.deltaTime;
            positive = true;
        }
        else
        {
            if (positive)
            {
                /// reverse the speed
                currentHoverSpeed = hoverSpeed;
            }
        }
        // add to the pos y
        pos.y += currentHoverSpeed * Time.deltaTime;

        // set pos
        transform.position = pos;
    }
}