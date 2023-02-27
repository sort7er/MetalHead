using UnityEngine;
using TMPro;
public class CZ50Upgrades : MonoBehaviour
{
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
        bulletLevel ++;
        bulletNumber.SetText(bulletLevel.ToString());
        if (!bulletOutline.activeSelf)
        {
            bulletOutline.SetActive(true);
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
        recoilLevel++;
        recoilNumber.SetText(recoilLevel.ToString());
        if (!recoilOutline.activeSelf)
        {
            recoilOutline.SetActive(true);
        }
    }
    public void RemoveLessRecoil()
    {
        if (bulletLevel > startBulletLevel)
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
        ammoClipLevel++;
        ammoClipNumber.SetText(ammoClipLevel.ToString());
        if (!ammoClipOutline.activeSelf)
        {
            ammoClipOutline.SetActive(true);
        }
    }
    public void RemoveMagSize()
    {
        if (bulletLevel > startBulletLevel)
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
        speedLevel++;
        speedNumber.SetText(speedLevel.ToString());
        if (!speedOutline.activeSelf)
        {
            speedOutline.SetActive(true);
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
        SetLevels();
    }
}
