using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [Header("VisualEffects")]
    public static EffectManager instance;
    public GameObject bulletPrefab;
    public GameObject bulletHolePrefab;
    public GameObject hitPlayer;
    public GameObject hitEnemy;
    public GameObject hitEnemyBarrel;
    public GameObject hitEnemyCrit;
    public GameObject hitEnemyMelee;
    public GameObject hitEnemyMeleeCrit;
    public GameObject parryEffect;
    public GameObject parryEffectUI;
    public GameObject popUpMessage;
    public GameObject enemyDeadEffect;
    public GameObject[] pickUp;
    public GameObject[] pickUpEffect;

    [Header("SoundEffects")]
    public GameObject keyPickup;
    public GameObject keyOpen;
    public GameObject parrySoundEffect;
    public GameObject parryFailedSoundEffect;
    public GameObject hitEnemyOnlySound;


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
        float pitch;
        if(type == 1)
        {
            hitEffectToInstantiate = hitEnemy;
            pitch = 1;
        }
        else if(type == 2)
        {
            hitEffectToInstantiate = hitEnemyCrit;
            pitch = 1;
        }
        else if (type == 3)
        {
            hitEffectToInstantiate = hitEnemy;
            pitch = 1.4f;
        }
        else
        {
            hitEffectToInstantiate = bulletHolePrefab;
            pitch = 1;
        }

        GameObject bulletHole = Instantiate(hitEffectToInstantiate, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
        bulletHole.transform.Rotate(Vector3.forward * Random.Range(-180, 180));
        if(bulletHole.GetComponent<AudioSource>() != null)
        {
            bulletHole.GetComponent<AudioSource>().pitch = pitch;
        }
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
            int randomNumber = Random.Range(0, 100);
            int pickupNumber;
            if(randomNumber >= 82)
            {
                pickupNumber = 0;
            }
            else if(randomNumber >= 50)
            {
                pickupNumber = 1;
            }
            else
            {
                pickupNumber = 2;
            }

            GameObject pickup = Instantiate(pickUp[pickupNumber], enemy.position, Quaternion.identity);
            pickup.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2,2), 2, Random.Range(-2, 2)), ForceMode.Impulse);
            pickup.GetComponent<Pickup>().SetPickupID(pickupNumber);
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
    public void SpawnBarrelHitEnemy(Vector3 positionToSpawn)
    {
        GameObject effect = Instantiate(hitEnemyBarrel, positionToSpawn, Quaternion.identity);
        effect.transform.parent = ParentManager.instance.effects;
        Destroy(effect, 2);
    }
    public void SpawnParryEffect(Vector3 positionToSpawn)
    {
        GameObject effect = Instantiate(parryEffect, positionToSpawn, Quaternion.identity);
        effect.transform.parent = ParentManager.instance.effects;
        Destroy(effect, 2);
    }
    public void SpawnParrySoundEffect(Vector3 positionToSpawn, int type)
    {
        GameObject sfxToInstantiate;
        if (type == 1)
        {
            sfxToInstantiate = parryFailedSoundEffect;
        }
        else
        {
            sfxToInstantiate = parrySoundEffect;
        }
        GameObject effect = Instantiate(sfxToInstantiate, positionToSpawn, Quaternion.identity);
        effect.transform.parent = ParentManager.instance.effects;
        Destroy(effect, 2);
    }
    public void SpawnParryEffectUI(Vector3 positionToSpawn)
    {
        GameObject effect = Instantiate(parryEffectUI, positionToSpawn, Quaternion.identity);
        effect.transform.parent = ParentManager.instance.effects;
        Destroy(effect, 2);
    }
    public void SpawnMeleeEffect(Vector3 pos, int type, Quaternion rotation)
    {
        GameObject hitEffectToInstantiate;
        if (type == 1)
        {
            hitEffectToInstantiate = hitEnemyMeleeCrit;
        }
        else if (type == 2)
        {
            hitEffectToInstantiate = hitEnemyOnlySound;
        }
        else
        {
            hitEffectToInstantiate = hitEnemyMelee;
        }

        GameObject bulletHole = Instantiate(hitEffectToInstantiate, pos, rotation, ParentManager.instance.effects);
        Destroy(bulletHole, 2f);
    }
    public void SpawnMessage(string text)
    {
        Vector3 spawnPos = new Vector3(GameManager.instance.XROrigin.transform.position.x + GameManager.instance.cam.transform.forward.x, GameManager.instance.cam.transform.position.y + GameManager.instance.cam.transform.forward.y, GameManager.instance.XROrigin.transform.position.z + GameManager.instance.cam.transform.forward.z);
        GameObject canvas = Instantiate(popUpMessage, spawnPos, Quaternion.identity, ParentManager.instance.effects);
        canvas.GetComponent<PopUpMessage>().SetMessage(text);
        Destroy(canvas, 2f);
    }

    public void Key(Vector3 position , int type)
    {
        GameObject effectToInstantiate;
        if (type == 0)
        {
            effectToInstantiate = keyPickup;
        }
        else
        {
            effectToInstantiate = keyOpen;
        }
        GameObject sfx = Instantiate(effectToInstantiate, position, Quaternion.identity, ParentManager.instance.effects);
        Destroy(sfx, 2f);
    }
}
