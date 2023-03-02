using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CZ50Upgrades : MonoBehaviour
{
    public int damageLevelCap, recoilLevelCap, ammoClipCap, penetrationCap, laserCap;
    public int damageCost, recoilCost, ammoCost, penetrationCost, laserCost;

    public TextMeshProUGUI bulletCostText, recoilCostText, ammoClipCostText, penetrationCostText, laserCostText;
    public TextMeshProUGUI bulletNumber, recoilNumber, ammoClipNumber, penetrationNumber, laserNumber;
    public GameObject bulletOutline, recoilOutline, ammoClipOutline, penetrationOutline, laserOutline;
    public UpgradeStation upgradeStation;

    private Color startTextColor, selectTextColor;
    private Animator cz50UpgradesAnim;
    private int bulletLevel, recoilLevel, ammoClipLevel, penetrationLevel, laserLevel;
    private int startBulletLevel, startRecoilLevel, startAmmoClipLevel, startPenetrationLevel, startLaserLevel;

    private void Start()
    {
        startTextColor = bulletCostText.color;
        selectTextColor = bulletOutline.GetComponent<Image>().color;
        cz50UpgradesAnim = GetComponent<Animator>();
        bulletLevel = UpgradeManager.instance.startBulletLevel;
        recoilLevel = UpgradeManager.instance.startRecoilLevel;
        ammoClipLevel = UpgradeManager.instance.startMagSizeLevel;
        penetrationLevel = UpgradeManager.instance.startProjectileLevel;
        laserLevel = UpgradeManager.instance.startLaserLevel;
        bulletNumber.text = bulletLevel.ToString();
        recoilNumber.text = recoilLevel.ToString();
        ammoClipNumber.text = ammoClipLevel.ToString();
        penetrationNumber.text = penetrationLevel.ToString();
        laserNumber.text = laserLevel.ToString();
        SetLevels();
        SetCost();
    }

    public void SetLevels()
    {
        startBulletLevel = bulletLevel;
        startRecoilLevel = recoilLevel;
        startAmmoClipLevel = ammoClipLevel;
        startPenetrationLevel = penetrationLevel;
        startLaserLevel = laserLevel;
    }
    public void ResetLevels()
    {
        bulletLevel = startBulletLevel;
        recoilLevel = startRecoilLevel;
        ammoClipLevel = startAmmoClipLevel;
        penetrationLevel = startPenetrationLevel;
        laserLevel = startLaserLevel;
        if(bulletLevel != damageLevelCap)
        {
            bulletNumber.text = bulletLevel.ToString();
        }
        if (recoilLevel != recoilLevelCap)
        {
            recoilNumber.text = recoilLevel.ToString();
        }
        if (ammoClipLevel != ammoClipCap)
        {
            ammoClipNumber.text = ammoClipLevel.ToString();
        }
        if (penetrationLevel != penetrationCap)
        {
            penetrationNumber.text = penetrationLevel.ToString();
        }
        if (laserLevel != laserCap)
        {
            laserNumber.text = laserLevel.ToString();
        }
    }

    public void SetCost()
    {
        bulletCostText.text = damageCost.ToString();
        recoilCostText.text = recoilCost.ToString();
        ammoClipCostText.text = ammoCost.ToString();
        penetrationCostText.text = penetrationCost.ToString();
        laserCostText.text = laserCost.ToString();
    }

    public void AddBullet()
    {
        if (bulletLevel < damageLevelCap)
        {
            if (upgradeStation.currencyOnScreen >= damageCost)
            {
                upgradeStation.AddingPurchase(damageCost);
                bulletLevel++;
                bulletNumber.SetText(bulletLevel.ToString());
                if (!bulletOutline.activeSelf)
                {
                    bulletOutline.SetActive(true);
                    bulletNumber.color = selectTextColor;
                }
                if(bulletLevel >= damageLevelCap)
                {
                    bulletNumber.text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
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
                bulletNumber.color = startTextColor;
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
                    recoilNumber.color = selectTextColor;
                }
                if(recoilLevel >= recoilLevelCap)
                {
                    recoilNumber.text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
            }
        }
    }
    public void RemoveLessRecoil()
    {
        if (recoilLevel > startRecoilLevel)
        {
            upgradeStation.RemovePurchase(recoilCost);
            recoilLevel--;
            recoilNumber.SetText(recoilLevel.ToString());
            if (recoilLevel == startRecoilLevel)
            {
                recoilOutline.SetActive(false);
                recoilNumber.color = startTextColor;
            }
        }
    }
    public void AddMagSize()
    {
        if(ammoClipLevel < ammoClipCap)
        {
            if (upgradeStation.currencyOnScreen >= ammoCost)
            {
                upgradeStation.AddingPurchase(ammoCost);
                ammoClipLevel++;
                ammoClipNumber.SetText(ammoClipLevel.ToString());
                if (!ammoClipOutline.activeSelf)
                {
                    ammoClipOutline.SetActive(true);
                    ammoClipNumber.color = selectTextColor;
                }
                if(ammoClipLevel >= ammoClipCap)
                {
                    ammoClipNumber.text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
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
                ammoClipNumber.color = startTextColor;
            }
        }
    }

    public void AddPenetration()
    {
        if(penetrationLevel < penetrationCap)
        {
            if (upgradeStation.currencyOnScreen >= penetrationCost)
            {
                upgradeStation.AddingPurchase(penetrationCost);
                penetrationLevel++;
                penetrationNumber.SetText(penetrationLevel.ToString());
                if (!penetrationOutline.activeSelf)
                {
                    penetrationOutline.SetActive(true);
                    penetrationNumber.color = selectTextColor;
                }
                if(penetrationLevel >= penetrationCap)
                {
                    penetrationNumber.text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
            }
        }
    }
    public void RemovePenetration()
    {
        if (penetrationLevel > startPenetrationLevel)
        {
            upgradeStation.RemovePurchase(penetrationCost);
            penetrationLevel--;
            penetrationNumber.SetText(penetrationLevel.ToString());
            if (penetrationLevel == startPenetrationLevel)
            {
                penetrationOutline.SetActive(false);
                penetrationNumber.color = startTextColor;
            }
        }
    }
    public void AddLaser()
    {
        if (laserLevel < laserCap)
        {
            if (upgradeStation.currencyOnScreen >= laserCost)
            {
                upgradeStation.AddingPurchase(laserCost);
                laserLevel++;
                laserNumber.SetText(laserLevel.ToString());
                if (!laserOutline.activeSelf)
                {
                    laserOutline.SetActive(true);
                    laserNumber.color = selectTextColor;
                }
                if (laserLevel >= laserCap)
                {
                    laserNumber.text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
            }
        }
    }
    public void RemoveLaser()
    {
        if (laserLevel > startLaserLevel)
        {
            upgradeStation.RemovePurchase(laserCost);
            laserLevel--;
            laserNumber.SetText(laserLevel.ToString());
            if (laserLevel == startLaserLevel)
            {
                laserOutline.SetActive(false);
                laserNumber.color = startTextColor;
            }
        }
    }
    public void Execute()
    {
        UpgradeManager.instance.UpgradeCZ50(bulletLevel, recoilLevel, ammoClipLevel, penetrationLevel, laserLevel);
        GameManager.instance.magnet.SetMetalsCollected(upgradeStation.currencyOnScreen);
        bulletOutline.SetActive(false);
        ammoClipOutline.SetActive(false);
        recoilOutline.SetActive(false);
        penetrationOutline.SetActive(false);
        laserOutline.SetActive(false);
        bulletNumber.color = startTextColor;
        recoilNumber.color = startTextColor;
        ammoClipNumber.color = startTextColor;
        penetrationNumber.color = startTextColor;
        laserNumber.color = startTextColor;
        SetLevels();
    }
    public void Abort()
    {
        bulletOutline.SetActive(false);
        ammoClipOutline.SetActive(false);
        recoilOutline.SetActive(false);
        penetrationOutline.SetActive(false);
        laserOutline.SetActive(false);
        bulletNumber.color = startTextColor;
        recoilNumber.color = startTextColor;
        ammoClipNumber.color = startTextColor;
        penetrationNumber.color = startTextColor;
        laserNumber.color = startTextColor;
        ResetLevels();
    }
    public void ActiveUpgrade(int activeUpgrade)
    {
        cz50UpgradesAnim.SetInteger("Upgrade", activeUpgrade);
    }
}
