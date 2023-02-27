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
    public int startBulletSpeedLevel;
    public Transform magPos;
    public Recoil cz50Recoil;
    public CZ50 cz50;

    [HideInInspector] public int magSize;

    private int bulletLevel, magSizeLevel, recoilLevel, speedLevel;

    private void Start()
    {
        UpgradeCZ50(startBulletLevel, startRecoilLevel, startMagSizeLevel, startBulletSpeedLevel);
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
    public void UpgradeCZ50(int bulletLevel, int recoilLevel, int magSizeLevel, int bulletSpeed)
    {
        cz50.SetDamage(10 + bulletLevel * 5);
        SetMagSize(7 + magSizeLevel * 3);
        cz50Recoil.UpgradeRecoil(recoilLevel);
        cz50.SetSpeed(75 + bulletSpeed * 25);
    }
}
