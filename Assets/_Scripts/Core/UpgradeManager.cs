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

    [HideInInspector] public int magSize;

    private void Start()
    {
        UpgradeCZ50(startBulletLevel, startRecoilLevel, startMagSizeLevel, startProjectileLevel, startLaserLevel);
    }

    public void SetMagSize(int size)
    {
        magSize = size;
        for(int i = 0; i < magPos.childCount; i++)
        {
            magPos.GetChild(i).GetComponent<Mag>().UpgradeMags();
            GameManager.instance.ammoBag.CheckAmmoStatus();
        }
    }
    public void UpgradeCZ50(int bulletLevel, int recoilLevel, int magSizeLevel, int projectileLevel, int laserLevel)
    {
        cz50.SetDamage(10 + bulletLevel * 5);
        SetMagSize(7 + magSizeLevel * 3);
        cz50Recoil.UpgradeRecoil(recoilLevel);
        if(projectileLevel== 0)
        {
            cz50.ProjectilePenetration(false);
        }
        else
        {
            cz50.ProjectilePenetration(true);
        }
        if (laserLevel == 0)
        {
            cz50.Laser(false);
        }
        else
        {
            cz50.Laser(true);
        }        
    }
}
