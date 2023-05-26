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
        GameObject bomb = Instantiate(bombPrefab, bombMuzzle.position, bombMuzzle.rotation);
        bomb.transform.parent = ParentManager.instance.bullets;

        float distance = Vector3.Distance(transform.position, GameManager.instance.XROrigin.transform.position);

        if(distance > 20)
        {
            multiplier = distance * 0.5f;
        }
        else if(distance > 10)
        {
            multiplier = distance * 0.75f;
        }
        else
        {
            multiplier = distance;
        }

        bomb.GetComponent<Rigidbody>().AddForce(bombMuzzle.forward * multiplier * 100, ForceMode.Impulse);

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
    }
}
