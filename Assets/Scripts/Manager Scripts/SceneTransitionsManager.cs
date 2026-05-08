using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionsManager : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static SceneTransitionsManager instance;

    // canvas we need to disable when transitioning scenes and bools for the canvases that can be set to that canvas
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public bool skinsCanvasSetCanvas;

    // scene transition canvas
    [SerializeField] private Canvas sceneTransitionCanvas;

    // the bg image and it's position, start position, middle position, and end position
    [SerializeField] private Image bgImage;
    private Vector3 bgImagePos;
    [SerializeField] private Transform startPos, middlePos, endPos;
    
    // the speed we move the bg image at
    private float moveSpeed = 2600f;

    // scene name and scene that we are going to name
    private string sceneName;
    private string sceneParam;

    // parts of the scene transition
    private bool firstPartSceneTransition;
    [HideInInspector] public bool secondPartSceneTransition;

    // bool to know if we have initialized the ball click controller
    private bool initializedBallClickController;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    
    private void FixedUpdate()
    {
        // get the name of our scene
        sceneName = SceneManager.GetActiveScene().name;

        // SWITCH BASED ON SCENE NAME AND SET THE CANVAS ACCORDINGLY
        switch (sceneName)
        {
            case SceneNames.MAIN_MENU_SCENE_NAME:
                if (GameObject.Find(UIObjectNames.GAMEMODE_SELECT_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    canvas = GameObject.Find(UIObjectNames.GAMEMODE_SELECT_CANVAS_NAME).GetComponent<Canvas>();
                }
                else if (GameObject.Find(UIObjectNames.SKINS_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    canvas = GameObject.Find(UIObjectNames.SKINS_CANVAS_NAME).GetComponent<Canvas>();
                    skinsCanvasSetCanvas = true;
                }
                else if (GameObject.Find(UIObjectNames.INVENTORY_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    canvas = GameObject.Find(UIObjectNames.INVENTORY_CANVAS_NAME).GetComponent<Canvas>();
                }
                break;
            
            case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                if (GameObject.Find(UIObjectNames.GAMEOVER_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    canvas = GameObject.Find(UIObjectNames.GAMEOVER_CANVAS_NAME).GetComponent<Canvas>();
                }
                //else if (GameObject.Find(UIObjectNames.PAUSE_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    //canvas = GameObject.Find(UIObjectNames.PAUSE_CANVAS_NAME).GetComponent<Canvas>();
                   // skinsCanvasSetCanvas = false;
                //}
                break;

            case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                if (GameObject.Find(UIObjectNames.GAMEOVER_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    canvas = GameObject.Find(UIObjectNames.GAMEOVER_CANVAS_NAME).GetComponent<Canvas>();
                }
                //else if (GameObject.Find(UIObjectNames.PAUSE_CANVAS_NAME).GetComponent<Canvas>().enabled) {
                    //canvas = GameObject.Find(UIObjectNames.PAUSE_CANVAS_NAME).GetComponent<Canvas>();
                    //skinsCanvasSetCanvas = false;
                //}
                break;
        }

        // HANDLE THE CERTAIN PARTS OF THE SCENE TRANSITION
        // we add the sceneName == sceneParam or else the image will move off the
        // screen before the scene is fully loaded
        if (firstPartSceneTransition)
            HandleFromStartToMiddleSceneTransition();
        else if (secondPartSceneTransition && sceneName == sceneParam)
            HandleFromMiddleToEndSceneTransition();

        // if we are in a game and we have not initialized the ball click controller initialize it
        if (sceneName != SceneNames.MAIN_MENU_SCENE_NAME && !initializedBallClickController)
        {
            BallClickController.instance.canThrow = false;
            initializedBallClickController = true;
        }
    }

    void HandleFromStartToMiddleSceneTransition()
    {
        // get bg image pos
        bgImagePos = bgImage.transform.position;

        // reset bools
        skinsCanvasSetCanvas = false;

        // move the bg image to middle, when it's in the middle load the new scene
        // and start the second part scene transition
        if (Vector3.Distance(bgImagePos, middlePos.position) > 0.01f)
        {
            bgImagePos = Vector3.MoveTowards(bgImagePos, middlePos.position, moveSpeed * Time.deltaTime);
        }
        else {
            SceneManager.LoadScene(sceneParam);
            firstPartSceneTransition = false;
            secondPartSceneTransition = true;
        }

        // set the bg image pos
        bgImage.transform.position = bgImagePos;
    }

    void HandleFromMiddleToEndSceneTransition()
    {
        // get bg image pos
        bgImagePos = bgImage.transform.position;

        // move the bg image to the end
        if (Vector3.Distance(bgImagePos, endPos.position) > 0.01f)
        {
            bgImagePos = Vector3.MoveTowards(bgImagePos, endPos.position, moveSpeed * Time.deltaTime);
        }
        else if (sceneParam == SceneNames.MAIN_MENU_SCENE_NAME) { // if we are in the main menu
            // reset scene transitions
            bgImagePos = startPos.position;
            sceneParam = null;
            secondPartSceneTransition = false;

            // disable the scene transition canvas
            sceneTransitionCanvas.enabled = false;
        }
        else if (sceneParam != SceneNames.MAIN_MENU_SCENE_NAME) { // if we are in a gamemode scene
            // switch based on scene param
            switch (sceneParam)
            {   
                // if we are in the classic gamemode only enable ball throwing
                // if we have completed the tutorial
                case SceneNames.CLASSIC_GAMEMODE_SCENE_NAME:
                    if (DataManager.instance.GetBool(DataManager.instance.classicGamemodeTutorialDoneData))
                        BallClickController.instance.canThrow = true;
                    break;
                
                // if we are in the party gamemode only enable ball throwing
                // if we have completed the tutorial
                case SceneNames.PARTY_GAMEMODE_SCENE_NAME:
                    if (DataManager.instance.GetBool(DataManager.instance.partyGamemodeTutorialDoneData))
                        BallClickController.instance.canThrow = true;
                    break;
            }
            
            // reset scene transitions
            bgImagePos = startPos.position;
            sceneParam = null;
            secondPartSceneTransition = false;

            // disable the scene transition canvas
            sceneTransitionCanvas.enabled = false;
        }

        // set the bg image pos
        bgImage.transform.position = bgImagePos;
    }

    public void SetSceneTransitionToMainMenu()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // reset ad manager bools
        AdsManager.instance.loadingRewardedAd = false;
        AdsManager.instance.rewardedAdFailedLoaded = false;

        // start the scene transition
        sceneParam = SceneNames.MAIN_MENU_SCENE_NAME;
        firstPartSceneTransition = true;

        // disable and enable canvases
        if (canvas != null)
            canvas.enabled = false;
        sceneTransitionCanvas.enabled = true;
    }

    public void SetSceneTransitionToClassicGamemode()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // reset ad manager bools
        AdsManager.instance.loadingRewardedAd = false;
        AdsManager.instance.rewardedAdFailedLoaded = false;

        // start the scene transition
        sceneParam = SceneNames.CLASSIC_GAMEMODE_SCENE_NAME;
        firstPartSceneTransition = true;

        // disable and enable canvases
        if (canvas != null)
            canvas.enabled = false;
        sceneTransitionCanvas.enabled = true;
    }

    public void SetSceneTransitionToPartyGamemode()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // reset ad manager bools
        AdsManager.instance.loadingRewardedAd = false;
        AdsManager.instance.rewardedAdFailedLoaded = false;

        // start the scene transition
        sceneParam = SceneNames.PARTY_GAMEMODE_SCENE_NAME;
        firstPartSceneTransition = true;

        // disable and enable canvases
        if (canvas != null)
            canvas.enabled = false;
        sceneTransitionCanvas.enabled = true;
    }
}