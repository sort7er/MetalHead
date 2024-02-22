using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    [Header("VisualEffects")]
    public static EffectManager instance;
    public GameObject bulletPrefab;
    public GameObject bulletHolePrefab;
    public GameObject bulletHolePrefabWithSound;
    public GameObject hitEnemy;
    public GameObject hitPlayer;
    public GameObject hitEnemyBarrel;
    public GameObject hitEnemyCrit;
    public GameObject hitEnemyMelee;
    public GameObject hitWood;
    public GameObject hitEnemyMeleeCrit;

    public GameObject parryEffect;
    public GameObject parryEffectUI;
    public GameObject popUpMessage;
    public GameObject enemyDeadEffect;
    public GameObject[] pickUp;
    public GameObject[] pickUpEffect;
    public GameObject pickUpRingEffect;
    public GameObject shotgunLines;
    public GameObject bombExplotion;

    [Header("SoundEffects")]
    public GameObject keyPickup;
    public GameObject keyOpen;
    public GameObject parrySoundEffect;
    public GameObject parryFailedSoundEffect;
    public GameObject hitEnemyOnlySound;


    //Pools
    private GameObject[] arrayBullets;
    private int poolBullets = 20;
    private int currentBullet;

    private GameObject[] arrayBulletHoles;
    private int poolBulletHoles = 20;
    private int currentBulletHole;

    private GameObject[] arrayBulletHoleWithSounds;
    private int poolBulletHoleWithSound = 20;
    private int currentBulletHoleWithSound;

    private GameObject[] arrayHitPlayers;
    private int poolHitPlayer = 10;
    private int currentHitPlayer;

    public GameObject[] arrayHitEnemies;
    private int poolHitEnemy = 7;
    private int currentHitEnemy;

    private GameObject[] arrayHitBarrels;
    private int poolHitBarrel = 5;
    private int currentHitBarrel;

    private GameObject[] arrayHitCrits;
    private int poolCrit = 7;
    private int currentCrit;

    private GameObject[] arrayHitMelees;
    private int poolMelee = 10;
    private int currentMelee;

    private GameObject[] arrayHitCritMelees;
    private int poolCritMelee = 10;
    private int currentCritMelee;

    private GameObject[] arrayHitWoods;
    private int poolWood = 10;
    private int currentWood;

    private GameObject[] arrayParrys;
    private int poolParry = 5;
    private int currentParry;

    private GameObject[] arrayParryUIs;
    private int poolParryUI = 5;
    private int currentParryUIs;

    private GameObject[] arrayMessages;
    private int poolMessages = 5;
    private int currentMessage;

    private GameObject[] arrayDeads;
    private int poolDead = 5;
    private int currentDead = 5;

    private GameObject[] arrayRings;
    private int poolRing = 5;
    private int currentRing = 5;

    private GameObject[] arrayLines;
    private int poolLine = 40;
    private int currentLine;

    private GameObject[] arrayExplotions;
    private int poolExplo = 10;
    private int currentExplo;

    private GameObject[] arrayKeys;
    private int poolKey = 4;
    private int currentKey;

    private GameObject[] arrayKeyOpens;
    private int poolKeyOpen = 4;
    private int currentKeyOpen;

    private GameObject[] arrayParrySounds;
    private int poolParrySound = 10;
    private int currentParrySound;

    private GameObject[] arrayParryFails;
    private int poolParryFail = 10;
    private int currentParryFail;

    private GameObject[] arrayHitOnlySounds;
    private int poolOnlySound = 10;
    private int currentOnlySound;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetUpPool(bulletPrefab, ParentManager.instance.bullets, ref arrayBullets, poolBullets, ref currentBullet);
        SetUpPool(bulletHolePrefab, ParentManager.instance.bullets, ref arrayBulletHoles, poolBulletHoles, ref currentBulletHole);
        SetUpPool(bulletHolePrefabWithSound, ParentManager.instance.bullets, ref arrayBulletHoleWithSounds, poolBulletHoleWithSound, ref currentBulletHoleWithSound);
        SetUpPool(hitWood, ParentManager.instance.bullets, ref arrayHitWoods, poolWood, ref currentWood);
        SetUpPool(hitEnemy, ParentManager.instance.bullets, ref arrayHitEnemies, poolHitEnemy, ref currentHitEnemy);
        SetUpPool(hitEnemyCrit, ParentManager.instance.bullets, ref arrayHitCrits, poolCrit, ref currentCrit);

        SetUpPool(hitPlayer, ParentManager.instance.effects, ref arrayHitPlayers, poolHitPlayer, ref currentHitPlayer);
        SetUpPool(hitEnemyBarrel, ParentManager.instance.effects, ref arrayHitBarrels, poolHitBarrel, ref currentHitBarrel);


        SetUpPool(hitEnemyMelee, ParentManager.instance.effects, ref arrayHitMelees, poolMelee, ref currentMelee);
        SetUpPool(hitEnemyMeleeCrit, ParentManager.instance.effects, ref arrayHitCritMelees, poolCritMelee, ref currentCritMelee);


        SetUpPool(parryEffect, ParentManager.instance.effects, ref arrayParrys, poolParry, ref currentParry);
        SetUpPool(parryEffectUI, ParentManager.instance.effects, ref arrayParryUIs, poolParryUI, ref currentParryUIs);
        SetUpPool(popUpMessage, ParentManager.instance.effects, ref arrayMessages, poolMessages, ref currentMessage);
        SetUpPool(enemyDeadEffect, ParentManager.instance.effects, ref arrayDeads, poolDead, ref currentDead);

        SetUpPool(pickUpRingEffect, ParentManager.instance.effects, ref arrayRings, poolRing, ref currentRing);
        SetUpPool(shotgunLines, ParentManager.instance.effects, ref arrayLines, poolLine, ref currentLine);
        SetUpPool(bombExplotion, ParentManager.instance.effects, ref arrayExplotions, poolExplo, ref currentExplo);

        SetUpPool(keyPickup, ParentManager.instance.effects, ref arrayKeys, poolKey, ref currentKey);
        SetUpPool(keyOpen, ParentManager.instance.effects, ref arrayKeyOpens, poolKeyOpen, ref currentKeyOpen);
        SetUpPool(parrySoundEffect, ParentManager.instance.effects, ref arrayParrySounds, poolParrySound, ref currentParrySound);
        SetUpPool(parryFailedSoundEffect, ParentManager.instance.effects, ref arrayParryFails, poolParryFail, ref currentParryFail);
        SetUpPool(hitEnemyOnlySound, ParentManager.instance.effects, ref arrayHitOnlySounds, poolOnlySound, ref currentOnlySound);
    }




    public void Fire(Transform muzzle, int damage, float speed)
    {
        GameObject bullet = arrayBullets[currentBullet];
        UpdatePool(ref currentBullet, poolBullets);
        SetTransformAndActivate(bullet.gameObject, muzzle.position, muzzle.rotation);
        bullet.GetComponent<Bullet>().Fire(damage, speed);
    }



    public void SpawnBulletHole(Transform hit, Vector3 point, Vector3 normal, int type)
    {
        GameObject hitEffect;
        float waitTime = 1.5f;
        float pitch;
        if(type == 1 || type == 3)
        {
            UpdatePool(ref currentHitEnemy, poolHitEnemy);
            hitEffect = arrayHitEnemies[currentHitEnemy];
            if(type == 3)
            {
                pitch = 1.4f;
            }
            else
            {
                pitch = 1;
            }
        }
        else if(type == 2)
        {
            UpdatePool(ref currentCrit, poolCrit);
            hitEffect = arrayHitCrits[currentCrit];
            pitch = 1;
        }
        else if(type==4)
        {
            UpdatePool(ref currentWood, poolWood);
            hitEffect = arrayHitWoods[currentWood];
            waitTime = 8;
            pitch = 1;
        }
        else if(type == 5)
        {
            UpdatePool(ref currentBulletHoleWithSound, poolBulletHoleWithSound);
            hitEffect = arrayBulletHoleWithSounds[currentBulletHoleWithSound];
            waitTime = 8;
            pitch = 1;
        }
        else
        {
            UpdatePool(ref currentBulletHole, poolBulletHoles);
            hitEffect = arrayBulletHoles[currentBulletHole];
            waitTime = 8;
            pitch = 1;
        }

        if(hitEffect == null)
        {
            Debug.Log("Fakk");
        }

        SetTransformAndActivate(hitEffect, point + normal * 0.001f, Quaternion.LookRotation(normal));

        hitEffect.transform.Rotate(Vector3.forward * Random.Range(-180, 180));

        if(hitEffect.GetComponent<AudioSource>() != null)
        {
            hitEffect.GetComponent<AudioSource>().pitch = pitch;
        }
        if (hit.GetComponent<Rigidbody>() != null)
        {
            hitEffect.transform.parent = hit.transform;
        }
        else
        {
            hitEffect.transform.parent = ParentManager.instance.effects;
        }

        StartCoroutine(DisableEffect(hitEffect, waitTime));
    }

    public void SpawnMeleeEffect(Vector3 pos, int type, Quaternion rotation)
    {
        GameObject hitEffect;
        if (type == 1)
        {
            hitEffect = arrayHitCritMelees[currentCritMelee];
            UpdatePool(ref currentCritMelee, poolCritMelee);
        }
        else if (type == 2)
        {
            hitEffect = arrayHitOnlySounds[currentOnlySound];
            UpdatePool(ref currentOnlySound, poolOnlySound);
        }
        else
        {
            hitEffect = arrayHitMelees[currentMelee];
            UpdatePool(ref currentMelee, poolMelee);
        }

        SetTransformAndActivate(hitEffect, pos, rotation);

        StartCoroutine(DisableEffect(hitEffect, 2));

    }


    public void SpawnPickups(Transform enemy, int numberOfPickups, bool willDisapear)
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
            pickup.GetComponent<Pickup>().WillDisapear(willDisapear);
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
        GameObject effect = arrayDeads[currentDead];
        UpdatePool(ref currentDead, poolDead);
        SetTransformAndActivate(effect, pointToSpawn.position, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 4));
    }
    public void SpawnHitPlayerEffect(Vector3 positionToSpawn)
    {
        GameObject effect = arrayHitPlayers[currentHitPlayer];
        UpdatePool(ref currentHitPlayer, poolHitPlayer);
        SetTransformAndActivate(effect, positionToSpawn, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 2));
    }
    public void SpawnBarrelHitEnemy(Vector3 positionToSpawn)
    {
        GameObject effect = arrayHitBarrels[currentHitBarrel];
        UpdatePool(ref currentHitBarrel, poolHitBarrel);
        SetTransformAndActivate(effect, positionToSpawn, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 2));
    }
    public void SpawnParryEffect(Vector3 positionToSpawn)
    {
        GameObject effect = arrayParrys[currentParry];
        UpdatePool(ref currentParry, poolParry);
        SetTransformAndActivate(effect, positionToSpawn, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 2));
    }
    public void SpawnParrySoundEffect(Vector3 positionToSpawn, int type)
    {
        GameObject sfxToInstantiate;
        if (type == 1)
        {
            sfxToInstantiate = arrayParryFails[currentParryFail];
            UpdatePool(ref currentParryFail, poolParryFail);
        }
        else
        {
            sfxToInstantiate = arrayParrySounds[currentParrySound];
            UpdatePool(ref currentParrySound, poolParrySound);
        }
        SetTransformAndActivate(sfxToInstantiate, positionToSpawn, Quaternion.identity);
        StartCoroutine(DisableEffect(sfxToInstantiate, 2));

    }
    public void SpawnParryEffectUI(Vector3 positionToSpawn)
    {
        GameObject effect = arrayParryUIs[currentParryUIs];
        UpdatePool(ref currentParryUIs, poolParryUI);
        SetTransformAndActivate(effect, positionToSpawn, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 2));
    }
    public void PickUpRingEffect(Vector3 positionToSpawn)
    {
        GameObject effect = arrayRings[currentRing];
        UpdatePool(ref currentRing, poolRing);
        SetTransformAndActivate(effect, positionToSpawn, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 2));
    }
 
    public void SpawnMessage(string text, float multiplier)
    {
        Vector3 spawnPos = new Vector3(GameManager.instance.XROrigin.transform.position.x + GameManager.instance.cam.transform.forward.x, GameManager.instance.cam.transform.position.y + GameManager.instance.cam.transform.forward.y, GameManager.instance.XROrigin.transform.position.z + GameManager.instance.cam.transform.forward.z);
        GameObject canvas = arrayMessages[currentMessage];
        UpdatePool(ref currentMessage, poolMessages);
        SetTransformAndActivate(canvas, spawnPos, Quaternion.identity);

        canvas.GetComponent<PopUpMessage>().SetMessage(text);
        canvas.GetComponent<Animator>().SetFloat("AnimationSpeed", multiplier);

        StartCoroutine(DisableEffect(canvas, 2));

    }

    public void Key(Vector3 position , int type)
    {
        GameObject effectToInstantiate;
        if (type == 0)
        {
            effectToInstantiate = arrayKeys[currentKey];
            UpdatePool(ref currentKey, poolKey);
        }
        else
        {
            effectToInstantiate = arrayKeyOpens[currentKeyOpen];
            UpdatePool(ref currentKeyOpen, poolKeyOpen);
        }
        SetTransformAndActivate(effectToInstantiate, position, Quaternion.identity);

        StartCoroutine(DisableEffect(effectToInstantiate, 2));
    }

    public void ShotGunLine(Vector3 position, Vector3 direction)
    {
        GameObject line = arrayLines[currentLine];
        UpdatePool(ref currentLine, poolLine);

        SetTransformAndActivate(line, position, Quaternion.identity);

        line.transform.LookAt(position + direction);

        StartCoroutine(DisableEffect(line, 2));
    }
    public void BombExplotion(Vector3 position)
    {
        GameObject effect = arrayExplotions[currentExplo];
        UpdatePool(ref currentExplo, poolExplo);
        SetTransformAndActivate(effect, position, Quaternion.identity);
        StartCoroutine(DisableEffect(effect, 2));
    }
    private void SetUpPool(GameObject prefab, Transform parent, ref GameObject[] array, int pool, ref int current)
    {
        array = new GameObject[pool];
        current = 0;

        for (int i = 0; i < pool; i++)
        {
            array[i] = Instantiate(prefab, parent);
            array[i].gameObject.SetActive(false);
        }
    }
    private void UpdatePool(ref int current, int poolSize)
    {
        if (current < poolSize - 1)
        {
            current++;
        }
        else
        {
            current = 0;
        }
    }
    private void SetTransformAndActivate(GameObject effect, Vector3 pos, Quaternion rot)
    {
        effect.SetActive(true);
        effect.transform.position = pos;
        effect.transform.rotation = rot;
    }
    private IEnumerator DisableEffect(GameObject bulletHole, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        bulletHole.SetActive(false);
    }
}
