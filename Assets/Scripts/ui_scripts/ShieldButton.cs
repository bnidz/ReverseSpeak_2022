using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldButton : MonoBehaviour
{
    public Image shieldImage;
    public Sprite shield_Full;
    public Sprite shield_EMPTY;
    
    // Start is called before the first frame update
    public void ShieldButtonPress()
    {
        if(Components.c.settings.currentPlayer.shield_count <= 0)
        {
            //launch add
            shieldImage.sprite = shield_EMPTY;
            Components.c.gameUIMan.UpdateShieldsIndicator();
            LaunchShieldsAd();
            return;
        }

        shieldImage.sprite = shield_Full;
        // check if player have sheilds
        if(Components.c.settings.currentPlayer.shield_count > 0)
        {
            if(Components.c.settings.currentPlayer.multiplier > 1)
            {
                // -- activate if multiplier --

                Components.c.settings.isActiveShield = true;
                //activate shield GFX


                if(Components.c.settings.currentPlayer.shield_count == 1)
                {
                    shieldImage.sprite = shield_EMPTY;
                }
                Components.c.settings.currentPlayer.shield_count--;
                //update hud
                Components.c.gameUIMan.UpdateShieldsIndicator();
                return;
            }
        }
    }


    public void DeActivateShield()
    {

        Components.c.settings.isActiveShield = false;
        //deactivate gfx

    }

    public void LaunchShieldsAd()
    {
        //LAUNCH AD
        Components.c.rewardedAdsButton.ShowAd_sheilds();
    }
}
