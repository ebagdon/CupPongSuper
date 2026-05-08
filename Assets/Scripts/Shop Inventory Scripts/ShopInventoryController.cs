using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopInventoryController : MonoBehaviour
{
    // canvases and the canvas that initiated the preview mode
    [SerializeField] private Canvas skinsCanvas, inventoryCanvas, previewCanvas, shopCanvas;
    private Canvas canvasInitiatedPreview;

    // the coinsText
    [SerializeField] private Text coinsText;

    // all of the preview balls
    [SerializeField] private GameObject[] shopPreviewBalls;

    // bool for initialBallActivated
    private bool initialBallActivated;

    // list of ballNames and variables for the active ball
    private List<string> ballNames = new List<string>();
    private int activeBallIndex;
    private string activeBallName;

    // bool for ballPreviewing
    private bool ballPreviewing;

    // all of our cup groups
    [SerializeField] private GameObject[] cupGroups;

    // list of cupNames and variables for the active cup group
    private List<string> cupNames = new List<string>();
    private int activeCupIndex;
    private string activeCupName;

    // bool for cupPreviewing 
    private bool cupPreviewing;

    // scrolls rects
    [SerializeField] private ScrollRect shopBallSkinsScroll, shopCupSkinsScroll;
    [SerializeField] private ScrollRect inventoryBallSkinsScroll, inventoryCupSkinsScroll;

    // statuses for if we are looking at the balls or cups
    [HideInInspector] public string currentShopStatus;
    [HideInInspector] public string currentInventoryStatus;
    private string ballsStatus = "BALLS";
    private string cupsStatus = "CUPS";

    // images, texts, and buttons for the purchase prompt
    [SerializeField] private Image purchasePromtBGImage, yesButtonImage, noButtonImage;
    [SerializeField] private Button yesButton, noButton;
    [SerializeField] private Text purchasePromtText, yesText, noText;

    // buy more coins prompt
    [SerializeField] private GameObject buyMoreCoinsPrompt;

    // back buttons
    [SerializeField] private Button shopCanvasNormalBackButton, shopCanvasToSkinsCanvasBackButton;

    // purchase cost and the savedFileName
    private int purchaseCost;
    private string savedFileName;

    // purchase and inventory buttons
    private Button purchaseButton;
    private Button inventoryButton;

    // buttons we need to deactivate
    private Button[] buttonsToDeactivate;

    // bool for if we need to sort the inventory
    private bool needToSortInventory = true;

    // components
    private MoveCameraToShopView moveCameraToShopView;
    private PreviewDragLookAround previewDragLookAround;
    private InventorySorter inventorySorter;
    private SceneTransitionsManager sceneTransitionsManager;
    private MainMenuBGAnimation mainMenuBGAnimation;
    private MainMenuController mainMenuController;

    private void Awake()
    {
        // set the current statuses
        currentShopStatus = ballsStatus;
        currentInventoryStatus = ballsStatus;

        // disable button
        shopCanvasToSkinsCanvasBackButton.gameObject.SetActive(false);
        
        // get all the ball names and put them in a list
        for (int i = 0; i < shopPreviewBalls.Length; i++)
        {
            ballNames.Add(shopPreviewBalls[i].GetComponent<BallName>().ballName);
        }

        // get all the cup names and put them in a list
        for (int i = 0; i < cupGroups.Length; i++)
        {
            cupNames.Add(cupGroups[i].GetComponent<CupName>().cupName);
        }

        // get components
        moveCameraToShopView = GetComponent<MoveCameraToShopView>();
        previewDragLookAround = Camera.main.GetComponent<PreviewDragLookAround>();
        inventorySorter = GameObject.Find(ObjectNames.InventorySorter_NAME).GetComponent<InventorySorter>();
        sceneTransitionsManager = GameObject.Find(ObjectNames.SceneTransitionsManager_NAME).GetComponent<SceneTransitionsManager>();
        mainMenuBGAnimation = GameObject.Find(ObjectNames.MainMenuController_NAME).GetComponent<MainMenuBGAnimation>();
        mainMenuController = GameObject.Find(ObjectNames.MainMenuController_NAME).GetComponent<MainMenuController>();
    }

    private void Update()
    {
        // when we are done previewing in our inventory the ball will go back to our equipped ball
        if (inventoryCanvas.enabled && !ballPreviewing && ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA) != activeBallName)
        {
            // get the equippedBallName
            string equippedBallString = ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_BALL_STRING_DATA);

            // go through all the preview balls
            for (int i = 0; i < shopPreviewBalls.Length; i++) {
                // if the equippedBallString matches the ballNames[i]
                if (equippedBallString == ballNames[i]) {
                    // disable the preview ball and activate our equipped ball
                    shopPreviewBalls[activeBallIndex].SetActive(false);
                    shopPreviewBalls[i].SetActive(true);

                    // set the ball index and name of the active ball
                    activeBallIndex = i;
                    activeBallName = equippedBallString;
                    
                    // return
                    return;
                }
            }
        }

        // when we are done previewing in our inventory the cups will go back to our equipped cups
        if (inventoryCanvas.enabled && !cupPreviewing && ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA) != activeCupName)
        {
            // get the equippedCupName
            string equippedCupString = ShopInventoryDataManager.instance.GetString(ShopInventoryDataManager.instance.equipped_CUP_STRING_DATA);

            // go through all the cup groups
            for (int i = 0; i < cupGroups.Length; i++) {
                // if the equippedBallString matches the ballNames[i]
                if (equippedCupString == cupNames[i]) {
                    // disable the preview cups and activate our equipped cups
                    cupGroups[activeCupIndex].SetActive(false);
                    cupGroups[i].SetActive(true);

                    // set the cup index and name of the active cup
                    activeCupIndex = i;
                    activeCupName = equippedCupString;

                    // return
                    return;
                }
            }
        }
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

    public void OpenInventoryForBallSkins()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // if we need to sort the inventory sort it
        if (needToSortInventory)
        {
            inventorySorter.SortInventory();
            needToSortInventory = false;
        }

        // set the inventory status
        currentInventoryStatus = ballsStatus;

        // enable the ball skins scroll rect
        inventoryCupSkinsScroll.gameObject.SetActive(false);
        inventoryBallSkinsScroll.gameObject.SetActive(true);

        // tell main menu controller and scene transitions manager which canvas we came from 
        sceneTransitionsManager.skinsCanvasSetCanvas = false;
        mainMenuController.canvasActivatedInventory = mainMenuController.skinsCanvas;

        // disable and enable canvases
        skinsCanvas.enabled = false;
        inventoryCanvas.enabled = true;
    }

    public void OpenInventoryForCupSkins()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // if we need to sort the inventory sort it
        if (needToSortInventory)
        {
            inventorySorter.SortInventory();
            needToSortInventory = false;
        }

        // set the inventory status
        currentInventoryStatus = cupsStatus;

        // enable the cup skins scroll rect
        inventoryBallSkinsScroll.gameObject.SetActive(false);
        inventoryCupSkinsScroll.gameObject.SetActive(true);

        // tell main menu controller and scene transitions manager which canvas we came from 
        sceneTransitionsManager.skinsCanvasSetCanvas = false;
        mainMenuController.canvasActivatedInventory = mainMenuController.skinsCanvas;

        // disable and enable canvases
        skinsCanvas.enabled = false;
        inventoryCanvas.enabled = true;
    }

    public void ShopStatusToBalls()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // set the current shop status
        currentShopStatus = ballsStatus;

        // activate the ballSkins scroll rect
        shopCupSkinsScroll.gameObject.SetActive(false);
        shopBallSkinsScroll.gameObject.SetActive(true);
    }

    public void ShopStatusToCups()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // set the current shop status
        currentShopStatus = cupsStatus;

        // activate the cupSkins scroll rect
        shopBallSkinsScroll.gameObject.SetActive(false);
        shopCupSkinsScroll.gameObject.SetActive(true);
    }

    public void InventoryStatusToBalls()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // set the current inventory status
        currentInventoryStatus = ballsStatus;

        // activate the ballSkins scroll rect
        inventoryCupSkinsScroll.gameObject.SetActive(false);
        inventoryBallSkinsScroll.gameObject.SetActive(true);
    }

    public void InventoryStatusToCups()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // set the current inventory status
        currentInventoryStatus = cupsStatus;

        // activate the cupSkins scroll rect
        inventoryBallSkinsScroll.gameObject.SetActive(false);
        inventoryCupSkinsScroll.gameObject.SetActive(true);
    }

    public void OpenPreviewForBall()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // set ballPreviewing to true
        ballPreviewing = true;

        // name for the active ball
        string activeName = "";

        // if the skins canvas is enabled
        if (skinsCanvas.enabled)
        {
            // set the activeName to the ball we are trying to preview
            activeName = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<ShopBallSkinName>().ballSkinName;
        }
        else if (inventoryCanvas.enabled) { // if the inventory canvas is enabled
            // set the activeName to the ball we are trying to preview
            activeName = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<BallSkinInventoryInfo>().ballSkinName;
        }

        // if the activeName is not equal to the name of the ball that is already active
        if (activeName != activeBallName)
        {
            // go through all the preview balls
            for (int i = 0; i < shopPreviewBalls.Length; i++) {
                // if the active name is equal to ballNames[i]
                if (activeName == ballNames[i]) {
                    // activate the ball we are trying to preview
                    shopPreviewBalls[activeBallIndex].SetActive(false);
                    shopPreviewBalls[i].SetActive(true);

                    // set the ball index and name of the active ball
                    activeBallIndex = i;
                    activeBallName = activeName;
                }
            }
        }
        
        // enable the preview drag look around
        previewDragLookAround.enabled = true;

        // find out which canvas we were in when we initiated the preview
        if (skinsCanvas.enabled)
            canvasInitiatedPreview = skinsCanvas;
        else if (inventoryCanvas.enabled)
            canvasInitiatedPreview = inventoryCanvas;

        // disable and enable canvases
        canvasInitiatedPreview.enabled = false;
        previewCanvas.enabled = true;
    }

    public void OpenPreviewForCup()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();
  
        // set cupPreviewing to true
        cupPreviewing = true;

        // name for the active ball
        string activeName = "";

        // if the skins canvas is enabled
        if (skinsCanvas.enabled)
        {
            // set the activeName to the ball we are trying to preview
            activeName = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<ShopCupSkinName>().cupSkinName;
        }
        else if (inventoryCanvas.enabled) { // if the inventory canvas is enabled
            // set the activeName to the ball we are trying to preview
            activeName = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<CupSkinInventoryInfo>().cupSkinName;
        }

        // if the activeName is not equal to the name of the cups that are already active
        if (activeName != activeCupName)
        {
            // go through all of the cup groups
            for (int i = 0; i < cupGroups.Length; i++) {
                // if the active name is equal to cupNames[i]
                if (activeName == cupNames[i]) {
                    // activate the cup group we are trying to preview
                    cupGroups[activeCupIndex].SetActive(false);
                    cupGroups[i].SetActive(true);

                    // set the cup group index and name of the cup group
                    activeCupIndex = i;
                    activeCupName = activeName;
                }
            }
        }
        
        // enable the preview drag look around
        previewDragLookAround.enabled = true;

        // find out which canvas we were in when we initiated the preview
        if (skinsCanvas.enabled)
            canvasInitiatedPreview = skinsCanvas;
        else if (inventoryCanvas.enabled)
            canvasInitiatedPreview = inventoryCanvas;

        // disable and enable canvases
        canvasInitiatedPreview.enabled = false;
        previewCanvas.enabled = true;
    }
    
    public void ClosePreview()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable the preview drag look around and enable the move camera to shop view script
        previewDragLookAround.enabled = false;
        moveCameraToShopView.enabled = true;

        // set previewing bools to false
        ballPreviewing = false;
        cupPreviewing = false;

        // disable and enable canvases
        previewCanvas.enabled = false;
        canvasInitiatedPreview.enabled = true;

        // set canvasInitiatedPreview to null
        canvasInitiatedPreview = null;
    }

    public void PurchasePrompt()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // get the current clicked/touched on object
        GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

        // get the ChooseInventoryOrPurchaseButton component
        ChooseInventoryOrPurchaseButton chooseInventoryOrPurchaseButton =
            currentSelectedGameObject.transform.parent.GetComponent<ChooseInventoryOrPurchaseButton>();
        
        // get the savedFileName, the purchase button, and the inventory button
        savedFileName = chooseInventoryOrPurchaseButton.savedFileName;
        purchaseButton = chooseInventoryOrPurchaseButton.purchaseButton;
        inventoryButton = chooseInventoryOrPurchaseButton.inventoryButton;

        // get the purchase cost
        purchaseCost = currentSelectedGameObject.GetComponent<Cost>().cost;

        // if we have enough coins to buy it
        if (purchaseCost <= DataManager.instance.GetInt(DataManager.instance.coinsData))
        {
            // disable all of the buttons
            buttonsToDeactivate = FindObjectsOfType<Button>();
            for (int i = 0; i < buttonsToDeactivate.Length; i++)
                buttonsToDeactivate[i].enabled = false;

            // enable the purchase prompt's images, buttons, and texts
            purchasePromtBGImage.enabled = true;
            yesButtonImage.enabled = true;
            noButtonImage.enabled = true;
            yesButton.enabled = true;
            noButton.enabled = true;
            purchasePromtText.enabled = true;
            yesText.enabled = true;
            noText.enabled = true;

            // disables the scroll rects' scrolling
            shopBallSkinsScroll.vertical = false;
            shopCupSkinsScroll.vertical = false;
        }
        else // we don't have enough coins to buy it
        {
            // reset to default values
            savedFileName = "";
            purchaseCost = 0;

            // get rid of the purchaseButton and inventoryButton references
            purchaseButton = null;
            inventoryButton = null;

            // disable all of the buttons
            buttonsToDeactivate = FindObjectsOfType<Button>();
            for (int i = 0; i < buttonsToDeactivate.Length; i++)
                buttonsToDeactivate[i].enabled = false;

            // handle back buttons
            shopCanvasNormalBackButton.gameObject.SetActive(false);
            shopCanvasToSkinsCanvasBackButton.gameObject.SetActive(true);

            // enable the buy more coins prompt
            buyMoreCoinsPrompt.SetActive(true);

            // disable the scroll rects' scrolling
            shopBallSkinsScroll.vertical = false;
            shopCupSkinsScroll.vertical = false;
        }
    }

    public void YesToPurchasePrompt()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // enable all the buttons
        for (int i = 0; i < buttonsToDeactivate.Length; i++)
            buttonsToDeactivate[i].enabled = true;

        // disable the purchase prompt's images, buttons, and texts
        purchasePromtBGImage.enabled = false;
        yesButtonImage.enabled = false;
        noButtonImage.enabled = false;
        yesButton.enabled = false;
        noButton.enabled = false;
        purchasePromtText.enabled = false;
        yesText.enabled = false;
        noText.enabled = false;

        // disable the purchase and inventory button
        purchaseButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(true);

        // make it so you can scroll again
        shopBallSkinsScroll.vertical = true;
        shopCupSkinsScroll.vertical = true;

        // subtracts the cost from out current coins
        DataManager.instance.SaveInt(DataManager.instance.coinsData,
            DataManager.instance.GetInt(DataManager.instance.coinsData) - purchaseCost);
        UpdateCoinsText();

        // make it so we own the skin
        ShopInventoryDataManager.instance.SaveOwnedData(savedFileName, true);

        // make it so we need to sort the inventory
        needToSortInventory = true;

        // reset to default values
        savedFileName = "";
        purchaseCost = 0;
        
        // get rid of the purchaseButton and inventoryButton references
        purchaseButton = null;
        inventoryButton = null;
    }

    public void NoToPurchasePrompt()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // enable all the buttons
        for (int i = 0; i < buttonsToDeactivate.Length; i++)
            buttonsToDeactivate[i].enabled = true;

        // disable the purchase prompt's images, buttons, and texts
        purchasePromtBGImage.enabled = false;
        yesButtonImage.enabled = false;
        noButtonImage.enabled = false;
        yesButton.enabled = false;
        noButton.enabled = false;
        purchasePromtText.enabled = false;
        yesText.enabled = false;
        noText.enabled = false;

        // make it so you can scroll again
        shopBallSkinsScroll.vertical = true;
        shopCupSkinsScroll.vertical = true;

        // reset to default values
        savedFileName = "";
        purchaseCost = 0;
        
        // get rid of the purchaseButton and inventoryButton references
        purchaseButton = null;
        inventoryButton = null;
    }

    public void YesToBuyMoreCoinsPrompt()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable the prompt
        buyMoreCoinsPrompt.SetActive(false);

        // enable all the buttons
        for (int i = 0; i < buttonsToDeactivate.Length; i++)
            buttonsToDeactivate[i].enabled = true;

        // make it so we can scroll again
        shopBallSkinsScroll.vertical = true;
        shopCupSkinsScroll.vertical = true;

        // disable and enable canvases
        skinsCanvas.enabled = false;
        shopCanvas.enabled = true;
    }

    public void NoToBuyMoreCoinsPrompt()
    {
        // play the the button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable the prompt
        buyMoreCoinsPrompt.SetActive(false);

        // enable all the buttons
        for (int i = 0; i < buttonsToDeactivate.Length; i++)
            buttonsToDeactivate[i].enabled = true;

        // make it so we can scroll again
        shopBallSkinsScroll.vertical = true;
        shopCupSkinsScroll.vertical = true;
    }

    public void CloseShop()
    {
        // play the button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        shopCanvas.enabled = false;
        skinsCanvas.enabled = true;
    }
}