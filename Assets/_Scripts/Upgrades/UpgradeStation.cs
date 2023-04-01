using UnityEngine;
using TMPro;

public class UpgradeStation : MonoBehaviour
{
    public Color errorColor;
    public float fanSoundSmoothTime;
    public AudioSource buttonSource, screenSource;
    public AudioClip screenOn, screenOff, error, select, select2, upgrading;
    public Animator screenAnim;
    public TextMeshProUGUI titleText, normalText, currencyText, minusText;
    public GameObject nut, backButton, executeButton, loadingBar;
    public GameObject[] gun;
    public CZ50Upgrades cz50Upgrades;

    [HideInInspector] public int currencyOnScreen, minusOnScreen;
    [HideInInspector] public bool isOn;

    private Animator loadingBarAnim;
    private InsertWeapon insertWeapon;
    private AudioSource upgradeStationSource;
    private TypeWriterText titleType, normalType, currencyType, minusType;
    private float currentVolume, targetVolume;
    private int activeUpgrade;
    private bool notEnough;


    private void Start()
    {
        loadingBarAnim = loadingBar.GetComponent<Animator>();
        insertWeapon = GetComponentInChildren<InsertWeapon>();
        upgradeStationSource = GetComponent<AudioSource>();
        titleType = titleText.GetComponent<TypeWriterText>();
        normalType = normalText.GetComponent<TypeWriterText>();
        currencyType = currencyText.GetComponent<TypeWriterText>();
        minusType = minusText.GetComponent<TypeWriterText>();
        nut.SetActive(false);
        backButton.SetActive(false);
        executeButton.SetActive(false);
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
        currencyType.StopTyping();
        if (currencyOnScreen == 0)
        {
            currencyText.text = "0";
        }
        else
        {
            currencyText.text = currencyOnScreen.ToString("#,#");
        }
        currencyType.StartTyping();
        minusType.StopTyping();
        minusText.text = "";
        if (insertWeapon.inserted)
        {
            insertWeapon.PowerOn();
        }
        else
        {
            StartScreen();
        }
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
    public void StartScreen()
    {
        normalType.StopTyping();
        titleType.StopTyping();
        normalText.text = "";
        
        titleText.text = "Upgrades";
        titleType.StartTyping();
        nut.SetActive(true);
        backButton.SetActive(false);
        executeButton.SetActive(false);
        loadingBar.SetActive(false);
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

        insertWeapon.PowerOff();
        nut.SetActive(false);

        backButton.SetActive(false);
        executeButton.SetActive(false);
        loadingBar.SetActive(false);

        for (int i = 0; i < gun.Length; i++)
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
            backButton.SetActive(true);
            gun[activeUpgrade - 1].SetActive(true);
            if (activeUpgrade == 1)
            {
                titleText.text = "CZ50";
            }
            titleType.StartTyping();
            GameManager.instance.IsUpgrading(true);
        }
        else
        {
            Debug.Log("NoWeaponToDrop");
        }

    }

    public void AddingPurchase(int cost)
    {
        minusType.StopTyping();
        currencyOnScreen -= cost;
        minusOnScreen += cost;
        minusText.text = " - " + minusOnScreen.ToString("#,#");
        minusType.StartTyping();
        if (!executeButton.activeSelf)
        {
            executeButton.SetActive(true);
        }
    }
    public void RemovePurchase(int cost)
    {
        minusType.StopTyping();
        currencyOnScreen += cost;
        minusOnScreen -= cost;
        if(minusOnScreen <= 0)
        {
            minusText.text = "";
            executeButton.SetActive(false);
        }
        else
        {
            minusText.text = " - " + minusOnScreen.ToString("#,#");
            minusType.StartTyping();
        }
    }
    public void Execute(bool withUpgrades)
    {
        minusType.StopTyping();
        minusOnScreen = 0;
        minusText.text = "";
        currencyText.text = "";
        gun[activeUpgrade - 1].SetActive(false);
        nut.SetActive(false);
        backButton.SetActive(false);
        executeButton.SetActive(false);
        if (withUpgrades)
        {
            if (activeUpgrade == 1)
            {
                cz50Upgrades.Execute();
                Upgrading();
            }
        }
        else
        {
            if (activeUpgrade == 1)
            {
                cz50Upgrades.Abort();
            }
            UpgradeDone();
        }

    }
    private void Upgrading()
    {
        normalText.text = "Loading";
        normalType.StartTyping();
        loadingBar.SetActive(true);
        loadingBarAnim.SetTrigger("Loading");
        screenSource.PlayOneShot(upgrading);
        Invoke("UpgradeDone", 4.5f);
    }
    private void UpgradeDone()
    {
        normalType.StopTyping();
        normalText.text = "";
        loadingBar.SetActive(false);
        currencyOnScreen = GameManager.instance.magnet.GetMetalsCollected();
        if (currencyOnScreen == 0)
        {
            currencyText.text = "0";
        }
        else
        {
            currencyText.text = currencyOnScreen.ToString("#,#");
        }
        currencyType.StartTyping();
        StartScreen();
        insertWeapon.InsertWeaponAnim(true);
        GameManager.instance.IsUpgrading(false);
    }

    public void NotEnough()
    {
        notEnough = true;
        screenSource.PlayOneShot(error);
        minusText.color = errorColor;
        currencyText.color = errorColor;
        Invoke("NotEnoughDone", 0.1f);
    }
    private void NotEnoughDone()
    {
        notEnough = false;
        minusText.color = Color.black;
        currencyText.color = Color.black;
    }
    public void SelectSound(int type)
    {
        if (!notEnough)
        {
            if(type == 0)
            {
                screenSource.PlayOneShot(select);
            }
            else
            {
                screenSource.PlayOneShot(select2);
            }
            
        }
    }
}
