using TMPro;
using UnityEngine;

public class Tac14 : MonoBehaviour
{
    [Header("Inputs")]
    public int magSize;
    public int damagePerPellet;
    public int stunPerPellet;
    public int numberOfPellets;
    public int bulletForce;
    public int penetrationAmount;
    public float offsetX;
    public float offsetY;

    [Header("References")]
    public Transform muzzle;
    public TextMeshProUGUI currentAmmoText;
    public ParticleSystem muzzleFlash;
    public GameObject linePrefab;

    private Color startColor;
    private SoundForGun soundForGun;
    private Animator gunAnim;

    private Vector3[] directionsToFire;
    private int currentAmmo;
    private bool cockingNeeded, projectilePenetration;

    private void Start()
    {
        soundForGun = GetComponent<SoundForGun>();
        startColor = currentAmmoText.color;
        currentAmmo = magSize;
        UpdateDial();
        if(numberOfPellets < 2)
        {
            numberOfPellets = 2;
        }
        directionsToFire = new Vector3[numberOfPellets];

    }


    public void Fire()
    {
        if (currentAmmo > 0 && !cockingNeeded)
        {
            //Fire
            directionsToFire[0] = muzzle.transform.forward;

            for(int i = 1; i < directionsToFire.Length; i++)
            {
                directionsToFire[i] = new Vector3(muzzle.transform.forward.x + Random.Range(-offsetX, offsetX), muzzle.transform.forward.y + Random.Range(-offsetY, offsetY), muzzle.transform.forward.z);


                Ray ray = new Ray(muzzle.position, directionsToFire[i]);
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
                            hit.transform.GetComponent<BodyPart>().TakeDamage(damagePerPellet, stunPerPellet, hit.transform.forward - hit.normal * bulletForce);
                            if (hit.transform.GetComponent<BodyPart>() != null && hit.transform.GetComponent<BodyPart>().crit)
                            {
                                EffectManager.instance.SpawnBulletHole(hit, 2);
                            }
                            else if (hit.transform.GetComponent<BodyPart>() != null && hit.transform.GetComponent<BodyPart>().bodyPart == 1)
                            {
                                EffectManager.instance.SpawnBulletHole(hit, 3);
                            }
                            else if (hit.transform.GetComponent<BodyPart>() != null)
                            {
                                EffectManager.instance.SpawnBulletHole(hit, 1);
                            }
                        }
                        else if (hit.transform.GetComponent<Target>() != null)
                        {
                            hit.transform.GetComponent<Target>().Hit(hit.point);
                            EffectManager.instance.SpawnBulletHole(hit, 4);
                        }
                        else
                        {
                            EffectManager.instance.SpawnBulletHole(hit, 0);
                        }
                        if (projectilePenetration)
                        {
                            Ray penRay = new Ray(hit.point + ray.direction * penetrationAmount, -ray.direction);
                            RaycastHit penHit;
                            if (hit.collider.Raycast(penRay, out penHit, penetrationAmount))
                            {
                                Ray secondBullet = new(penHit.point, directionsToFire[i]);
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
                                            secondHit.transform.GetComponent<BodyPart>().TakeDamage(damagePerPellet, stunPerPellet, secondHit.transform.forward - hit.normal * bulletForce);

                                            if (secondHit.transform.GetComponent<BodyPart>() != null && secondHit.transform.GetComponent<BodyPart>().crit)
                                            {
                                                EffectManager.instance.SpawnBulletHole(hit, 2);
                                            }
                                            else if (hit.transform.GetComponent<BodyPart>() != null && hit.transform.GetComponent<BodyPart>().bodyPart == 1)
                                            {
                                                EffectManager.instance.SpawnBulletHole(hit, 3);
                                            }
                                            else if (secondHit.transform.GetComponent<BodyPart>() != null)
                                            {
                                                EffectManager.instance.SpawnBulletHole(hit, 1);
                                            }
                                        }
                                        else
                                        {
                                            EffectManager.instance.SpawnBulletHole(secondHit, 0);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }



            }
            //Spawn the lines
            for (int i = 0; i < directionsToFire.Length; i++)
            {
                EffectManager.instance.ShotGunLine(muzzle.transform.position, directionsToFire[i]);
            }


            gunAnim.Play("Fire");
            soundForGun.Fire();
            currentAmmo--;
            cockingNeeded = true;
            muzzleFlash.Play();
            UpdateDial();
        }
        else if(currentAmmo <= 0)
        {
            //Empty
            soundForGun.Empty();
            EmptyColor();
            Invoke(nameof(DefaultColor), 0.1f);
        }
        else
        {
            //Need to be cocked
            soundForGun.Empty();
            Debug.Log("Need to cock the gun");
        }
    }

    public void CockingGun()
    {
        if(currentAmmo > 0)
        {
            cockingNeeded = false;
        }
    }

    public void AddSlug(int numberOfSlugs)
    {
        currentAmmo += numberOfSlugs;
        if(currentAmmo > magSize)
        {
            currentAmmo = magSize;
        }
        UpdateDial();
    }
    public void UpdateDial()
    {
        currentAmmoText.text = currentAmmo.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        soundForGun.Drop();
    }
    private void DefaultColor()
    {
        currentAmmoText.color = startColor;
    }
    private void EmptyColor()
    {
        currentAmmoText.color = Color.red;
    }


    //Upgrades
    public void ProjectilePenetration(bool state)
    {
        projectilePenetration = state;
    }

}
