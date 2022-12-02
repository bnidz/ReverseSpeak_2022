using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Runtime.InteropServices;
using System;

public class IAPManager : MonoBehaviour
{

    public int shield_1_value = 25;
    public int shield_2_value = 100;
    public int shield_3_value = 300;

    public static IAPManager i;
    // Start is called before the first frame update
    void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (i != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject BuyScreen;
    private string IAPtext_shields_1 = "com.hotminute.reversespeak.shields_1";
    private string IAPtext_shields_2 = "com.hotminute.reversespeak.shields_2";
    private string IAPtext_shields_3 = "com.hotminute.reversespeak.shields_3";
    public GameObject PurchasePopUp;
    public bool isExportPurchased = false;

    public void OnPurchaseComplete(Product product)
    {

        if (product.definition.id == IAPtext_shields_1)
        {


            Components.c.settings.thisPlayer.shield_count += 25;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.sfxmanager.PlaySFX("shield_001");

            //Hide popup


#if UNITY_IOS

           // showNativeAlert("Purchase Completed", "Thank you for purchasing premium face tracking, new features are now enabled!");
            showNativeAlert(Components.c.localisedStrings.shop_text , Components.c.localisedStrings.iap_bundle_thankYou + Components.c.localisedStrings.IAP_shields_1_button_string);

#endif
            BuyScreen.SetActive(false);
            //StartCoroutine(HideIAPMENU());
        }
        if (product.definition.id == IAPtext_shields_2)
        {
            //med shields

                //add shields
            Components.c.settings.thisPlayer.shield_count += 100;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.sfxmanager.PlaySFX("shield_001");




#if UNITY_IOS

            //showNativeAlert("Purchase Completed", "Thank you for purchasing premium face tracking, new features are now enabled!");
            showNativeAlert(Components.c.localisedStrings.shop_text , Components.c.localisedStrings.iap_bundle_thankYou + Components.c.localisedStrings.IAP_shields_2_button_string);

#endif
            BuyScreen.SetActive(false);
            //StartCoroutine(HideIAPMENU());
        }
        if (product.definition.id == IAPtext_shields_3)
        {
        
            Components.c.settings.thisPlayer.shield_count += 300;
            Components.c.gameUIMan.UpdateUIToConfigs();
            Components.c.sfxmanager.PlaySFX("shield_001");


#if UNITY_IOS

            showNativeAlert(Components.c.localisedStrings.shop_text , Components.c.localisedStrings.iap_bundle_thankYou + Components.c.localisedStrings.IAP_shields_3_button_string);

#endif
            BuyScreen.SetActive(false);
            //StartCoroutine(HideIAPMENU());
        }
    }

    private IEnumerator HideIAPMENU()
    {
        yield return new WaitForSeconds(.5f);
        BuyScreen.SetActive(false);
        //TF_Manager.i.notInBuyScreen = true;
        //throw new NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        // if (product.definition.id == IAPtext_faceTrack)
        // {
            //enable export button functionality
            //Hide popup
            // reason.
            //isExportPurchased = false;
            //Debug.Log(reason.ToString());

#if UNITY_IOS

            showNativeAlert(Components.c.localisedStrings.iap_info_failed, reason.ToString());

#endif
            //BuyScreen.SetActive(false);
        
    }

    public void ShowPurchasePanel()
    {
        BuyScreen.SetActive(true);
    }

#if UNITY_IOS

    [DllImport("__Internal")] extern static public void showNativeAlert(string title, string message);

#endif
}