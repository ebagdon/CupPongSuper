using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class CupCollector : MonoBehaviour
{
    // scene name
    private string sceneName;

    // bools for anim and if we are a target
    [HideInInspector] public bool startAnim, isTarget;

    // our made cup feedback
    private MMFeedbacks madeCupFeedback;

    private void Awake()
    {
        // get scene name
        sceneName = SceneManager.GetActiveScene().name;

        // get our made cup feedback
        madeCupFeedback = GameObject.Find("MadeCupFeedback").GetComponent<MMFeedbacks>();
    }

    void Success()
    {
        // add to our streak
        GameManager.instance.AddToStreak();

        // play the made cup feedback
        madeCupFeedback.PlayFeedbacks();

        // if we haven't collided with this cup then add it to the cupsHit list
        for (int i = 0; i < BallCollision.instance.cupsHit.Count; i++)
        {
            if (transform.parent.name != BallCollision.instance.cupsHit[i].transform.parent.gameObject.name)
                BallCollision.instance.cupsHit.Add(this);
        }

        // go through all the cups we hit
        for (int i = 0; i < BallCollision.instance.cupsHit.Count; i++)
        {
            // start the cups animation
            BallCollision.instance.cupsHit[i].startAnim = true;
        }
                
        // check if we are in the targetEvent and we hit the target
        if (sceneName == SceneNames.PARTY_GAMEMODE_SCENE_NAME && GameManager.instance.currentEvent ==
            GameManager.instance.targetEvent && isTarget)
        {
            // set the GameManager's hitTarget to true
            GameManager.instance.hitTarget = true;
        }

        // make it so we aren't a target anymore, start our anim,
        // set the GameManager's cupsLeft to the cupsList count
        isTarget = false;
        startAnim = true;
        GameManager.instance.cupsLeft = GameManager.instance.cupsList.Count;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // making sure we collided with the ball, the ballRespawnManager can take the ball,
        // we are also making sure the ball is active
        if (collision.CompareTag(Tags.PLAYERS_BALL_TAG) && BallRespawnManager.instance.canCollect &&
            collision.gameObject.activeInHierarchy)
        {
            // disable the ball and stop it's movement
            collision.gameObject.SetActive(false);
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // switch based on our scene name
            switch (sceneName)
            {
                // check if we should count the shot that was made and if we should call success to add coins and stuff
                case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                    Success();
                    break;
                case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                    if (GameManager.instance.currentEvent == GameManager.instance.bankShotEvent && GameManager.instance.didBankShot)
                        Success();
                    else if (GameManager.instance.currentEvent == GameManager.instance.rampShotEvent)
                        Success();
                    else if (GameManager.instance.currentEvent == GameManager.instance.targetEvent && isTarget)
                        Success();
                    break;
            }
        }
    }
}