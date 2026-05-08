using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToShopView : MonoBehaviour
{
    // our preview ball
    [SerializeField] private GameObject previewBall;
    
    // our camera and it's pos
    private Camera mainCam;
    private Vector3 camPos;
    
    // the transform for the camera in the shop
    [SerializeField] private Transform shopCamPosRot;

    // speeds to move and rotate the camera
    private float moveCamSpeed = 25f;
    private float rotateCamSpeed;
    private float toShopRotateCamSpeed = 145f;
    private float insideShopRotateCamSpeed = 625f;

    // bool for if we are moving inside the shop
    private bool movingInShopStatus;

    // bools for moving and rotating the camera
    private bool moveDone;
    private bool rotateDone;

    // main menu bg animation
    private MainMenuBGAnimation mainMenuBGAnimation;

    private void Awake()
    {
        // get our camera and main menu bg animation
        mainCam = Camera.main;
        mainMenuBGAnimation = GameObject.Find(ObjectNames.MainMenuController_NAME).GetComponent<MainMenuBGAnimation>();
    }

    private void FixedUpdate()
    {
        // if we are not playing the main menu bg animation
        if (!mainMenuBGAnimation.playMainMenuAnim)
        {
            // get cam pos
            camPos = mainCam.transform.position;

            // move the camera
            if (Vector3.Distance(camPos, shopCamPosRot.position) > 0.01f) {
                camPos = Vector3.MoveTowards(camPos, shopCamPosRot.position, moveCamSpeed * Time.deltaTime);
            }
            else {
                moveDone = true;
            }

            // set the camera position
            mainCam.transform.position = camPos;

            // set camera rotate speed
            if (!movingInShopStatus)
                rotateCamSpeed = toShopRotateCamSpeed;
            else
                rotateCamSpeed = insideShopRotateCamSpeed;

            // rotate the camera
            mainCam.transform.rotation = Quaternion.RotateTowards(mainCam.transform.rotation, shopCamPosRot.rotation, rotateCamSpeed * Time.deltaTime);

            // check if we are done rotating the camera
            if (mainCam.transform.rotation == shopCamPosRot.rotation) {
                rotateDone = true;
            }
        }

        // if we are done moving and rotating the camera
        if (moveDone && rotateDone) {
            // reset bools
            moveDone = false;
            rotateDone = false;

            // set moving in shop to true
            movingInShopStatus = true;

            // disable the script
            this.enabled = false;
        }
    }
}