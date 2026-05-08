using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallInCupSound : MonoBehaviour
{
    // scene name
    private string sceneName;

    private void Awake()
    {
        // get the scene name
        sceneName = SceneManager.GetActiveScene().name;
    }

    // called when something enters the collider on this object
    private void OnTriggerEnter(Collider collision)
    {
        // if it was the ball we collided with and if the ball isn't respawning
        if (collision.CompareTag(Tags.PLAYERS_BALL_TAG) && BallLife.instance.respawnBall == false)
        {
            // play the ball in cup sound
            SoundManager.instance.PlayBallInCupSound();
        }
    }
}