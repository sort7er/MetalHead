using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Gun")]
    public int bulletDamage;
    public float bulletSpeed;

    [Header("References")]
    public Transform gunMuzzle;
    public Transform bombMuzzle;
    public GameObject bombPrefab;
    public AudioSource gunSource, gunSource2;
    public ParticleSystem muzzleFlash;

    private float multiplier;
    private bool swap;

    public void FireBomb()
    {
        Debug.Log("Fire");
        GameObject bomb = Instantiate(bombPrefab, bombMuzzle.position, bombMuzzle.rotation);
        bomb.transform.parent = ParentManager.instance.bullets;

        float distance = Vector3.Distance(transform.position, GameManager.instance.XROrigin.transform.position);
        float heightDifference = GameManager.instance.XROrigin.transform.position.y - transform.position.y;

        Debug.Log(heightDifference);

        if (heightDifference < 0.1f)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = 1 + (heightDifference * 0.005f);
        }

        bomb.GetComponent<Rigidbody>().AddForce(bombMuzzle.forward * distance * 100 * multiplier, ForceMode.Impulse);

    }

    public void FireBullet()
    {
        if (!swap)
        {
            gunSource2.Play();
            swap = true;
        }
        else
        {
            gunSource.Play();
            swap = false;
        }
        
        muzzleFlash.Play();
        EffectManager.instance.Fire(gunMuzzle, bulletDamage, bulletSpeed);
        Debug.Log("Fire bullet");
    }
}
