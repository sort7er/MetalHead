using UnityEngine;

public class CZ50 : MonoBehaviour
{
    [Header("Values")]
    public int startAmmo;

    [Header("References")]
    public GameObject casingPrefab;
    public Transform muzzle, casingPoint;
    public Dial doubleDial, singleDial;
    public MeshRenderer doubleZero, singleZero;
    public Material defaultMaterial, emptyMaterial;
    public ParticleSystem muzzleFlash;

    private Mag magInGun;
    private Animator cz50Anim;
    private SoundForGun soundForGun;
    public int currentAmmo;
    private int singleDigit, doubleDigit;
    private bool reloadNeeded, firstDialUpdate;

    private void Start()
    {
        cz50Anim = GetComponent<Animator>();
        soundForGun = GetComponent<SoundForGun>();
        DefaultColor();
    }

    public void Fire()
    {
        if(currentAmmo > 0 && !reloadNeeded)
        {
            magInGun.Fire();
            currentAmmo--;
            soundForGun.Fire();
            cz50Anim.SetTrigger("Fire");
            UpdateDial();
            EffectManager.instance.Fire(muzzle);
            muzzleFlash.Play();
        }
        else
        {
            reloadNeeded = true;
            soundForGun.Empty();
            EmptyColor();
            Invoke("DefaultColor", 0.1f);
        }
        
    }

    public void Grab()
    {
        soundForGun.Grab();
        if(GameManager.instance.CheckHand("Pistol") == 1)
        {
            GameManager.instance.leftHand.GrabPistol(true);
        }
        if (GameManager.instance.CheckHand("Pistol") == 2)
        {
            GameManager.instance.rightHand.GrabPistol(true);
        }
    }
    public void Release()
    {
        GameManager.instance.leftHand.GrabPistol(false);
        GameManager.instance.rightHand.GrabPistol(false);
    }

    public void Reload()
    {
        reloadNeeded = false;
        UpdateDial();
    }
    public void MagOut()
    {
        currentAmmo = 0;
        UpdateDial();
    }
    public void MagIn(Mag mag)
    {
        magInGun = mag;
        currentAmmo = magInGun.GetCurrentAmmoFromMag();
        if (!firstDialUpdate)
        {
            UpdateDial();
            firstDialUpdate = true;
        }
    }

    public void UpdateDial()
    {
        singleDigit = currentAmmo % 10;
        doubleDigit = (currentAmmo / 10) % 10;
        singleDial.SetDial(singleDigit);
        doubleDial.SetDial(doubleDigit);
    }
    private void OnCollisionEnter(Collision collision)
    {
        soundForGun.Grab();
    }
    private void DefaultColor()
    {
        doubleZero.material = defaultMaterial;
        singleZero.material = defaultMaterial;
    }
    private void EmptyColor()
    {
        doubleZero.material = emptyMaterial;
        singleZero.material = emptyMaterial;
    }
    public void Casing()
    {
        GameObject casing = Instantiate(casingPrefab, casingPoint.position, casingPoint.rotation);
        casing.transform.parent = ParentManager.instance.bullets;
    }
}
