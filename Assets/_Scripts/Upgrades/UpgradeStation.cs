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
    public GameObject[] gun;

    private int currencyOnScreen;
    private InsertWeapon insertWeapon;
    private AudioSource upgradeStationSource;
    private TypeWriterText titleType, normalType, currencyType;
    private float currentVolume, targetVolume;
    private bool magnetIn, isOn;

    private void Start()
    {
        insertWeapon = GetComponentInChildren<InsertWeapon>();
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
        StartScreen();
    }

    private void ScreenOff()
    {
        isOn = false;
        CancelInvoke();
        buttonSource.PlayOneShot(screenOff);
        screenAnim.SetBool("ScreenOn", false);
        targetVolume = 0;
        AllOff();
    }

    public void MagnetIn()
    {
        magnetIn = true;
        insertWeapon.CanInsert(true);
        if (isOn)
        {
            MetalText();
        }
    }
    public void MagnetOut()
    {
        magnetIn = false;
        insertWeapon.CanInsert(false);
        if (isOn)
        {
            MetalText();
        }
    }
    private void StartScreen()
    {
        normalText.text = "";
        titleText.text = "Upgrades";
        titleType.StartTyping();
        Invoke("MetalText", 1f);
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].SetActive(false);
        }
    }
    private void MetalText()
    {
        currencyType.StopTyping();
        normalType.StopTyping();
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
            insertWeapon.CanInsert(true);
            normalText.text = "";
            currencyText.text = currencyOnScreen.ToString("00000");
            currencyType.StartTyping();
            Invoke("InsertWeaponMessage", 2f);
            nut.SetActive(true);

        }     
    }
    private void AllOff()
    {
        currencyType.StopTyping();
        normalType.StopTyping();
        titleType.StopTyping();

        titleText.text = "";
        normalText.text = "";
        currencyText.text = "";

        nut.SetActive(false);

        for(int i = 0; i < gun.Length; i++)
        {
            gun[i].SetActive(false);
        }
        insertWeapon.Eject();
    }


    private void InsertWeaponMessage()
    {
        normalText.text = "Please insert weapon to upgrade";
        normalType.StartTyping();
    }
    public void WeaponIn(int weaponID)
    {
        CancelInvoke();
        normalType.StopTyping();
        normalText.text = "";
        gun[weaponID].SetActive(true);
        if(weaponID == 0)
        {
            titleText.text = "CZ50";
        }
        titleType.StartTyping();
    }

    public void WeaponOut()
    {
        insertWeapon.Eject();
        insertWeapon.CanInsert(false);
        CancelInvoke();
        StartScreen();
    }

}
