using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class ShieldButton : MonoBehaviour
{
    public Image shieldImage;
    public Sprite shield_Full;
    public Sprite shield_EMPTY;
    public GameObject thisShield;
    public GameObject shield;
    // Start is called before the first frame update
    public void ShieldButtonPress()
    {
        if(Components.c.settings.isActiveShield)
        {
            return;
        }
        if(Components.c.settings.thisPlayer.shield_count <= 0)
        {
            //launch add
            shieldImage.sprite = shield_EMPTY;
            Components.c.gameUIMan.UpdateShieldsIndicator();
            LaunchShieldsAd();
            StartCoroutine(Shield_ON_OFF(false));
            return;
        }
        shieldImage.sprite = shield_Full;
        // check if player have sheilds
        if(Components.c.settings.thisPlayer.shield_count > 0)
        {
            if(Components.c.settings.thisPlayer.multiplier > 1)
            {
                // -- activate if multiplier --
                ActivateShield();
                //return;
            }
        }
    }
    public Image heartImage;
    public TextMeshProUGUI plustext;
    public IEnumerator Shield_ON_OFF(bool on)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        thisShield.SetActive(on);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }
    public void DeActivateShield()
    {
        //thisShield.SetActive(true);
        Components.c.settings.isActiveShield = false;
        //deactivate 
        shield.SetActive(false);
        shieldImage.enabled = true;
        ChangeHeartColorRed();

        if(Components.c.settings.thisPlayer.shield_count < 1)
        {
            plustext.text = "+";
        }
        if(Components.c.settings.thisPlayer.shield_count > 0)
        {
            plustext.text = "";
        }
        Components.c.gameUIMan.UpdateShieldsIndicator();
        if(Components.c.settings.thisPlayer.multiplier > 1)
        {
            //thisShield.SetActive(true);
            StartCoroutine(Shield_ON_OFF(true));
        }
        else
        {
            //thisShield.SetActive(false);
            StartCoroutine(Shield_ON_OFF(false));
        }    
    }
    public void CheckStatusTo_GFX()
    {
        //Components.c.gameUIMan.UpdateShieldsIndicator();
        if(Components.c.settings.thisPlayer.multiplier > 1)
        {
            if(Components.c.settings.isActiveShield)
            {
                StartCoroutine(Shield_ON_OFF(false));
            }            
            else
            {
                if(Components.c.settings.thisPlayer.shield_count > 0)
                {
                    StartCoroutine(Shield_ON_OFF(true));
                }
                //thisShield.SetActive(false);
                return;
            }
        }else
        {

                StartCoroutine(Shield_ON_OFF(false));
            
        }
    }

    public void ActivateShield()
    {
        Components.c.settings.isActiveShield = true;
        //activate shield GFX
        shield.SetActive(true);
        ChangeHeartColorBlue();
        {
            StartCoroutine(Shield_ON_OFF(false));
        }
        //update hud
        Components.c.settings.thisPlayer.shield_count--;
        Components.c.gameUIMan.UpdateShieldsIndicator();
        // shieldImage.enabled = false;
        // StartCoroutine(Shield_ON_OFF(false));
    }

    public void LaunchShieldsAd()
    {
        //LAUNCH AD
        Components.c.rewardedAdsButton.ShowAd_sheilds();
    }
    public Color colorRed;
    public Color colorBlue;
    public void ChangeHeartColorBlue()
    {
        heartImage.color = colorBlue;
    }
    public void ChangeHeartColorRed()
    {
        heartImage.color = colorRed;
    }
}