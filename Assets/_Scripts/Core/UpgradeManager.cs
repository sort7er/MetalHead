using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("CZ50")]
    public int startBulletLevel;
    public int startRecoilLevel;
    public int startMagSizeLevel;
    public int startProjectileLevel;
    public int startLaserLevel;


    [Header("Tac14")]
    public int tac14StartReload;
    public int tac14StartAuto;
    public int tac14StartDamage;
    public int tac14StartMagSize;
    public int tac14StartPellets;
    public int tac14StartPenetration;

    [HideInInspector] public int[] tac14StartLevels;
    [HideInInspector] public int[] tac14Caps;


    [Header("Tac14 caps")]
    public int reloadEfficiencyCap;
    public int autoCap, damageCap, magSizeCap, pelletCap, penetrationCap;

    [Header("References")]
    public Transform magPos;
    public Recoil cz50Recoil;
    public CZ50 cz50;
    public Tac14 tac14;
    public CZ50Upgrades cZ50Upgrades;
    public Tac14Upgrades tac14Upgrades;

    [HideInInspector] public int magSize;
    private void Awake()
    {
        instance = this;

        if (cZ50Upgrades != null)
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

        if (tac14Upgrades != null)
        {
            if (tac14StartReload > reloadEfficiencyCap)
            {
                tac14StartReload = reloadEfficiencyCap;
            }
            if (tac14StartAuto > autoCap)
            {
                tac14StartAuto = autoCap;
            }
            if (tac14StartDamage > damageCap)
            {
                tac14StartDamage = damageCap;
            }
            if (tac14StartMagSize > magSizeCap)
            {
                tac14StartMagSize = magSizeCap;
            }
            if (tac14StartPellets > pelletCap)
            {
                tac14StartPellets = pelletCap;
            }
            if (tac14StartPenetration > penetrationCap)
            {
                tac14StartPenetration = penetrationCap;
            }
        }


        tac14StartLevels = new int[6];
        tac14StartLevels[0] = tac14StartReload;
        tac14StartLevels[1] = tac14StartAuto;
        tac14StartLevels[2] = tac14StartDamage;
        tac14StartLevels[3] = tac14StartMagSize;
        tac14StartLevels[4] = tac14StartPellets;
        tac14StartLevels[5] = tac14StartPenetration;

        tac14Caps= new int[6];
        tac14Caps[0] = reloadEfficiencyCap;
        tac14Caps[1] = autoCap;
        tac14Caps[2] = damageCap;
        tac14Caps[3] = magSizeCap;
        tac14Caps[4] = pelletCap;
        tac14Caps[5] = penetrationCap;

    }

    private void Start()
    {
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
        if(tac14 != null)
        {
            tac14.UpgradeReload(reloadLevel);
            tac14.UpgradeAuto(autoLevel);
            tac14.UpgradeDamage(damage);
            tac14.UpgradeMagSize(magSize);
            tac14.UpgradePellets(pellets);
            tac14.UpgradePenetration(projectileLevel);
        }

    }
}
