using UnityEngine;

public class CZ50 : MonoBehaviour
{
    [Header("Values")]
    public int startAmmo;
    public int magSize;

    [Header("References")]
    public Transform muzzle;
    public Dial doubleDial, singleDial;
    public MeshRenderer doubleZero, singleZero;
    public Material defaultMaterial, emptyMaterial;
    public ParticleSystem muzzleFlash;

    private Animator cz50Anim;
    private SoundForGun soundForGun;
    public int currentAmmo, totalAmmo;
    private int singleDigit, doubleDigit;

    private void Start()
    {
        cz50Anim = GetComponent<Animator>();
        soundForGun = GetComponent<SoundForGun>();
        currentAmmo = magSize;
        totalAmmo = startAmmo;
        UpdateDial();
        DefaultColor();
    }

    public void Fire()
    {
        if(currentAmmo > 0)
        {
            currentAmmo--;
            soundForGun.Fire();
            cz50Anim.SetTrigger("Fire");
            UpdateDial();
            EffectManager.instance.Fire(muzzle);
            muzzleFlash.Play();
        }
        else
        {
            soundForGun.Empty();
            EmptyColor();
            Invoke("DefaultColor", 0.1f);
        }
        
    }

    public void Grab()
    {
        soundForGun.Grab();
    }

    public void Reload()
    {
        if(totalAmmo >= magSize)
        {
            totalAmmo -= magSize - currentAmmo;
            currentAmmo = magSize;
        }
        else if(totalAmmo < magSize)
        {
            currentAmmo += totalAmmo;
            if(currentAmmo > magSize)
            {
                totalAmmo = currentAmmo - magSize;
                currentAmmo = magSize;
            }
            else
            {
                totalAmmo = 0;
            }
        }

        UpdateDial();
    }
    public void MagOut()
    {
        if(currentAmmo != 0)
        {
            int rest = currentAmmo - 1;
            currentAmmo -= rest;
            totalAmmo += rest;
            UpdateDial();
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
}
