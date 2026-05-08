using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// only use iOS if we are on iOS
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class MainMenuController : MonoBehaviour
{
    // canvases
    [SerializeField] private Canvas mainMenuCanvas, settingsCanvas, languagesSelectCanvas,
        ballThrowTypeSelectCanvas, gamemodeSelectCanvas, shopCanvas, inventoryCanvas;
        
    // skins canvas
    public Canvas skinsCanvas;

    // the canvas that we were last in before going into the inventory canvas
    [HideInInspector] public Canvas canvasActivatedInventory;

    // position of the gamemodeSelectCanvas
    [SerializeField] private RectTransform gamemodeSelectCanvasContent;
    
    // coinsText and leaderboard button texts
    [SerializeField] private Text coinsText;

    // variables for the description of the pullback ball throw mode
    [SerializeField] private GameObject pullbackModeDescriptionBG;
    private float currentPullbackModeDescriptionTimeShown;
    private float currentPullbackModeDescriptionTimeShownThreshold = 7f;
    private bool pullbackModeDescriptionActive;

    // reset tutorials prompt
    [SerializeField] private GameObject resetTutorialsPrompt;

    [SerializeField] private GameObject initialShopPreviewedBall;

    // components
    private SceneTransitionsManager sceneTransitionsManager;
    private InventorySorter inventorySorter;
    private MainMenuBGAnimation mainMenuBGAnimation;

    private void Awake() 
    {
        // initialize variable
        canvasActivatedInventory = null;

        // get components
        sceneTransitionsManager = GameObject.Find(ObjectNames.SceneTransitionsManager_NAME).GetComponent<SceneTransitionsManager>();
        inventorySorter = GameObject.Find(ObjectNames.InventorySorter_NAME).GetComponent<InventorySorter>();
        mainMenuBGAnimation = GetComponent<MainMenuBGAnimation>();
    }

    private void Update()
    {   
        // if an iOS user has played for more than 10 minutes and
        // hasn't been asked to review our app ask them
        if (DataManager.instance.GetFloat(DataManager.instance.timePlayedData) / 60f >= 10f &&
            !DataManager.instance.GetBool(DataManager.instance.askedToReviewAppData))
        {
            #if UNITY_IOS
                Device.RequestStoreReview();
                DataManager.instance.SaveBool(DataManager.instance.askedToReviewAppData, true);
            #endif
        }

        // handle our description for our pullback ball throw mode
        HandlePullbackModeDescription();
    }

    void UpdateCoinsText()
    {
        // create a string builder and clear it
        StringBuilder sb = new StringBuilder();
        sb.Clear();

        // get the amount of coins we have and create some bools for how to display it
        float coins = (int)DataManager.instance.GetInt(DataManager.instance.coinsData);
        bool displayHundredThousand = false;
        bool displayMillion = false;

        // display the coins in hundred thousands
        if (coins >= 100000 && coins <= 999999)
        {   
            coins /= 1000;
            coins = Mathf.Floor(coins);

            displayHundredThousand = true;
        }
        else if (coins >= 1000000) { // display the coins in millions
            coins /= 1000000;
            coins = Mathf.Floor(coins);

            displayMillion = true;
        }

        // set the string builder
        if (displayHundredThousand)
            sb.Append(coins.ToString("F0") + "k");
        else if (displayMillion)
            sb.Append(coins.ToString("F0") + "m");
        else
            sb.Append(coins);

        // set the coins text
        coinsText.text = sb.ToString();
    }

    void HandlePullbackModeDescription()
    {
        // if the description is currently shown
        if (pullbackModeDescriptionActive)
        {
            // add to the timer
            currentPullbackModeDescriptionTimeShown += Time.deltaTime;

            // if the timer is up
            if (currentPullbackModeDescriptionTimeShown >= currentPullbackModeDescriptionTimeShownThreshold)
            {
                // disable the description
                pullbackModeDescriptionBG.SetActive(false);
                pullbackModeDescriptionActive = false; 

                // reset the timer
                currentPullbackModeDescriptionTimeShown = 0f;
            }
        }
        else // if the description is not active
        {   
            // reset the timer
            currentPullbackModeDescriptionTimeShown = 0f;
        }
    }

    #region MENUS

    public void OpenClosePullbackDescription()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // if it is closed open it
        if (!pullbackModeDescriptionActive)
        {
            pullbackModeDescriptionBG.SetActive(true);
            pullbackModeDescriptionActive = true;
        }
        else // if it is open disable it
        {
            pullbackModeDescriptionBG.SetActive(false);
            pullbackModeDescriptionActive = false; 
        }
    }

    public void OpenSettingsMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // if the prompt is open disable it
        if (resetTutorialsPrompt.activeInHierarchy)
            resetTutorialsPrompt.SetActive(false);

        // disable and enable canvases
        mainMenuCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    public void OpenResetTutorialsPrompt()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // check if both tutorials have not been completed if so return
        if (!DataManager.instance.GetBool(DataManager.instance.classicGamemodeTutorialDoneData) &&
            !DataManager.instance.GetBool(DataManager.instance.partyGamemodeTutorialDoneData))
        {
            return;
        }

        // get all the buttons and disable them
        Button[] buttons = FindObjectsOfType<Button>();
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].enabled = false;

        // activate the prompt
        resetTutorialsPrompt.SetActive(true);
    }

    public void YesToResetTutorialsPrompt()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // reset tutorials
        DataManager.instance.SaveBool(DataManager.instance.classicGamemodeTutorialDoneData, false);
        DataManager.instance.SaveBool(DataManager.instance.partyGamemodeTutorialDoneData, false);

        // close prompt
        resetTutorialsPrompt.SetActive(false);

        // get all currently shown buttons and enable them
        Button[] buttons = FindObjectsOfType<Button>();
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].enabled = true;
    }

    public void NoToResetTutorialsPrompt()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // close prompt
        resetTutorialsPrompt.SetActive(false);

        // get all currently shown buttons and enable them
        Button[] buttons = FindObjectsOfType<Button>();
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].enabled = true;
    }

    // for the back button in the settings menu
    public void CloseSettingsMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        settingsCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }

    public void OpenLanguageSelectMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        settingsCanvas.enabled = false;
        languagesSelectCanvas.enabled = true;
    }

    // for the back button in the language select menu
    // this back button will take you back to the settings menu
    public void CloseLanguageSelectMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        languagesSelectCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    public void OpenGamemodeSelectMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable canvas
        mainMenuCanvas.enabled = false;
        
        // set anchors
        gamemodeSelectCanvasContent.anchorMin = new Vector2(gamemodeSelectCanvasContent.anchorMin.x, 0.5f);
        gamemodeSelectCanvasContent.anchorMax = new Vector2(gamemodeSelectCanvasContent.anchorMax.x, 0.5f);

        // reset anchored position
        gamemodeSelectCanvasContent.anchoredPosition = Vector2.zero;
    }

    public void CloseGamemodeSelectMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // set anchors
        gamemodeSelectCanvasContent.anchorMin = new Vector2(gamemodeSelectCanvasContent.anchorMin.x, 1f);
        gamemodeSelectCanvasContent.anchorMax = new Vector2(gamemodeSelectCanvasContent.anchorMax.x, 1f);

        // set anchored position
        gamemodeSelectCanvasContent.anchoredPosition = new Vector2(0f, 1389f);

        // enable canvas
        mainMenuCanvas.enabled = true;
    }

    public void OpenSkins()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // stop playing our main menu background animation
        mainMenuBGAnimation.playMainMenuAnim = false;

        // update our coins text in the shop
        UpdateCoinsText();

        // activate object
        initialShopPreviewedBall.SetActive(true);
        
        // disable and enable canvases
        mainMenuCanvas.enabled = false;
        skinsCanvas.enabled = true;
    }

    public void OpenShop()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        mainMenuCanvas.enabled = false;
        shopCanvas.enabled = true;
    }

    public void CloseShop()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        shopCanvas.enabled = false;
        mainMenuCanvas.enabled = true;
    }

    public void OpenInventory()
    {   
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // stop playing our main menu background animation and sort the inventory
        mainMenuBGAnimation.playMainMenuAnim = false;
        inventorySorter.SortInventory();

        // set the canvas we came from to canvasActivatedInventory
        canvasActivatedInventory = mainMenuCanvas;

        // disable and enable canvases
        mainMenuCanvas.enabled = false;
        inventoryCanvas.enabled = true;
    }

    public void CloseInventory()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // diable canvas
        inventoryCanvas.enabled = false;

        // if we came from main menu set the scene transition back to main menu
        if (canvasActivatedInventory == mainMenuCanvas)
            sceneTransitionsManager.SetSceneTransitionToMainMenu();
        else if (canvasActivatedInventory == skinsCanvas) // if we came from skinsCanvas
        {
            // enable skins canvas and set canvasActivatedInventory to null
            skinsCanvas.enabled = true;
            canvasActivatedInventory = null;
        }
    }

    public void OpenBallThrowTypeSelect()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        settingsCanvas.enabled = false;
        ballThrowTypeSelectCanvas.enabled = true;
    }

    public void CloseBallThrowTypeSelect()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        ballThrowTypeSelectCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    #endregion
}