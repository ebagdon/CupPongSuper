using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsBall : MonoBehaviour
{
    // rotations, rotating speeds, and bool for if we should start rotating
    [SerializeField] private Transform defaultCameraPosRot, rampShotEventCameraPosRot;
    private float lookRotateSpeed = 2f;
    private float resetRotateSpeed = 4f;
    [HideInInspector] public bool startRotating;

    // our ball and the ball's rigidbody
    private GameObject ball;
    private Rigidbody ballBody;

    private void Start()
    {   
        // get the ball and it's rigidbody
        ball = BallCollision.instance.gameObject;
        ballBody = ball.GetComponent<Rigidbody>();
    }

    private void Update()
    {   
        // determine if we should allow the camera to rotate towards the ball or not
        if (ball.activeSelf && ShouldStartRotating()) {
            startRotating = true;
        } else if (BallHover.instance.hover) {
            startRotating = false;
        }

        // if we should start rotating and we have thrown the ball
        if (startRotating && !BallLife.instance.respawnBall && BallClickController.instance.thrown)
        {
            // rotate the camera to look at the ball
            Rotate();
        }
        else { // if NONE of the if statements passed that means we need to reset the camera's rotation.
            // determine the target rotation
            Quaternion targetRot;
            if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent) {
                targetRot = rampShotEventCameraPosRot.rotation;
            }
            else {
                targetRot = defaultCameraPosRot.rotation;
            }

            // set the camera's rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, resetRotateSpeed * Time.deltaTime);
            if (transform.rotation == targetRot) {
                startRotating = false;
            }
        }
    }

    void Rotate()
    {
        // Calculate the direction and distance to the target
        Vector3 direction = ball.transform.position - transform.position;
        float distance = direction.magnitude;

        // Calculate the angle to rotate the camera towards the target
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Calculate the rotation speed based on how far we need to rotate the camera
        float rotationSpeed = lookRotateSpeed * Mathf.Clamp(distance / 45f, 0.5f, 1f);

        // Rotate the camera towards the target
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    bool ShouldStartRotating()
    {   
        // get the ball's screen space position and determine if it has moved
        // far enough to start rotating the camera
        Vector2 ballScreenSpacePos = Camera.main.WorldToScreenPoint(ball.transform.position);
        if (ballScreenSpacePos.y >= (float)Screen.height / 2.1f)
        {
            return true;
        }
        else if (ballScreenSpacePos.y <= (float)Screen.height / 3f && ballBody.velocity.y < -1f) {
            if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent && ballBody.velocity.z <= 0)
                return false;

            return true;
        }
        else if (ballScreenSpacePos.x >= (float)Screen.width / 1.5f) {
            return true;
        }
        else if (ballScreenSpacePos.x <= (float)Screen.width / 3f) {
            return true;
        }

        // return false if none of the if statements executed
        return false;
    }
}