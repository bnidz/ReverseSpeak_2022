using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;


public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
 
    public void Init()
    {   
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId =  "Rewarded_iOS";
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        //string gameId = "4929786";
        //bool testMode = true;
 

        Advertisement.Initialize ("4929786", false);
    
        //Disable the button until the ad is ready to show:
       // _showAdButton.interactable = false;
    }
 
    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
 
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
 
        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            Debug.Log( "Configure the button to call the ShowAd() method when clicked:");
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }
 
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        if(Components.c.settings.debug_ads_off)
        {
                Components.c.settings.thisPlayer.current_Hearts += Components.c.settings.thisConfigs.ad_heart_reward;
                Components.c.settings.thisPlayer.current_Skips += Components.c.settings.thisConfigs.ad_skip_reward;
                return;
            
        }
        // Disable the button:
      //  _showAdButton.interactable = false;
        // Then show the ad:
        Components.c.settings.lastShields = false;
        Advertisement.Show(_adUnitId, this);
    }

    public void ShowAd_sheilds()
    {
        if(Components.c.settings.debug_ads_off)
        {
                Components.c.settings.thisPlayer.shield_count += 3;
                Components.c.shieldButton.ShieldButtonPress();
            return;
        }
        Components.c.settings.lastShields = true;
        // Disable the button:
      //  _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
    }

    private bool localeChange = false;
    public void ShowAD_localeChange()
    {
        if(Components.c.settings.debug_ads_off)
        {
            Components.c.settings.ExecuteLocaleChange();
            return;
        }
        localeChange = true;
        Advertisement.Show(_adUnitId, this);

    }
    private IEnumerator LocaleChangeAd_Done()
    {
        yield return new WaitForSeconds(.2f);
        localeChange = false;
    }
 
    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            if(localeChange)
            {
                Components.c.settings.ExecuteLocaleChange();
                StartCoroutine(LocaleChangeAd_Done());
                return;
            }

            // Load another ad:
            Advertisement.Load(_adUnitId, this);
            if(!Components.c.settings.lastShields)
            {
                Components.c.settings.thisPlayer.current_Hearts += Components.c.settings.thisConfigs.ad_heart_reward;
                Components.c.settings.thisPlayer.current_Skips += Components.c.settings.thisConfigs.ad_skip_reward;
            }
            if(Components.c.settings.lastShields)
            {
                //add may be shield addition to configs
                Components.c.settings.thisPlayer.shield_count += 3;
                Components.c.shieldButton.ShieldButtonPress();
              
            }
            Components.c.settings.SavePlayerdDataToFile();
            Components.c.gameUIMan.UpdateUIToConfigs();

        }
    }
 
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
 
    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}