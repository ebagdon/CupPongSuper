using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CupHitAnimation : MonoBehaviour
{   
    // bool for if we have been rewarded or not
    private bool rewarded;

    // variables for initial position
    private Vector3 posAtStart;
    private bool haveInitPos;

    // anim speeds
    private float upSpeed = 9.5f;
    private float leftRightSpeed = 20f;

    // distances for moving and deactivating
    private float upDistance = 6f;
    private float leftRightDistance = 40f;
    private float deactivateDistance = 39f;

    // delay time after the cup has been hit to start the anim
    private float delayTime;
    private float delayTimeThreshold = 0.1f;

    // bools for deciding which direction to go and if we have finished going up
    private bool left;
    private bool leftDecided;
    private bool upFinished;

    // the cup collector on this cup
    [HideInInspector] public CupCollector cupCollector;

    // our mesh colliders
    private MeshCollider outsideCol;
    private MeshCollider insideCol;
    private MeshCollider rimCol;

    private void Awake()
    {
        // the cupCollector on this cup
        cupCollector = gameObject.transform.GetChild(3).GetComponent<CupCollector>();

        // our mesh colliders
        outsideCol = gameObject.transform.GetChild(2).GetComponent<MeshCollider>();
        insideCol = gameObject.transform.GetChild(1).GetComponent<MeshCollider>();
        rimCol = gameObject.transform.GetChild(0).GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        // if we have started the anim
        if (cupCollector.startAnim)
        {   
            // if we have not been rewarded
            if (!rewarded)
            {
                // remove cups left, remove from the cups list, and add earned coins
                GameManager.instance.cupsLeft--;
                GameManager.instance.cupsList.Remove(gameObject);
                GameManager.instance.AddEarnedCoins();

                // set rewarded to true
                rewarded = true;
            }
            // get the initial cup pos if we haven't already
            if (haveInitPos == false)
            {
                posAtStart = transform.position;
                haveInitPos = true;
            }

            // once the delayTime passes the threshold start playing the animation
            delayTime += Time.deltaTime;
            if (delayTime >= delayTimeThreshold)
                PlayCupHitAnimation();
        }
    }

    // please note we do not have an actual animation
    // we are doing everything from code
    void PlayCupHitAnimation()
    {
        // disable all the colliders
        outsideCol.enabled = false;
        insideCol.enabled = false;
        rimCol.enabled = false;

        // decide to go left or right if we haven't already
        if (leftDecided)
        {
            DecideDirection();
            leftDecided = true;
        }

        // take the cup up the specified upDistance
        if (transform.position.y <= posAtStart.y + upDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, transform.position.y + upDistance,
                transform.position.z), upSpeed * Time.deltaTime);
        }
        else // we are done moving
        {
            // set the upFinished bool
            upFinished = true;
        }


        // if we have finished going up
        if (upFinished)
        {
            // move left if we want to go left
            if (left && transform.position.x <= posAtStart.x + leftRightDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x + leftRightDistance,
                    transform.position.y, transform.position.z),
                    leftRightSpeed * Time.deltaTime);
            }
            else // we are done moving
            {
                // if the lastMadeCup is this cup then set it to null
                if (BallCollision.instance.lastMadeCup == gameObject)
                    BallCollision.instance.lastMadeCup = null;
            }

            // move right if we want to go right
            if (left == false && transform.position.x >= posAtStart.x - leftRightDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x + leftRightDistance,
                    transform.position.y, transform.position.z),
                    -leftRightSpeed * Time.deltaTime);
            }
            else // we are done moving
            {
                // if the lastMadeCup is this cup then set it to null
                if (BallCollision.instance.lastMadeCup == gameObject)
                    BallCollision.instance.lastMadeCup = null;
            }
        }

        // remove this cup from the cupsList
        GameManager.instance.cupsList.Remove(transform.gameObject);

        // once we have moved past the deactivateDistance deactive this cup
        if (Mathf.Abs(transform.position.x) >= deactivateDistance)
            gameObject.SetActive(false);
    }

    void DecideDirection()
    {
        // if we are a center cup
        if (transform.position.x == 0f)
        {
            // 50% chance to go left
            if (Random.Range(0, 2) > 0)
                left = true;
        }

        // if we are a cup on the left side of the table go left
        if (transform.position.x > 0f)
            left = true;
    }
}