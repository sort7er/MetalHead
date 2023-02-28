using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CZ50Upgrades : MonoBehaviour
{
    public Color selectColor;

    public int damageLevelCap, recoilLevelCap, ammoClipCap, bulletSpeedCap;
    public int damageCost, recoilCost, ammoCost, speedCost;

    public Image[] upgradeImage;
    public TextMeshProUGUI bulletCostText, recoilCostText, ammoClipCostText, speedCostText;
    public TextMeshProUGUI bulletNumber, recoilNumber, ammoClipNumber, speedNumber;
    public GameObject bulletOutline, recoilOutline, ammoClipOutline, speedOutline;
    public UpgradeStation upgradeStation;

    private Color startColor, startTextColor, selectTextColor;
    private Animator cz50UpgradesAnim;
    private int activeUpgrade;
    private int bulletLevel, recoilLevel, ammoClipLevel, speedLevel;
    private int startBulletLevel, startRecoilLevel, startAmmoClipLevel, startSpeedLevel;

    private void Start()
    {
        startColor = upgradeImage[1].color;
        startTextColor = bulletCostText.color;
        selectTextColor = bulletOutline.GetComponent<Image>().color;
        upgradeImage[0].color = selectColor;
        cz50UpgradesAnim = GetComponent<Animator>();
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

    public void AddSpeed()
    {
        if(speedLevel < bulletSpeedCap)
        {
            if (upgradeStation.currencyOnScreen >= speedCost)
            {
                upgradeStation.AddingPurchase(speedCost);
                speedLevel++;
                speedNumber.SetText(speedLevel.ToString());
                if (!speedOutline.activeSelf)
                {
                    speedOutline.SetActive(true);
                    speedNumber.color = selectTextColor;
                }
                if(speedLevel >= bulletSpeedCap)
                {
                    speedNumber.text = "Max";
                }
            }
            else
            {
                upgradeStation.NotEnough();
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
                speedNumber.color = startTextColor;
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
        bulletNumber.color = startTextColor;
        recoilNumber.color = startTextColor;
        ammoClipNumber.color = startTextColor;
        speedNumber.color = startTextColor;
        SetLevels();
    }
    public void Up()
    {
        if(activeUpgrade >= 3)
        {
            activeUpgrade = 0;
        }
        else
        {
            activeUpgrade++;
        }
        for (int i = 0; i < upgradeImage.Length; i++)
        {
            upgradeImage[i].color = startColor;
        }
        upgradeImage[activeUpgrade].color = selectColor;
        cz50UpgradesAnim.SetInteger("Upgrade", activeUpgrade);
    }
    public void Down()
    {
        if (activeUpgrade <= 0)
        {
            activeUpgrade = 3;
        }
        else
        {
            activeUpgrade--;
        }
        for(int i = 0; i < upgradeImage.Length; i++)
        {
            upgradeImage[i].color = startColor;
        }
        upgradeImage[activeUpgrade].color = selectColor;
        cz50UpgradesAnim.SetInteger("Upgrade", activeUpgrade);
    }
    public void Add()
    {
        if(activeUpgrade == 0)
        {
            AddMagSize();
        }
        else if(activeUpgrade == 1)
        {
            AddBullet();
        }
        else if(activeUpgrade == 2)
        {
            AddSpeed();
        }
        else if(activeUpgrade == 3)
        {
            AddLessRecoil();
        }
    }
    public void Remove()
    {
        if (activeUpgrade == 0)
        {
            RemoveMagSize();
        }
        else if (activeUpgrade == 1)
        {
            RemoveBullet();
        }
        else if (activeUpgrade == 2)
        {
            RemoveSpeed();
        }
        else if (activeUpgrade == 3)
        {
            RemoveLessRecoil();
        }
    }
}
