using UnityEngine;

public class CZ50 : MonoBehaviour
{
    [Header("Values")]
    public int startAmmo;
    public int magSize;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform muzzle;
    public Dial doubleDial, singleDial;
    public MeshRenderer doubleZero, singleZero;
    public Material defaultMaterial, emptyMaterial;

    private SoundForGun soundForGun;
    private int currentAmmo, totalAmmo;
    private int singleDigit, doubleDigit;

    private void Start()
    {
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
            UpdateDial();
            GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            bullet.transform.parent = ParentManager.instance.bullets;
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
