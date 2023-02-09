using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public AudioClip[] gunShots;
    public AudioClip[] gunGrab;

    private AudioSource gunSource;

    private void Start()
    {
        gunSource= GetComponent<AudioSource>();
    }

    public void Fire()
    {
        //gunSource.pitch = Random.Range(0.95f, 1.05f);
        gunSource.PlayOneShot(gunShots[Random.Range(0, gunShots.Length)]);

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.transform.parent = ParentManager.instance.bullets;
    }

    public void Grab()
    {
        gunSource.PlayOneShot(gunGrab[Random.Range(0, gunGrab.Length)]);
    }
}
