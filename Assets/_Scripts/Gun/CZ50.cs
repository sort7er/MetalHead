using System.Net;
using UnityEngine;

public class CZ50 : MonoBehaviour
{

    public int bulletForce, penetrationAmount;

    [Header("References")]
    public GameObject casingPrefab, laser;
    public Transform muzzle, casingPoint, leftAttach, rightAttach;
    public Dial doubleDial, singleDial;
    public MeshRenderer doubleZero, singleZero;
    public Material defaultMaterial, emptyMaterial;
    public ParticleSystem muzzleFlash;

    private TwoHandGrab twoHandGrab;
    private Recoil recoil;
    private Mag magInGun;
    private Animator cz50Anim;
    private SoundForGun soundForGun;
    private int currentAmmo;
    private int damage, singleDigit, doubleDigit;
    private bool reloadNeeded, firstDialUpdate, projectilePenetration;

    private void Start()
    {
        twoHandGrab = GetComponent<TwoHandGrab>();
        recoil = GetComponent<Recoil>();
        cz50Anim = GetComponent<Animator>();
        soundForGun = GetComponent<SoundForGun>();
        DefaultColor();
    }

    public void Fire()
    {
        if(currentAmmo > 0 && !reloadNeeded)
        {
            magInGun.Fire();
            recoil.StartRecoil();
            currentAmmo--;
            soundForGun.Fire();
            cz50Anim.SetTrigger("Fire");
            UpdateDial();
            muzzleFlash.Play();
            //EffectManager.instance.Fire(muzzle, damage, bulletSpeed);
            Ray ray = new Ray(muzzle.position, muzzle.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 5000, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if(!hit.transform.CompareTag("Player") && !hit.transform.CompareTag("Gun"))
                {
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(hit.transform.forward - hit.normal * bulletForce, ForceMode.Impulse);
                    }
                    if (hit.transform.CompareTag("Enemy"))
                    {
                        hit.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
                    }
                    EffectManager.instance.SpawnBulletHole(hit);
                    if (projectilePenetration)
                    {
                        Ray penRay = new Ray(hit.point + ray.direction * penetrationAmount, -ray.direction);
                        RaycastHit penHit;
                        if (hit.collider.Raycast(penRay, out penHit, penetrationAmount))
                        {
                            Ray secondBullet = new(penHit.point, muzzle.forward);
                            RaycastHit secondHit;
                            if (Physics.Raycast(secondBullet, out secondHit, 5000, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                            {
                                if (!secondHit.transform.CompareTag("Player") && !secondHit.transform.CompareTag("Gun"))
                                {
                                    if (secondHit.rigidbody != null)
                                    {
                                        secondHit.rigidbody.AddForce(secondHit.transform.forward - secondHit.normal * bulletForce, ForceMode.Impulse);
                                    }
                                    if (secondHit.transform.CompareTag("Enemy"))
                                    {
                                        secondHit.transform.GetComponent<EnemyHealth>().TakeDamage(damage);
                                    }
                                    EffectManager.instance.SpawnBulletHole(secondHit);
                                }
                            }
                        }
                    }

                }
            }


        }
        else
        {
            reloadNeeded = true;
            soundForGun.Empty();
            EmptyColor();
            Invoke("DefaultColor", 0.1f);
        }
        
    }

    public void Grab()
    {
        soundForGun.Grab();
        if(GameManager.instance.CheckHand("Gun") == 1)
        {
            twoHandGrab.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabPistol(true);
        }
        if (GameManager.instance.CheckHand("Gun") == 2)
        {
            twoHandGrab.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabPistol(true);
        }
    }
    public void Release()
    {
        GameManager.instance.leftHand.GrabPistol(false);
        GameManager.instance.rightHand.GrabPistol(false);
    }

    public void Reload()
    {
        reloadNeeded = false;
        UpdateDial();
    }
    public void MagOut()
    {
        reloadNeeded = true;
        currentAmmo = 0;
        UpdateDial();
    }
    public void MagIn(Mag mag)
    {
        magInGun = mag;
        currentAmmo = magInGun.GetCurrentAmmoFromMag();
        if (!firstDialUpdate)
        {
            UpdateDial();
            firstDialUpdate = true;
        }
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
    public void Casing()
    {
        GameObject casing = Instantiate(casingPrefab, casingPoint.position, casingPoint.rotation);
        casing.transform.parent = ParentManager.instance.bullets;
    }
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
    public void ProjectilePenetration(bool state)
    {
        projectilePenetration = state;
    }
    public void Laser(bool state)
    {
        laser.SetActive(state);
    }
}
