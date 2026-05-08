using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    // public instance that can be accessed anywhere in my code
    public static AdsManager instance;

    // ids and current id variables for interstitial ads
    private string interstitial_IOS = "ad_id_goes_here";
    private string interstitial_ANDROID = "ad_id_goes_here";
    private string interstitialID;

    // ids and current id variables for rewarded ads
    private string rewarded_IOS = "ad_id_goes_here";
    private string rewarded_ANDROID = "ad_id_goes_here";
    private string rewardedID;

    // ids and current id variables for the game id
    private string gameId_IOS = "game_id_goes_here";
    private string gameId_ANDROID = "game_id_goes_here";
    private string gameId;
    
    // bools for loading the ad
    [HideInInspector] public bool loadingRewardedAd;
    [HideInInspector] public bool rewardedAdFailedLoaded;

    private void Awake()
    {
        // public instance that can be accessed anywhere in my code
        DontDestroyOnLoad(this);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        // set current ad IDS
        interstitialID = (Application.platform == RuntimePlatform.IPhonePlayer) ? interstitial_IOS : interstitial_ANDROID;
        rewardedID = (Application.platform == RuntimePlatform.IPhonePlayer) ? rewarded_IOS : rewarded_ANDROID;

        // set current game id and initialize the ads
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? gameId_IOS : gameId_ANDROID;
        Advertisement.Initialize(gameId, false, this);
    }

    public void PlayRewardedAd()
    {
        // load the ad
        loadingRewardedAd = true;
        Advertisement.Load(rewardedID, this);
    }

    public void PlayInterstitialAd()
    {
        // if we have not bought the no ads purchase
        if (DataManager.instance.GetBool(DataManager.instance.playAdsData))
        {
            // one in three chance
            int randNum = Random.Range(0, 3);
            if (randNum == 0)
            {
                // load the interstitial ad
                Advertisement.Load(interstitialID, this);
            }
        }
    }

    // CALLBACKS
    // CALLBACKS

    public void OnInitializationComplete() {}
    public void OnInitializationFailed (UnityAdsInitializationError error, string name) {}

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        if (adUnitId == interstitialID && GameOverManager.instance.gameOverCanvas.enabled && GameOverManager.instance.bg.gameObject.activeInHierarchy)
        {
            Advertisement.Show(interstitialID, this);
        }
        else if (adUnitId == rewardedID && GameOverManager.instance.gameOverCanvas.enabled && GameOverManager.instance.adForExtraMissBG.gameObject.activeInHierarchy) {
            Advertisement.Show(rewardedID, this);
        }

        // reset bools
        loadingRewardedAd = false;
        rewardedAdFailedLoaded = false;
    }
 
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        //Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.

        // if the ad that failed to load is the rewarded ad
        if (adUnitId == rewardedID)
        {
            // set ad failed loaded to true
            rewardedAdFailedLoaded = true;
        }
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        //Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }
    
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        // if the rewarded ad completes
        if (adUnitId == rewardedID)
        {
            // reward the player
            GameOverManager.instance.RewardPlayerForAd();

            // reset bools
            loadingRewardedAd = false;
            rewardedAdFailedLoaded = false;
        }
    }
}