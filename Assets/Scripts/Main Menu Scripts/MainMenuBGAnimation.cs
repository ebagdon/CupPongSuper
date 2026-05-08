using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBGAnimation : MonoBehaviour
{
    // camera and the camera position
    private Camera mainCam;
    private Vector3 cameraPos;

    // the point we rotate around and the speed we move around that point
    [SerializeField] private GameObject point;
    private float moveAroundSpeed = -18f;

    // start position and rotations for the canmera
    private float cameraXStartPoint = 10.31662f;
    private float cameraZStartPoint = -6.596168f;
    private float cameraStartRotationX = 23.67f;
    private float cameraStartRotationY = 255.626f;

    // when the camera gets to this x start the screen slide
    private float cameraXStartScreenSlidePoint = -8f;
    
    // end position
    private float cameraXEndPoint = -10.31662f;

    // our screen slide, start and end positions for screen slide
    [SerializeField] private Image screenSlideImage;
    [SerializeField] private Transform screenSlideStart, screenSlideEnd;

    // the speed our screen slide moves
    private float screenSlideSpeed = 2500f;

    // bools for screen slide
    private bool screenSlide;
    private bool imageDoneSliding;

    // array of balls and variables for our mainMenuBall
    [SerializeField] private BallName[] mainMenuBallsToChooseFrom;
    private GameObject mainMenuBall;
    private Quaternion initialBallRotation;
    private Rigidbody mainMenuBallBody;
    [SerializeField] private Transform ballSpawnPos;

    // if we have added force
    private bool forceAdded;

    // array of cups, list of cups, where the cups spawn, and the cup we are using for the animation
    [SerializeField] private CupName[] cupGroups;
    private List<GameObject> cupsList = new List<GameObject>();
    [SerializeField] private Transform[] startingCupPositions;
    private GameObject animationCup;

    // if we should play the animation and reset bools
    [HideInInspector] public bool playMainMenuAnim = true;
    private bool resetCameraCalled, calledResets;

    // our main menu controller
    private MainMenuController mainMenuController;

    private void Awake()
    {
        // our camera
        mainCam = Camera.main;

        // our main menu controller
        mainMenuController = GetComponent<MainMenuController>();
    }

    private void Start()
    {
        // get and spawn the main menu ball
        for (int i = 0; i < mainMenuBallsToChooseFrom.Length; i++)
        {
            if (mainMenuBallsToChooseFrom[i].ballName ==
                ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA))
            {
                initialBallRotation = mainMenuBallsToChooseFrom[i].gameObject.transform.rotation;
                mainMenuBall = mainMenuBallsToChooseFrom[i].gameObject;
                mainMenuBall.SetActive(true);

                // get the mainMenuBall's rigidbody
                mainMenuBallBody = mainMenuBall.GetComponent<Rigidbody>();
            }
        }

        // variable for the cup group that will be active in the main menu
        GameObject mainMenuCupGroup = null;

        // go through every cup and set its position
        for (int a = 0; a < cupGroups.Length; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                cupGroups[a].transform.GetChild(b).transform.position = startingCupPositions[b].position;
            }
            cupGroups[a].gameObject.SetActive(false);
        }

        // get and spawn the main menu cups
        for (int i = 0; i < cupGroups.Length; i++)
        {
            if (cupGroups[i].cupName ==
                ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA))
            {
                mainMenuCupGroup = cupGroups[i].gameObject;
                mainMenuCupGroup.SetActive(true);
            }
        }

        // THIS WILL ONLY WORK IF THE CUP IS THE ONLY OBJECT WITH THE ANIMATOR COMPONENT IN THE SCENE
        // initialize the animationCup
        animationCup = FindObjectOfType<Animator>().gameObject;
        animationCup.SetActive(false);
        animationCup.SetActive(true);
    }

    private void FixedUpdate()
    {
        // if we should play the animation
        if (playMainMenuAnim)
        {
            // play the animation
            mainMenuAnim();

            // if we need to do the screen slide reset the camera
            if (screenSlide)
                PlayScreenSlide();

            // if it is time to throw the ball throw it
            if (mainCam.transform.position.x >= -5f && mainCam.transform.position.x <= 5f && !forceAdded)
                ThrowBall();

            // set called resets to false
            calledResets = false;
        }
        else if (!playMainMenuAnim && !calledResets) // if it is time to reset everything
        {
            // reset everything
            ResetBall();
            ResetCup();
            ResetScreenSlide();

            // disable the ball and destroy the cup's animator component
            mainMenuBall.SetActive(false);
            Destroy(animationCup.GetComponent<Animator>());

            // set called resets to true
            calledResets = true;
        }
    }

    void mainMenuAnim()
    {
        // rotate around the point
        if (mainCam.transform.position.x > cameraXEndPoint)
            mainCam.transform.RotateAround(point.transform.position, Vector3.up, moveAroundSpeed * Time.deltaTime);

        // if it is time trigger the screen slide
        if (mainCam.transform.position.x <= cameraXStartScreenSlidePoint)
           screenSlide = true;
    }

    void PlayScreenSlide()
    {
        // move the screenSlide
        if (Vector3.Distance(screenSlideEnd.position, screenSlideImage.transform.position) > 0.01f && !imageDoneSliding)
        {
            screenSlideImage.transform.position = Vector3.MoveTowards(screenSlideImage.transform.position,
                screenSlideEnd.position, screenSlideSpeed * Time.deltaTime);
        }
        else // when we are done set it to true
            imageDoneSliding = true;

        // check if it is time to reset everything
        if (screenSlideImage.transform.position.x >= -100f && screenSlideImage.transform.position.x <= 100f
            && !resetCameraCalled)
        {
            // reset everthing
            ResetCameraPositionAndRotation();
            resetCameraCalled = true;
            ResetBall();
            ResetCup();
        }

        // if we are done with the screenSlide reset it
        if (imageDoneSliding)
        {
            ResetScreenSlide();
            screenSlide = false;
        }
    }

    void ResetCameraPositionAndRotation()
    {
        // reset camera position to starting position
        mainCam.transform.position = new Vector3(cameraXStartPoint,
            mainCam.transform.position.y, cameraZStartPoint);

        // reset camera rotation to starting rotation
        mainCam.transform.eulerAngles = new Vector3(cameraStartRotationX, cameraStartRotationY,
            mainCam.transform.rotation.z);
    }

    void ThrowBall()
    {
        // throw the ball
        mainMenuBallBody.useGravity = true;
        mainMenuBallBody.AddForce(0f, 1580f, -580f);
        mainMenuBallBody.angularVelocity = new Vector3(-5f, 0f, 0f);

        // set forceAdded to true
        forceAdded = true;
    }

    void ResetBall()
    {
        // activate ball
        mainMenuBall.SetActive(true);

        // stop gravity from effecting the ball
        mainMenuBallBody.useGravity = false;

        // reset velocities
        mainMenuBallBody.velocity = Vector3.zero;
        mainMenuBallBody.angularVelocity = Vector3.zero;

        // reset position and rotation
        mainMenuBall.transform.position = ballSpawnPos.position;
        mainMenuBall.transform.rotation = initialBallRotation;

        // set forceAdded to false
        forceAdded = false;
    }
    
    void ResetCup()
    {
        // reset position and play the idle animation for the cup
        animationCup.transform.position = startingCupPositions[4].transform.position;
        animationCup.GetComponent<Animator>().Play(AnimationNames.MAIN_MENU_CUP_IDLE_ANIM_NAME);
    }

    void ResetScreenSlide()
    {
        // reset positon
        screenSlideImage.transform.position = screenSlideStart.position;

        // reset bools
        resetCameraCalled = false;
        imageDoneSliding = false;
    }
}