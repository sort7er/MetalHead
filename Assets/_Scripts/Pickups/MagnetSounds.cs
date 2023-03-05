using UnityEngine;

public class MagnetSounds : MonoBehaviour
{
    public AudioClip[] magnetDrop;
    public AudioClip[] magnetActivate;


    private AudioSource magnetSource;
    private bool allreadyPlayed;

    private void Start()
    {
        magnetSource = GetComponent<AudioSource>();
    }
    public void MagnetActivate(int index)
    {
        magnetSource.PlayOneShot(magnetActivate[index]);
    }
    public void MagnetDrop()
    {
        if(!allreadyPlayed)
        {
            magnetSource.PlayOneShot(magnetDrop[Random.Range(0, magnetDrop.Length)]);
            allreadyPlayed = true;
            Invoke("WaitTime", 0.5f);
        }
    }
    private void WaitTime()
    {
        allreadyPlayed = false;
    }
}
