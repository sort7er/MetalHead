using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("CZ50")]
    public int startAmmoClipSizeLevel;
    public int startBulletLevel;
    public int startLaserLevel;
    public int startProjectileLevel;
    public int startRecoilLevel;

    [Header("CZ50 caps")]
    public int ammoClipSizeCap;
    public int bulletCap;
    public int laserCap;
    public int penetrarionCap;
    public int recoilCap;

    [Header("Tac14")]
    public int tac14StartReload;
    public int tac14StartAuto;
    public int tac14StartDamage;
    public int tac14StartMagSize;
    public int tac14StartPellets;
    public int tac14StartPenetration;


    [Header("Tac14 caps")]
    public int reloadEfficiencyCap;
    public int autoCap, damageCap, magSizeCap, pelletCap, penetrationCap;

    [Header("References")]
    public Transform magPos;
    public Recoil cz50Recoil;
    public CZ50 cz50;
    public Tac14 tac14;

    [HideInInspector] public int[] cz50StartLevels;
/*    [HideInInspector] */public int[] cz50CurrentLevels;
    [HideInInspector] public int[] tac14StartLevels;
    [HideInInspector] public int[] tac14CurrentLevels;
    [HideInInspector] public int[] cz50caps;
    [HideInInspector] public int[] tac14Caps;
    [HideInInspector] public int magSize;


    private void Awake()
    {
        instance = this;

        if (startAmmoClipSizeLevel > ammoClipSizeCap)
        {
            startAmmoClipSizeLevel = ammoClipSizeCap;
        }
        if (startBulletLevel > bulletCap)
        {
            startBulletLevel = bulletCap;
        }
        if (startLaserLevel > laserCap)
        {
            startLaserLevel = laserCap;
        }
        if (startProjectileLevel > penetrationCap)
        {
            startProjectileLevel = penetrationCap;
        }
        if (startRecoilLevel > recoilCap)
        {
            startRecoilLevel = recoilCap;
        }


        cz50StartLevels = new int[5];
        cz50StartLevels[0] = startAmmoClipSizeLevel;
        cz50StartLevels[1] = startBulletLevel;
        cz50StartLevels[2] = startLaserLevel;
        cz50StartLevels[3] = startProjectileLevel;
        cz50StartLevels[4] = startRecoilLevel;

        cz50CurrentLevels= new int[5];
        cz50CurrentLevels[0] = startAmmoClipSizeLevel;
        cz50CurrentLevels[1] = startBulletLevel;
        cz50CurrentLevels[2] = startLaserLevel;
        cz50CurrentLevels[3] = startProjectileLevel;
        cz50CurrentLevels[4] = startRecoilLevel;

        cz50caps = new int[5];
        cz50caps[0] = ammoClipSizeCap;
        cz50caps[1] = bulletCap;
        cz50caps[2] = laserCap;
        cz50caps[3] = penetrarionCap;
        cz50caps[4] = recoilCap;


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


        tac14StartLevels = new int[6];
        tac14StartLevels[0] = tac14StartReload;
        tac14StartLevels[1] = tac14StartAuto;
        tac14StartLevels[2] = tac14StartDamage;
        tac14StartLevels[3] = tac14StartMagSize;
        tac14StartLevels[4] = tac14StartPellets;
        tac14StartLevels[5] = tac14StartPenetration;

        tac14CurrentLevels = new int[6];
        tac14CurrentLevels[0] = tac14StartReload;
        tac14CurrentLevels[1] = tac14StartAuto;
        tac14CurrentLevels[2] = tac14StartDamage;
        tac14CurrentLevels[3] = tac14StartMagSize;
        tac14CurrentLevels[4] = tac14StartPellets;
        tac14CurrentLevels[5] = tac14StartPenetration;

        tac14Caps = new int[6];
        tac14Caps[0] = reloadEfficiencyCap;
        tac14Caps[1] = autoCap;
        tac14Caps[2] = damageCap;
        tac14Caps[3] = magSizeCap;
        tac14Caps[4] = pelletCap;
        tac14Caps[5] = penetrationCap;

    }

    private void Start()
    {
        UpgradeCZ50(startAmmoClipSizeLevel, startBulletLevel, startLaserLevel, startProjectileLevel, startRecoilLevel);
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
    public void UpgradeCZ50(int ammoClipSizeLevel, int bulletLevel, int laserLevel, int projectileLevel, int recoilLevel)
    {
        cz50CurrentLevels[0] = ammoClipSizeLevel;
        cz50CurrentLevels[1] = bulletLevel;
        cz50CurrentLevels[2] = laserLevel;
        cz50CurrentLevels[3] = projectileLevel;
        cz50CurrentLevels[4] = recoilLevel;


        SetMagSize(7 + ammoClipSizeLevel * 3);
        cz50.SetDamage(10 + bulletLevel * 5);
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
            tac14CurrentLevels[0] = reloadLevel;
            tac14CurrentLevels[1] = autoLevel;
            tac14CurrentLevels[2] = damage;
            tac14CurrentLevels[3] = magSize;
            tac14CurrentLevels[4] = pellets;
            tac14CurrentLevels[5] = projectileLevel;



            tac14.UpgradeReload(reloadLevel);
            tac14.UpgradeAuto(autoLevel);
            tac14.UpgradeDamage(damage);
            tac14.UpgradeMagSize(magSize);
            tac14.UpgradePellets(pellets);
            tac14.UpgradePenetration(projectileLevel);
        }

    }
}
