using UnityEngine;
using TMPro;
public class CZ50Upgrades : MonoBehaviour
{
    public int damageLevelCap, recoilLevelCap, ammoClipCap, bulletSpeedCap;
    public int damageCost, recoilCost, ammoCost, speedCost;

    public TextMeshProUGUI bulletCostText, recoilCostText, ammoClipCostText, speedCostText;
    public TextMeshProUGUI bulletNumber, recoilNumber, ammoClipNumber, speedNumber;
    public GameObject bulletOutline, recoilOutline, ammoClipOutline, speedOutline;
    public UpgradeStation upgradeStation;

    private int bulletLevel, recoilLevel, ammoClipLevel, speedLevel;
    private int startBulletLevel, startRecoilLevel, startAmmoClipLevel, startSpeedLevel;

    private void Start()
    {

        bulletLevel = UpgradeManager.instance.startBulletLevel;
        recoilLevel = UpgradeManager.instance.startRecoilLevel;
        ammoClipLevel = UpgradeManager.instance.startMagSizeLevel;
        speedLevel = UpgradeManager.instance.startBulletSpeedLevel;
        bulletNumber.text = bulletLevel.ToString();
        recoilNumber.text = recoilLevel.ToString();
        ammoClipNumber.text = ammoClipLevel.ToString();
        speedNumber.text = speedLevel.ToString();
        SetLevels();
        SetCost();
    }

    public void SetLevels()
    {
        startBulletLevel = bulletLevel;
        startRecoilLevel = recoilLevel;
        startAmmoClipLevel = ammoClipLevel;
        startSpeedLevel = speedLevel;
    }

    public void SetCost()
    {
        bulletCostText.text = damageCost.ToString();
        recoilCostText.text = recoilCost.ToString();
        ammoClipCostText.text = ammoCost.ToString();
        speedCostText.text = speedCost.ToString();
    }

    public void AddBullet()
    {
        if (bulletLevel < damageLevelCap)
        {
            if (upgradeStation.currencyOnScreen >= damageCost)
            {
                bulletLevel++;
                bulletNumber.SetText(bulletLevel.ToString());
                if (!bulletOutline.activeSelf)
                {
                    bulletOutline.SetActive(true);
                }
            }
            
        }
    }
    public void RemoveBullet()
    {
        if(bulletLevel > startBulletLevel)
        {
            upgradeStation.RemovePurchase(damageCost);
            bulletLevel--;
            bulletNumber.SetText(bulletLevel.ToString());
            if(bulletLevel == startBulletLevel)
            {
                bulletOutline.SetActive(false);
            }
        }        
    }
    public void AddLessRecoil()
    {
        if(recoilLevel < recoilLevelCap)
        {

            if(upgradeStation.currencyOnScreen >= recoilCost)
            {
                upgradeStation.AddingPurchase(recoilCost);
                recoilLevel++;
                recoilNumber.SetText(recoilLevel.ToString());
                if (!recoilOutline.activeSelf)
                {
                    recoilOutline.SetActive(true);
                }
            }            
        }
    }
    public void RemoveLessRecoil()
    {
        if (recoilLevel > startSpeedLevel)
        {
            upgradeStation.RemovePurchase(recoilCost);
            recoilLevel--;
            recoilNumber.SetText(recoilLevel.ToString());
            if (recoilLevel == startRecoilLevel)
            {
                recoilOutline.SetActive(false);
            }
        }
    }
    public void AddMagSize()
    {
        if(ammoClipLevel < ammoClipCap)
        {
            if (upgradeStation.currencyOnScreen >= ammoCost)
            {
                ammoClipLevel++;
                ammoClipNumber.SetText(ammoClipLevel.ToString());
                if (!ammoClipOutline.activeSelf)
                {
                    ammoClipOutline.SetActive(true);
                }
            } 
        }
    }
    public void RemoveMagSize()
    {
        if (ammoClipLevel > startAmmoClipLevel)
        {
            upgradeStation.RemovePurchase(ammoCost);
            ammoClipLevel--;
            ammoClipNumber.SetText(ammoClipLevel.ToString());
            if (ammoClipLevel == startAmmoClipLevel)
            {
                ammoClipOutline.SetActive(false);
            }
        }
    }

    public void AddSpeed()
    {
        if(speedLevel < bulletSpeedCap)
        {
            if (upgradeStation.currencyOnScreen >= speedCost)
            {
                speedLevel++;
                speedNumber.SetText(speedLevel.ToString());
                if (!speedOutline.activeSelf)
                {
                    speedOutline.SetActive(true);
                }
            }
        }
    }
    public void RemoveSpeed()
    {
        if (speedLevel > startSpeedLevel)
        {
            upgradeStation.RemovePurchase(speedCost);
            speedLevel--;
            speedNumber.SetText(speedLevel.ToString());
            if (speedLevel == startSpeedLevel)
            {
                speedOutline.SetActive(false);
            }
        }
    }

    public void Execute()
    {
        UpgradeManager.instance.UpgradeCZ50(bulletLevel, recoilLevel, ammoClipLevel, speedLevel);
        GameManager.instance.magnet.SetMetalsCollected(upgradeStation.currencyOnScreen);
        bulletOutline.SetActive(false);
        ammoClipOutline.SetActive(false);
        recoilOutline.SetActive(false);
        speedOutline.SetActive(false);
        SetLevels();
    }
}
