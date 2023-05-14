using UnityEngine;

public class SoundForGun : MonoBehaviour
{
    public LayerMask enemyMask;

    public AudioClip[] gunShots;
    public AudioClip[] gunGrab;
    public AudioClip[] gunEmpty;
    public AudioClip[] slide;
    public AudioClip[] slideBack;
    public AudioClip[] magazine;

    private AudioSource gunSource;


    private void Start()
    {
        gunSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        gunSource.PlayOneShot(gunShots[Random.Range(0, gunShots.Length)]);
        CheckSound.instance.CheckIfEnemyCanHearTheSound(transform.position, 25, false);
    }
    public void Empty()
    {
        if(gunEmpty.Length > 0)
        {
            gunSource.PlayOneShot(gunEmpty[Random.Range(0, gunEmpty.Length)]);
        }
        else
        {
            //Debug.Log("Missing empty audio");
        }
    }
    public void Grab()
    {
        
        if (gunGrab.Length > 0)
        {
            gunSource.PlayOneShot(gunGrab[Random.Range(0, gunGrab.Length)]);
        }
        else
        {
            //Debug.Log("Missing gungrab audio");
        }
    }
    public void Drop()
    {
        if (gunGrab.Length > 0)
        {
            gunSource.PlayOneShot(gunGrab[Random.Range(0, gunGrab.Length)]);
            CheckSound.instance.CheckIfEnemyCanHearTheSound(transform.position, 5, true);
        }
        else
        {
            //Debug.Log("Missing drop audio");
        }
        
    }
    public void Magazine(int index)
    {
        gunSource.PlayOneShot(magazine[index]);
    }
    public void Slide()
    {
        gunSource.PlayOneShot(slide[Random.Range(0, slide.Length)]);
    }
    public void SlideBack()
    {
        gunSource.PlayOneShot(slideBack[Random.Range(0, slideBack.Length)]);
    }
}
