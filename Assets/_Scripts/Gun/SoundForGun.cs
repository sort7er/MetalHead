using UnityEngine;

public class SoundForGun : MonoBehaviour
{
    public float soundRange;

    public AudioClip[] gunShots;
    public AudioClip[] gunGrab;
    public AudioClip[] gunEmpty;
    public AudioClip[] slide;
    public AudioClip[] magazine;

    private AudioSource gunSource;
    private Collider[] possibleEnemiesWhoHeardMe;

    private void Start()
    {
        gunSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        gunSource.PlayOneShot(gunShots[Random.Range(0, gunShots.Length)]);
        //CheckIfEnemyCanHearTheSound();
    }
    public void Empty()
    {
        gunSource.PlayOneShot(gunEmpty[Random.Range(0, gunEmpty.Length)]);
    }
    public void Grab()
    {
        gunSource.PlayOneShot(gunGrab[Random.Range(0, gunGrab.Length)]);
    }
    public void Magazine(int index)
    {
        gunSource.PlayOneShot(magazine[index]);
    }
    public void Slide(int index)
    {
        gunSource.PlayOneShot(slide[index]);
    }
    //private void CheckIfEnemyCanHearTheSound()
    //{
    //    possibleEnemiesWhoHeardMe = Physics.OverlapSphere(transform.position, soundRange, 11);

    //    foreach (Collider enemy in possibleEnemiesWhoHeardMe)
    //    {
    //        if(enemy.GetComponent<RunningEnemy>() != null)
    //        {
    //            Debug.Log("Yes");
    //            //enemy.GetComponent<RunningEnemy>().AlertEnemy();
    //        }
    //    }
    //}
}
