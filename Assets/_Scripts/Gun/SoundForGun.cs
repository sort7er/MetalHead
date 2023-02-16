using UnityEngine;

public class SoundForGun : MonoBehaviour
{
    public AudioClip[] gunShots;
    public AudioClip[] gunGrab;
    public AudioClip[] gunEmpty;
    public AudioClip[] slide;

    private AudioSource gunSource;

    private void Start()
    {
        gunSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        gunSource.PlayOneShot(gunShots[Random.Range(0, gunShots.Length)]);
    }
    public void Empty()
    {
        gunSource.PlayOneShot(gunEmpty[Random.Range(0, gunShots.Length)]);
    }
    public void Grab()
    {
        gunSource.PlayOneShot(gunGrab[Random.Range(0, gunShots.Length)]);
    }
    public void Slide(int index)
    {
        gunSource.PlayOneShot(slide[index]);
    }
}