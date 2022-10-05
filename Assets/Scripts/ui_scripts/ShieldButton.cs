using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
                return;
            }
        }
    }

    public Image heartImage;

    public TextMeshProUGUI plustext;
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
        Components.c.gameUIMan.UpdateShieldsIndicator();
        if(Components.c.settings.thisPlayer.multiplier > 1)
        {
            thisShield.SetActive(true);
        }else
        {
            thisShield.SetActive(false);
        }
        
    }

    public void CheckStatusTo_GFX()
    {
        //Components.c.gameUIMan.UpdateShieldsIndicator();
        if(Components.c.settings.thisPlayer.multiplier > 1)
        {
            thisShield.SetActive(true);
        }else
        {
            thisShield.SetActive(false);
        }
    }

    public void ActivateShield()
    {

        Components.c.settings.isActiveShield = true;
        //activate shield GFX
        shield.SetActive(true);
        ChangeHeartColorBlue();
        plustext.text = "+";

        if(Components.c.settings.thisPlayer.shield_count == 1)
        {
            shieldImage.sprite = shield_EMPTY;
        }
        //update hud
        Components.c.gameUIMan.UpdateShieldsIndicator();
        shieldImage.enabled = false;
        Components.c.settings.thisPlayer.shield_count--;
        thisShield.SetActive(false);
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