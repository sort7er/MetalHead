using UnityEngine;
using TMPro;

public class UpgradeStation : MonoBehaviour
{
    public float fanSoundSmoothTime;
    public AudioSource buttonSource;
    public AudioClip screenOn, screenOff;
    public Animator screenAnim;
    public TextMeshProUGUI currencyText;
    public TypeWriterText welcomeText, metalsText;

    private int currencyOnScreen;
    private AudioSource upgradeStationSource;
    private float currentVolume, targetVolume;
    private bool magnetIn, isOn;

    private void Start()
    {
        upgradeStationSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, Time.deltaTime * fanSoundSmoothTime);
        upgradeStationSource.volume = currentVolume;
    }

    public void Screen()
    {
        if (!isOn)
        {
            ScreenOn();
        }
        else
        {
            ScreenOff();
        }
    }

    private void ScreenOn()
    {
        isOn = true;
        buttonSource.PlayOneShot(screenOn);
        screenAnim.SetBool("ScreenOn", true);
        targetVolume = 1;
        currencyOnScreen = GameManager.instance.magnet.GetMetalsCollected();
        currencyText.text = "";
        welcomeText.StartTyping();
        Invoke("MetalText", 1f);
    }

    private void ScreenOff()
    {
        isOn = false;
        CancelInvoke();
        buttonSource.PlayOneShot(screenOff);
        screenAnim.SetBool("ScreenOn", false);
        targetVolume = 0;
    }

    public void MagnetIn()
    {
        magnetIn = true;
        MetalText();
    }
    public void MagnetOut()
    {
        magnetIn = false;
        MetalText();
    }

    private void MetalText()
    {
        if (!magnetIn)
        {
            currencyText.text = "Please insert magnet";
        }
        else
        {
            currencyText.text = "Metals: " + currencyOnScreen.ToString("00000");
        }
        metalsText.StartTyping();
    }

}
