using UnityEngine;
using TMPro;

public class UpgradeStation : MonoBehaviour
{
    public float fanSoundSmoothTime;
    public AudioSource buttonSource;
    public AudioClip screenOn, screenOff;
    public Animator screenAnim;
    public TextMeshProUGUI titleText, normalText, currencyText;
    public GameObject nut;

    private int currencyOnScreen;
    private AudioSource upgradeStationSource;
    private TypeWriterText titleType, normalType, currencyType;
    private float currentVolume, targetVolume;
    private bool magnetIn, isOn;

    private void Start()
    {
        upgradeStationSource = GetComponent<AudioSource>();
        titleType = titleText.GetComponent<TypeWriterText>();
        normalType = normalText.GetComponent<TypeWriterText>();
        currencyType = currencyText.GetComponent<TypeWriterText>();
        nut.SetActive(false);
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
        normalText.text = "";
        titleType.StartTyping();
        Invoke("MetalText", 1f);
    }

    private void ScreenOff()
    {
        isOn = false;
        CancelInvoke();
        buttonSource.PlayOneShot(screenOff);
        screenAnim.SetBool("ScreenOn", false);
        targetVolume = 0;
        ResetType();
        nut.SetActive(false);
    }

    public void MagnetIn()
    {
        magnetIn = true;
        if (isOn)
        {
            MetalText();
        }
    }
    public void MagnetOut()
    {
        magnetIn = false;
        MetalText();
    }

    private void MetalText()
    {
        ResetType();
        if (!magnetIn)
        {
            CancelInvoke();
            currencyText.text = "";
            normalText.text = "Please insert magnet";
            normalType.StartTyping();
            nut.SetActive(false);
        }
        else
        {
            nut.SetActive(true);
            normalText.text = "";
            currencyText.text = currencyOnScreen.ToString("00000");
            currencyType.StartTyping();
            Invoke("InsertWeapon", 2f);
            
        }
        
    }

    private void ResetType()
    {
        currencyType.StopTyping();
        normalType.StopTyping();
    }


    private void InsertWeapon()
    {
        normalText.text = "Please insert weapon to upgrade";
        normalType.StartTyping();
    }

}
