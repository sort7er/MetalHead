using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject bulletPrefab;
    public GameObject bulletHolePrefab;

    private void Awake()
    {
        instance = this;
    }

    public void Fire(Transform muzzle)
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.transform.parent = ParentManager.instance.bullets;
    }
    public void SpawnBulletHole(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        GameObject bulletHole = Instantiate(bulletHolePrefab, collision.contacts[0].point + normal * 0.001f, Quaternion.LookRotation(normal));
        bulletHole.transform.Rotate(Vector3.forward * Random.Range(-180, 180));
        if (collision.rigidbody != null)
        {
            bulletHole.transform.parent = collision.transform;
        }
        else
        {
            bulletHole.transform.parent = ParentManager.instance.effects;
        }
        Destroy(bulletHole, 25f);

        //GameObject hitEffect = Instantiate(HitEffectPrefab, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
        //hitEffect.transform.parent = hitEffectParent;
        //Destroy(hitEffect, 2f);
    }
}
