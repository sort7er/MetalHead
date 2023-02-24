using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    public AudioSource buttonSource;
    public AudioClip screenOn, screenOff;
    public Animator screenAnim;

    private AudioSource upgradeStationSource;

    private void Start()
    {
        upgradeStationSource = GetComponent<AudioSource>();
    }

    public void ScreenOn()
    {
        buttonSource.PlayOneShot(screenOn);
        screenAnim.SetBool("ScreenOn", true);
        upgradeStationSource.Play();
    }
    public void ScreenOff()
    {
        buttonSource.PlayOneShot(screenOff);
        screenAnim.SetBool("ScreenOn", false);
        upgradeStationSource.Stop();
    }
}
