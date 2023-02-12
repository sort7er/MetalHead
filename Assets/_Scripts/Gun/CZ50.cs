using UnityEngine;

public class CZ50 : MonoBehaviour
{
    [Header("Values")]
    public int magSize;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform muzzle;
    public AudioClip[] gunShots;
    public AudioClip[] gunGrab;
    public Dial doubleDial, singleDial;


    private AudioSource gunSource;
    private int currentAmmo;
    private int singleDigit, doubleDigit;

    private void Start()
    {
        gunSource= GetComponent<AudioSource>();
        currentAmmo = magSize;
    }

    public void Fire()
    {
        gunSource.PlayOneShot(gunShots[Random.Range(0, gunShots.Length)]);
        currentAmmo--;
        singleDigit = currentAmmo % 10;
        doubleDigit = (currentAmmo / 10) % 10;
        singleDial.SetDial(singleDigit);
        doubleDial.SetDial(doubleDigit);

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.transform.parent = ParentManager.instance.bullets;
    }

    public void Grab()
    {
        gunSource.PlayOneShot(gunGrab[Random.Range(0, gunGrab.Length)]);
    }
}
