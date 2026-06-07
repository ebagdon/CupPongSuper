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

    // interstitial ad id
    private string interstitialID = "id_goes_here";

    // rewarded ad id
    private string rewardedID = "id_goes_here";

    // game id
    private string gameId = "id_goes_here";

    // bool for if the ads sdk have been initialized
    private bool adsInitialized = false;
    
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

        // initialize the ads if they haven't been initialized already
        if (!adsInitialized && Application.internetReachability != NetworkReachability.NotReachable)
        {
            Advertisement.Initialize(gameId, false, this);
            adsInitialized = true;
        }
    }

    private void Update()
    {
        // initialize the ads if they haven't been initialized already
        if (!adsInitialized && Application.internetReachability != NetworkReachability.NotReachable)
        {
            Advertisement.Initialize(gameId, false, this);
            adsInitialized = true;
        }
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