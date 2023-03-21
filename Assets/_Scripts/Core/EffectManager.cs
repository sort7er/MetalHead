using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject bulletPrefab;
    public GameObject bulletHolePrefab;
    public GameObject hitPlayer;
    public GameObject hitEnemy;
    public GameObject hitEnemyCrit;
    public GameObject enemyDeadEffect;
    public GameObject[] pickUp;
    public GameObject[] pickUpEffect;

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
    public void SpawnBulletHole(RaycastHit hit, int type)
    {
        GameObject hitEffectToInstantiate;
        if(type == 1)
        {
            hitEffectToInstantiate = hitEnemy;
        }
        else if(type == 2)
        {
            hitEffectToInstantiate = hitEnemyCrit;
        }
        else
        {
            hitEffectToInstantiate = bulletHolePrefab;
        }

        GameObject bulletHole = Instantiate(hitEffectToInstantiate, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
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
            int randomPickup = Random.Range(0, pickUp.Length);
            GameObject pickup = Instantiate(pickUp[randomPickup], enemy.position, Quaternion.identity);
            pickup.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2,2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
            pickup.GetComponent<Pickup>().SetPickupID(randomPickup);
            pickup.transform.parent = ParentManager.instance.pickups;
        }
    }
    public void SpawnPickupeEffect(Transform pointToSpawn, int pickUpID)
    {
        GameObject pickup = Instantiate(pickUpEffect[pickUpID], pointToSpawn.position, Quaternion.identity);
        pickup.transform.parent = ParentManager.instance.pickups;
        Destroy(pickup, 1);
    }
    public void SpawnDeadEnemyEffect(Transform pointToSpawn)
    {
        GameObject effect = Instantiate(enemyDeadEffect, pointToSpawn.position, Quaternion.identity);
        effect.transform.parent = ParentManager.instance.effects;
        Destroy(effect, 4);
    }
    public void SpawnHitPlayerEffect(Vector3 positionToSpawn)
    {
        GameObject effect = Instantiate(hitPlayer, positionToSpawn, Quaternion.identity);
        effect.transform.parent = ParentManager.instance.effects;
        Destroy(effect, 2);
    }
}
