using UnityEngine;
using TMPro;

public class UpgradeStation : MonoBehaviour
{
    public Color errorColor;
    public float fanSoundSmoothTime;
    public AudioSource buttonSource;
    public AudioClip screenOn, screenOff;
    public Animator screenAnim;
    public TextMeshProUGUI titleText, normalText, currencyText, minusText;
    public GameObject nut;
    public GameObject[] gun;
    public CZ50Upgrades cz50Upgrades;

    [HideInInspector] public int currencyOnScreen, minusOnScreen;

    private InsertWeapon insertWeapon;
    private AudioSource upgradeStationSource;
    private TypeWriterText titleType, normalType, currencyType, minusType;
    private float currentVolume, targetVolume;
    private int activeUpgrade;
    private bool isOn;

    private void Start()
    {
        insertWeapon = GetComponentInChildren<InsertWeapon>();
        upgradeStationSource = GetComponent<AudioSource>();
        titleType = titleText.GetComponent<TypeWriterText>();
        normalType = normalText.GetComponent<TypeWriterText>();
        currencyType = currencyText.GetComponent<TypeWriterText>();
        minusType = minusText.GetComponent<TypeWriterText>();
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
        minusOnScreen = 0;
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
    private void StartScreen()
    {
        currencyType.StopTyping();
        normalType.StopTyping();
        titleType.StopTyping();
        minusType.StopTyping();

        minusText.text = "";
        normalText.text = "";
        currencyText.text = currencyOnScreen.ToString("#,#");
        currencyType.StartTyping();
        
        titleText.text = "Upgrades";
        titleType.StartTyping();
        nut.SetActive(true);
        Invoke("InsertWeaponMessage", 1f);
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].SetActive(false);
        }
    }
    private void AllOff()
    {
        currencyType.StopTyping();
        normalType.StopTyping();
        titleType.StopTyping();
        minusType.StopTyping();

        minusText.text = "";
        titleText.text = "";
        normalText.text = "";
        currencyText.text = "";

        nut.SetActive(false);

        for(int i = 0; i < gun.Length; i++)
        {
            gun[i].SetActive(false);
        }
    }


    private void InsertWeaponMessage()
    {
        normalText.text = "Please insert weapon to upgrade";
        normalType.StartTyping();

        insertWeapon.InsertWeaponAnim(true);
        
    }
    public void WeaponIn(int weaponID)
    {
        if(weaponID != 0)
        {
            activeUpgrade = weaponID;
            CancelInvoke();
            normalType.StopTyping();
            titleType.StopTyping();
            normalText.text = "";
            gun[activeUpgrade - 1].SetActive(true);
            if (activeUpgrade == 1)
            {
                titleText.text = "CZ50";
            }
            titleType.StartTyping();
        }
        else
        {
            Debug.Log("NoWeaponToDrop");
        }

    }

    public void WeaponOut()
    {
        CancelInvoke();
        StartScreen();
    }

    public void AddingPurchase(int cost)
    {
        minusType.StopTyping();
        currencyOnScreen -= cost;
        minusOnScreen += cost;
        minusText.text = " - " + minusOnScreen.ToString("#,#");
        minusType.StartTyping();
    }
    public void RemovePurchase(int cost)
    {
        minusType.StopTyping();
        currencyOnScreen += cost;
        minusOnScreen -= cost;
        if(minusOnScreen <= 0)
        {
            minusText.text = "";
        }
        else
        {
            minusText.text = " - " + minusOnScreen.ToString("#,#");
            minusType.StartTyping();
        }
    }

    public void Up()
    {
        if(activeUpgrade == 1)
        {
            cz50Upgrades.Up();
        }
    }
    public void Down()
    {
        if (activeUpgrade == 1)
        {
            cz50Upgrades.Down();
        }
    }
    public void Add()
    {
        if (activeUpgrade == 1)
        {
            cz50Upgrades.Add();
        }
    }
    public void Remove()
    {
        if (activeUpgrade == 1)
        {
            cz50Upgrades.Remove();
        }
    }
    public void Execute()
    {
        if (activeUpgrade == 1)
        {
            cz50Upgrades.Execute();
        }
        minusType.StopTyping();
        minusOnScreen = 0;
        minusText.text = "";
        if(currencyOnScreen == 0)
        {
            currencyText.text = "0";
        }
        else
        {
            currencyText.text = currencyOnScreen.ToString("#,#");
        }

        currencyType.StartTyping();
    }
    public void NotEnough()
    {
        minusText.color = errorColor;
        currencyText.color = errorColor;
        Invoke("NotEnoughDone", 0.1f);
    }
    private void NotEnoughDone()
    {
        minusText.color = Color.black;
        currencyText.color = Color.black;
    }
}
