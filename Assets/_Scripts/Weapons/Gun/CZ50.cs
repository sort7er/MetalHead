using TMPro;
using UnityEngine;

public class CZ50 : MonoBehaviour
{

    public int bulletForce, penetrationAmount;

    [Header("References")]
    public GameObject casingPrefab, laser;
    public Transform muzzle, casingPoint, leftAttach, rightAttach;
    public TextMeshProUGUI ammoText;
    public ParticleSystem muzzleFlash;
    public Icons icons;
    public Slide slide;

    [HideInInspector] public bool reloadNeeded;

    private ReleaseMag releaseMag;
    private Color startColor;
    private TwoHandGrab twoHandGrab;
    private Recoil recoil;
    private Mag magInGun;
    private Animator cz50Anim;
    private SoundForGun soundForGun;
    private int currentAmmo;
    private int damage;
    private bool firstDialUpdate, projectilePenetration, slideBack, left, isHolding, questAnimSet, showHelp;

    private void Start()
    {
        startColor = ammoText.color;
        twoHandGrab = GetComponent<TwoHandGrab>();
        recoil = GetComponent<Recoil>();
        cz50Anim = GetComponent<Animator>();
        soundForGun = GetComponent<SoundForGun>();
        releaseMag= GetComponent<ReleaseMag>();
        DefaultColor();
    }

    private void Update()
    {
        if (isHolding)
        {
            CheckForCurrentIcon();
        }
        Debug.Log(showHelp);
    }

    public void Fire()
    {
        if (currentAmmo > 0 && !reloadNeeded)
        {
            if (!slideBack)
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
                if (Physics.Raycast(ray, out hit, 5000, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    if (!hit.transform.CompareTag("Player") && !hit.transform.CompareTag("Gun") && !hit.transform.CompareTag("InvisibleWall") && hit.transform.gameObject.layer != 9)
                    {
                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(hit.transform.forward - hit.normal * bulletForce, ForceMode.Impulse);
                        }
                        if (hit.transform.CompareTag("Enemy"))
                        {
                            hit.transform.GetComponent<BodyPart>().TakeDamage(damage, damage, hit.transform.forward - hit.normal * bulletForce);
                            if (hit.transform.GetComponent<BodyPart>() != null && hit.transform.GetComponent<BodyPart>().crit)
                            {
                                EffectManager.instance.SpawnBulletHole(hit.transform, hit.point, hit.normal, 2);
                            }
                            else if (hit.transform.GetComponent<BodyPart>() != null && hit.transform.GetComponent<BodyPart>().bodyPart == 1)
                            {
                                EffectManager.instance.SpawnBulletHole(hit.transform, hit.point, hit.normal, 3);
                            }
                            else if(hit.transform.GetComponent<BodyPart>() != null)
                            {
                                EffectManager.instance.SpawnBulletHole(hit.transform, hit.point, hit.normal, 1);
                            }
                        }
                        else if(hit.transform.GetComponent<Target>() != null)
                        {
                            hit.transform.GetComponent<Target>().Hit(hit.point);
                            EffectManager.instance.SpawnBulletHole(hit.transform, hit.point, hit.normal, 4);
                        }
                        else
                        {
                            EffectManager.instance.SpawnBulletHole(hit.transform, hit.point, hit.normal, 0);
                        }
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
                                    if (!secondHit.transform.CompareTag("Player") && !secondHit.transform.CompareTag("Gun") && hit.transform.gameObject.layer != 9)
                                    {
                                        if (secondHit.rigidbody != null)
                                        {
                                            secondHit.rigidbody.AddForce(secondHit.transform.forward - secondHit.normal * bulletForce, ForceMode.Impulse);
                                        }
                                        if (secondHit.transform.CompareTag("Enemy"))
                                        {
                                            secondHit.transform.GetComponent<BodyPart>().TakeDamage(damage, damage, secondHit.transform.forward - hit.normal * bulletForce);
                                            
                                            if (secondHit.transform.GetComponent<BodyPart>() != null && secondHit.transform.GetComponent<BodyPart>().crit)
                                            {
                                                EffectManager.instance.SpawnBulletHole(secondHit.transform, secondHit.point, secondHit.normal, 2);
                                            }
                                            else if (hit.transform.GetComponent<BodyPart>() != null && hit.transform.GetComponent<BodyPart>().bodyPart == 1)
                                            {
                                                EffectManager.instance.SpawnBulletHole(secondHit.transform, secondHit.point, secondHit.normal, 3);
                                            }
                                            else if(secondHit.transform.GetComponent<BodyPart>() != null)
                                            {
                                                EffectManager.instance.SpawnBulletHole(secondHit.transform, secondHit.point, secondHit.normal, 1);
                                            }
                                        }
                                        else
                                        {
                                            EffectManager.instance.SpawnBulletHole(secondHit.transform, secondHit.point, secondHit.normal, 0);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                soundForGun.Empty();
            }



        }
        else
        {
            showHelp = true;
            icons.ChangeColor(Color.red);
            reloadNeeded = true;
            soundForGun.Empty();
            EmptyColor();
            Invoke("DefaultColor", 0.1f);
        }
        
    }

    public void Grab()
    {
        isHolding= true;
        soundForGun.Grab();
        if(GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            twoHandGrab.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabPistol(true);
            left = true;
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            twoHandGrab.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabPistol(true);
            left = false;
        }
        GameManager.instance.CheckCurrentAmmoBag(1);
    }
    public void Release()
    {
        isHolding= false;
        if (!GameManager.instance.leftHand.IsHoldingSomething())
        {
            GameManager.instance.leftHand.GrabPistol(false);
        }
        if (!GameManager.instance.rightHand.IsHoldingSomething())
        {
            GameManager.instance.rightHand.GrabPistol(false);
        }
        GameManager.instance.ReleaseWeapon(1);
        icons.IconDone();
        ReleaseControllers();
        showHelp = false;
    }

    public void Reload()
    {
        reloadNeeded = false;
        showHelp= false;
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
        ammoText.text = currentAmmo.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        soundForGun.Drop();
    }
    private void DefaultColor()
    {
        ammoText.color = startColor;
    }
    private void EmptyColor()
    {
        ammoText.color = Color.red;
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
    public void SlideBack(bool state)
    {
        slideBack = state;
    }

    public void CheckForCurrentIcon()
    {
        if(releaseMag.InsertMagNeeded())
        {
            icons.ShowIcon(0, releaseMag.magLocation);
        }
        else if (releaseMag.reloadValid)
        {
            icons.ShowIcon(0, slide.transform);
        }
        else
        {
            icons.IconDone();
        }

        if(currentAmmo <= 0 && !releaseMag.InsertMagNeeded() && !releaseMag.release && !releaseMag.reloadValid)
        {
            if (!questAnimSet && showHelp)
            {
                if (left)
                {
                    GameManager.instance.leftHand.questController.QuestActive(true);
                    GameManager.instance.leftHand.questController.Secondary();
                }
                else
                {
                    GameManager.instance.rightHand.questController.QuestActive(true);
                    GameManager.instance.rightHand.questController.Secondary();
                }
                questAnimSet = true;
            }   
        }
        else if(questAnimSet)
        {
            ReleaseControllers();                    
        }

        
    }

    private void ReleaseControllers()
    {
        GameManager.instance.rightHand.questController.QuestActive(false);
        GameManager.instance.leftHand.questController.QuestActive(false);
        questAnimSet = false;
    }
}
