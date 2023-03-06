using UnityEngine;

public class MagnetSounds : MonoBehaviour
{
    public float loopSoundSmoothTime;

    public AudioClip[] magnetDrop;
    public AudioClip[] magnetActivate;
    public AudioSource magnetSource;
    public AudioSource magnetLoopSource;

    private float currentVolume, targetVolume;

    private bool allreadyPlayed;

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
    public void MagnetOn()
    {
        targetVolume= 1.0f;
    }
    public void MagnetOff()
    {
        targetVolume= 0.0f;
    }
    private void Update()
    {
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, Time.deltaTime * loopSoundSmoothTime);
        magnetLoopSource.volume = currentVolume;
    }
}
