using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // public instance that can be accessed anywhere in our code
    public static GameOverManager instance;

    // canvases
    public Canvas gameOverCanvas;
    [SerializeField] private Canvas gameplayCanvas, settingsCanvas, languageSelectCanvas, inventoryCanvas;

    // bgs
    public GameObject bg;
    public GameObject adForExtraMissBG;

    // texts
    [SerializeField] private Text numberFromCupsTotalText, numberRemovedFromMissesTotalText, numberTotalText;
    [SerializeField] private Text youWinText, youLoseText;
    [SerializeField] private Text thisIsWinNumberText;

    // ad button, images and texts for the ad button
    [SerializeField] private Button adButton;
    [SerializeField] private Image[] adIcons;
    [SerializeField] private Text loadingText, needInternetText;

    // timer for showing the needInternetText and bool for if we should show it
    private float needInternetTextCurrentTime;
    private float needInternetTextTimeThreshold = 1f;
    [HideInInspector] public bool showNeedInternetText;

    // bools for if we should try to do a rewarded ad
    [HideInInspector] public bool firstLoss = true;
    private bool firstLossDealtWith;

    // bools
    private bool gameOverInitialized;
    private bool gameOverCalled;
    [HideInInspector] public bool respawnedAfterAd;
    private bool startedNewEvent;

    private void Awake()
    {
        // public instance that can be accessed anywhere in our code
        if (instance == null)
            instance = this;

        // fixes the text suddenly changing if in another language
        gameOverCanvas.enabled = true;

        // enable texts
        youWinText.enabled = true;
        youLoseText.enabled = true;
        thisIsWinNumberText.enabled = true;
    }

    private void Update()
    {
        // if the lost and it is our first loss
        if (GameManager.instance.loseGameOver && firstLoss && !firstLossDealtWith)
        {
            // check if we should try to play a rewarded ad and make sure the internet is reachable
            int randNum = Random.Range(0, 20);
            if (randNum <= 10 && Application.internetReachability != NetworkReachability.NotReachable) {
                adForExtraMissBG.SetActive(true);
            }
            else { // if we shouldn't then lose the game
                LoseGameOver();
                gameOverCalled = true;
            }

            // enable the game over canvas
            gameplayCanvas.enabled = false;
            gameOverCanvas.enabled = true;

            // set firstLossDealtWith to true
            firstLossDealtWith = true;
        }
        else if (GameManager.instance.loseGameOver && !firstLoss && !gameOverCalled) {
            // lose the game
            LoseGameOver();
            gameOverCalled = true;
        }
        
        // if we won and game over has not been called
        if (GameManager.instance.winGameOver && !gameOverCalled)
        {
            // enable the gameOverCanvas
            gameOverCanvas.enabled = true;

            // call game over
            WinGameOver();
            gameOverCalled = true;
        }

        // if the youWinText is enabled and the game over has not been initialized
        if (youWinText.enabled && !gameOverInitialized)
        {
            // fixes the text suddenly changing if in another language
            gameOverCanvas.enabled = false;

            // disable texts
            youWinText.enabled = false;
            youLoseText.enabled = false;
            thisIsWinNumberText.enabled = false;

            // set gameOverInitialized to true
            gameOverInitialized = true;
        }

        // if we are loading the rewarded ad, it has not failed, and the adForExtraMissBG is active
        if (AdsManager.instance.loadingRewardedAd && !AdsManager.instance.rewardedAdFailedLoaded && adForExtraMissBG.gameObject.activeInHierarchy)
        {
            // disable the ad button
            adButton.interactable = false;

            // if the ad icons are enabled disable them
            if (adIcons[0].enabled)
            {
                for (int i = 0; i < adIcons.Length; i++)
                    adIcons[i].enabled = false;
            }

            // enable the loading text
            loadingText.enabled = true;
        }  
        else if (AdsManager.instance.rewardedAdFailedLoaded && adForExtraMissBG.gameObject.activeInHierarchy) {
            // IF WE FAILED LOADING THE REWARDED AD AND
            // THE adForExtraMissBG IS ACTIVE

            // disable the ad button
            adButton.interactable = false;

            // if the ad icons are enabled disable them
            if (adIcons[0].enabled)
            {
                for (int i = 0; i < adIcons.Length; i++)
                    adIcons[i].enabled = false;
            }

            // reset ad manager bools
            AdsManager.instance.loadingRewardedAd = false;
            AdsManager.instance.rewardedAdFailedLoaded = false;

            // set showNeedInternetText to true
            showNeedInternetText = true;
        }

        // handle the need internet text
        if (showNeedInternetText)
            HandleNeedInternetText();
    }

    void HandleNeedInternetText()
    {
        // add to the timer
        needInternetTextCurrentTime += Time.deltaTime;

        // if the timer is not up and we are showing the loading text
        if (needInternetTextCurrentTime < needInternetTextTimeThreshold)
        {
            // disable and enable texts
            loadingText.enabled = false;
            needInternetText.enabled = true;
        }
        else if (needInternetTextCurrentTime >= needInternetTextTimeThreshold) { // if the timer is up
            // reset ad manager bools
            AdsManager.instance.loadingRewardedAd = false;
            AdsManager.instance.rewardedAdFailedLoaded = false;
            
            // reset timer
            needInternetTextCurrentTime = 0f;
            showNeedInternetText = false;
            
            // disable the bg and call lose game over if it hasn't been called already
            adForExtraMissBG.SetActive(false);
            if (!gameOverCalled) {
                LoseGameOver();
                gameOverCalled = true;
            }
        }
    }

    public void LoseGameOver()
    {
        // play the sound and play an ad
        SoundManager.instance.PlayEndOfGameCoinSound();
        AdsManager.instance.PlayInterstitialAd();

        // disable ball throwing
        BallClickController.instance.canThrow = false;

        // activate the bg
        bg.SetActive(true);

        // update texts
        UpdateText(numberFromCupsTotalText,
            GameManager.instance.totalCoinsEarnedFromCups.ToString());
        UpdateText(numberRemovedFromMissesTotalText,
            GameManager.instance.totalCoinsRemovedFromMisses.ToString());
        UpdateText(numberTotalText,
            GameManager.instance.coinsEarned.ToString());

        // update texts' alignment
        numberFromCupsTotalText.GetComponentInParent<DynamicTextImageAlignment>().
            UpdateAlignmentTotalCupCoinsEarned();
        numberRemovedFromMissesTotalText.GetComponentInParent<DynamicTextImageAlignment>().
            UpdateAlignmentTotalMissCoinsRemoved();
        numberTotalText.GetComponentInParent<DynamicTextImageAlignment>().
            UpdateAlignmentTotalCoinsEarned();

        // save the number of coins we have
        DataManager.instance.SaveInt(DataManager.instance.coinsData,
            DataManager.instance.GetInt(DataManager.instance.coinsData) + GameManager.instance.coinsEarned);

        // disable gameplayCanvas and enable youLoseText
        gameplayCanvas.enabled = false;
        youLoseText.enabled = true;
    }

    void WinGameOver()
    {
        // play the sound and play an ad
        SoundManager.instance.PlayEndOfGameCoinSound();
        AdsManager.instance.PlayInterstitialAd();

        // stop the ball from respawning
        BallRespawnManager.instance.canRespawn = false;

        // disable ball throwing
        BallClickController.instance.canThrow = false;

        // activate the bg
        bg.SetActive(true);

        // update texts
        UpdateText(numberFromCupsTotalText,
            GameManager.instance.totalCoinsEarnedFromCups.ToString());
        UpdateText(numberRemovedFromMissesTotalText,
            GameManager.instance.totalCoinsRemovedFromMisses.ToString());
        UpdateText(numberTotalText,
            GameManager.instance.coinsEarned.ToString());

        // update texts' alignment
        numberFromCupsTotalText.GetComponentInParent<DynamicTextImageAlignment>().
            UpdateAlignmentTotalCupCoinsEarned();
        numberRemovedFromMissesTotalText.GetComponentInParent<DynamicTextImageAlignment>().
            UpdateAlignmentTotalMissCoinsRemoved();
        numberTotalText.GetComponentInParent<DynamicTextImageAlignment>().
            UpdateAlignmentTotalCoinsEarned();

        // save the number of coins we have
        DataManager.instance.SaveInt(DataManager.instance.coinsData,
            DataManager.instance.GetInt(DataManager.instance.coinsData) + GameManager.instance.coinsEarned);

        // save the number of wins we have
        DataManager.instance.SaveInt(DataManager.instance.winsData, DataManager.instance.
            GetInt(DataManager.instance.winsData) + 1);

        // update text
        thisIsWinNumberText.GetComponentInParent<TextLanguageController>().enabled = false;
        thisIsWinNumberText.text = thisIsWinNumberText.text + DataManager.instance.
            GetInt(DataManager.instance.winsData);

        // disable gameplayCanvas and enable texts
        gameplayCanvas.enabled = false;
        youWinText.enabled = true;
        thisIsWinNumberText.enabled = true;
    }

    void UpdateText(Text text, string message)
    {
        // create a string builder and clear it
        StringBuilder sb = new StringBuilder();
        sb.Clear();
        
        // append to the string builder and set the text
        sb.Append(message);
        text.text = sb.ToString();
    }

    public void OpenSettings()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        gameOverCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    // back to our GameOverCanvas
    public void CloseSettings()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        settingsCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    public void OpenLanguageSelectMenu()
    {   
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        settingsCanvas.enabled = false;
        languageSelectCanvas.enabled = true;
    }

    // back to our SettingsCanvas
    public void CloseLanguageSelectMenu()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // disable and enable canvases
        languageSelectCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    public void PlayAdForExtraMiss()
    {
        // play button sound and play an ad
        SoundManager.instance.PlayButtonClickSound();
        AdsManager.instance.PlayRewardedAd();
    }

    public void RewardPlayerForAd()
    {
        // make it so we have not won or loss
        GameManager.instance.loseGameOver = false;
        GameManager.instance.winGameOver = false;

        // set firstLoss to false
        firstLoss = false;

        // respawn the ball for after the rewarded ad
        BallRespawnManager.instance.RespawnBallAfterAd();
        respawnedAfterAd = true;

        // disable the bg and enable the gameplayCanvas
        adForExtraMissBG.SetActive(false);
        gameplayCanvas.enabled = true;
    }

    public void CancelAdForExtraMiss()
    {
        // play button sound
        SoundManager.instance.PlayButtonClickSound();

        // reset ad manager bools
        AdsManager.instance.loadingRewardedAd = false;
        AdsManager.instance.rewardedAdFailedLoaded = false;

        // disable the bg and call lose game over if it hasn't been called already
        adForExtraMissBG.SetActive(false);
        if (!gameOverCalled)
        {
            LoseGameOver();
            gameOverCalled = true;
        }
    }
}