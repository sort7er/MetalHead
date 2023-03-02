using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject bulletPrefab;
    public GameObject bulletHolePrefab;
    public GameObject[] pickUp;

    private void Awake()
    {
        instance = this;
    }

    public void Fire(Transform muzzle, int damage, int speed)
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.GetComponent<Bullet>().SetDamage(damage);
        bullet.GetComponent<Bullet>().SetSpeed(speed);
        bullet.transform.parent = ParentManager.instance.bullets;
    }
    public void SpawnBulletHole(RaycastHit hit)
    {
        GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
        bulletHole.transform.Rotate(Vector3.forward * Random.Range(-180, 180));
        if (hit.rigidbody != null)
        {
            bulletHole.transform.parent = hit.transform;
        }
        else
        {
            bulletHole.transform.parent = ParentManager.instance.effects;
        }
        Destroy(bulletHole, 25f);
    }

    public void SpawnPickups(Transform enemy, int numberOfPickups)
    {
        for(int i = 0; i < numberOfPickups; i++)
        {
            GameObject pickup = Instantiate(pickUp[Random.Range(0, pickUp.Length)], enemy.position, Quaternion.identity);
            pickup.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2,2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
            pickup.transform.parent = ParentManager.instance.pickups;
        }
    }
}
