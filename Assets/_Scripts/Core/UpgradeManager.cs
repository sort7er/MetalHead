using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    private void Awake()
    {
        instance= this;
    }

    [Header("CZ50")]
    public int startBulletLevel;
    public int startRecoilLevel;
    public int startMagSizeLevel;
    public int startProjectileLevel;
    public int startLaserLevel;
    public Transform magPos;
    public Recoil cz50Recoil;
    public CZ50 cz50;
    public Tac14 tac14;

    [Header("Tac14")]
    public int tac14StartReload;
    public int tac14StartAuto;
    public int tac14StartDamage;
    public int tac14StartMagSize;
    public int tac14StartPellets;
    public int tac14StartPenetration;

    public CZ50Upgrades cZ50Upgrades;

    [HideInInspector] public int magSize;

    private void Start()
    {
        if(cZ50Upgrades != null)
        {
            if (startBulletLevel > cZ50Upgrades.damageLevelCap)
            {
                startBulletLevel = cZ50Upgrades.damageLevelCap;
            }
            if (startRecoilLevel > cZ50Upgrades.recoilLevelCap)
            {
                startRecoilLevel = cZ50Upgrades.recoilLevelCap;
            }
            if (startMagSizeLevel > cZ50Upgrades.damageLevelCap)
            {
                startMagSizeLevel = cZ50Upgrades.damageLevelCap;
            }
            if (startProjectileLevel > cZ50Upgrades.penetrationCap)
            {
                startProjectileLevel = cZ50Upgrades.penetrationCap;
            }
            if (startLaserLevel > cZ50Upgrades.laserCap)
            {
                startLaserLevel = cZ50Upgrades.laserCap;
            }
        }
        
        UpgradeCZ50(startBulletLevel, startRecoilLevel, startMagSizeLevel, startProjectileLevel, startLaserLevel);
        UpgradeTac14(tac14StartReload, tac14StartAuto, tac14StartDamage, tac14StartMagSize, tac14StartPellets, tac14StartPenetration);

    }

    public void SetMagSize(int size)
    {
        magSize = size;
        for(int i = 0; i < magPos.childCount; i++)
        {
            magPos.GetChild(i).GetComponent<Mag>().UpgradeMags();
        }
        GameManager.instance.ammoBag.UpdateAmmo();
    }
    public void UpgradeCZ50(int bulletLevel, int recoilLevel, int magSizeLevel, int projectileLevel, int laserLevel)
    {
        cz50.SetDamage(10 + bulletLevel * 5);
        SetMagSize(7 + magSizeLevel * 3);
        cz50Recoil.UpgradeRecoil(recoilLevel);
        if(projectileLevel== 2)
        {
            cz50.ProjectilePenetration(true);
        }
        else
        {
            cz50.ProjectilePenetration(false);
        }
        if (laserLevel == 2)
        {
            cz50.Laser(true);
        }
        else
        {
            cz50.Laser(false);
        }        
    }
    public void UpgradeTac14(int reloadLevel, int autoLevel, int damage, int magSize, int pellets, int projectileLevel)
    {
        tac14.UpgradeReload(reloadLevel);
        tac14.UpgradeAuto(autoLevel);
        tac14.UpgradeDamage(damage);
        tac14.UpgradeMagSize(magSize);
        tac14.UpgradePellets(pellets);
        tac14.UpgradePenetration(projectileLevel);
    }
}
