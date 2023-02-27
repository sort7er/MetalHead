using UnityEngine;
using TMPro;
public class CZ50Upgrades : MonoBehaviour
{
    public int damageLevelCap, recoilLevelCap, ammoClipCap, bulletSpeedCap;

    public TextMeshProUGUI bulletCost, recoilCost, ammoClipCost, speedCost;
    public TextMeshProUGUI bulletNumber, recoilNumber, ammoClipNumber, speedNumber;
    public GameObject bulletOutline, recoilOutline, ammoClipOutline, speedOutline;

    private int bulletLevel, recoilLevel, ammoClipLevel, speedLevel;
    private int startBulletLevel, startRecoilLevel, startAmmoClipLevel, startSpeedLevel;

    private void Start()
    {
        bulletLevel = UpgradeManager.instance.startBulletLevel;
        recoilLevel = UpgradeManager.instance.startRecoilLevel;
        ammoClipLevel = UpgradeManager.instance.startMagSizeLevel;
        speedLevel = UpgradeManager.instance.startBulletSpeedLevel;
        bulletNumber.SetText(bulletLevel. ToString());
        recoilNumber.SetText(recoilLevel. ToString());
        ammoClipNumber.SetText(ammoClipLevel. ToString());
        speedNumber.SetText(speedLevel. ToString());
        SetLevels();
    }

    public void SetLevels()
    {
        startBulletLevel = bulletLevel;
        startRecoilLevel = recoilLevel;
        startAmmoClipLevel = ammoClipLevel;
        startSpeedLevel = speedLevel;
    }

    public void AddBullet()
    {
        if (bulletLevel < damageLevelCap)
        {
            bulletLevel++;
            bulletNumber.SetText(bulletLevel.ToString());
            if (!bulletOutline.activeSelf)
            {
                bulletOutline.SetActive(true);
            }
        }
    }
    public void RemoveBullet()
    {
        if(bulletLevel > startBulletLevel)
        {
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
            recoilLevel++;
            recoilNumber.SetText(recoilLevel.ToString());
            if (!recoilOutline.activeSelf)
            {
                recoilOutline.SetActive(true);
            }
        }
    }
    public void RemoveLessRecoil()
    {
        if (recoilLevel > startSpeedLevel)
        {
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
            ammoClipLevel++;
            ammoClipNumber.SetText(ammoClipLevel.ToString());
            if (!ammoClipOutline.activeSelf)
            {
                ammoClipOutline.SetActive(true);
            }
        }
    }
    public void RemoveMagSize()
    {
        if (ammoClipLevel > startAmmoClipLevel)
        {
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
            speedLevel++;
            speedNumber.SetText(speedLevel.ToString());
            if (!speedOutline.activeSelf)
            {
                speedOutline.SetActive(true);
            }
        }

    }
    public void RemoveSpeed()
    {
        if (speedLevel > startSpeedLevel)
        {
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
        bulletOutline.SetActive(false);
        ammoClipOutline.SetActive(false);
        recoilOutline.SetActive(false);
        speedOutline.SetActive(false);
        SetLevels();
    }
}
