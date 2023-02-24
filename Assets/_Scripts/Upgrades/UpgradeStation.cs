using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    public float fanSoundSmoothTime;
    public AudioSource buttonSource;
    public AudioClip screenOn, screenOff;
    public Animator screenAnim;

    private AudioSource upgradeStationSource;
    private float currentVolume, targetVolume;

    private void Start()
    {
        upgradeStationSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, Time.deltaTime * fanSoundSmoothTime);
        upgradeStationSource.volume = currentVolume;
    }

    public void ScreenOn()
    {
        buttonSource.PlayOneShot(screenOn);
        screenAnim.SetBool("ScreenOn", true);
        targetVolume = 1;
    }
    public void ScreenOff()
    {
        buttonSource.PlayOneShot(screenOff);
        screenAnim.SetBool("ScreenOn", false);
        targetVolume = 0;
    }
}
