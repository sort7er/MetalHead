using TMPro;
using UnityEngine;

public class TV : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource check;
    public AudioClip[] checkClip;

    [Header("Tutorial displays")]
    public TextMeshProUGUI niceText;
    public TextMeshProUGUI infoText;
    public Transform tutorialParent;
    public bool pathLeft;

    public GameObject messageOverlay;
    public GameObject arrowDisplay;
    //public GameObject turningDisplay;
    //public GameObject quickturnDisplay;
    //public GameObject movementDisplay;
    //public GameObject menuDisplay;
    //public GameObject grabGunDisplay;
    //public GameObject shootGunDisplay;
    //public GameObject reloadGunDisplay;
    //public GameObject magnetDisplay;
    //public GameObject magnetMessageDisplay;

    [Header("Checklist")]
    public GameObject[] checkboxes;
    public TextMeshProUGUI[] objectiveText;
    public GameObject[] fill;


    private GameObject currentDisplay;
    private TypeWriterText[] typeWriterText;
    private TypeWriterText niceTypeWriterText;
    private TypeWriterText infoTypeWriterText;
    private DisplayStepByStep reloadGun;
    private DisplayStepByStep magnet;

    private bool pausedChecked, includeReloadAfterPause;

    private void Start()
    {
        //magnet = magnetDisplay.GetComponent<DisplayStepByStep>();
        //reloadGun = reloadGunDisplay.GetComponent<DisplayStepByStep>();
        niceTypeWriterText = niceText.GetComponent<TypeWriterText>();
        infoTypeWriterText = infoText.GetComponent<TypeWriterText>();
        niceText.text = "";
        infoText.text = "";
        typeWriterText = new TypeWriterText[objectiveText.Length];
        for(int i = 0; i < objectiveText.Length; i++)
        {
            typeWriterText[i] = objectiveText[i].GetComponent<TypeWriterText>();
        }
        ResetTutorial();
        ResetChecklist();

        if (pathLeft)
        {
            arrowDisplay.transform.localScale = new Vector3(-arrowDisplay.transform.localScale.x, arrowDisplay.transform.localScale.y, arrowDisplay.transform.localScale.z);
        }
        else
        {
            arrowDisplay.transform.localScale = new Vector3(arrowDisplay.transform.localScale.x, arrowDisplay.transform.localScale.y, arrowDisplay.transform.localScale.z);
        }
    }

    //private void Update()
    //{
    //    if(GameManager.instance.isPaused && !pausedChecked)
    //    {
    //        EnableCurrentDisplay(false);
    //        pausedChecked = true;
    //    }
    //    else if(!GameManager.instance.isPaused && pausedChecked)
    //    {
    //        EnableCurrentDisplay(true);
    //        pausedChecked = false;
    //    }
    //}

    //Enable
    public void Turning()
    {
        Objective(0, "Turn left");
        Objective(1, "Turn right");
        //SetCurrentDisplay(turningDisplay, true);
        if (LocomotionManager.instance.currentQuickTurnType == 0)
        {
            Objective(2, "Quickturn");
        }
    }

    public void Movement()
    {
        //SetCurrentDisplay(movementDisplay, true);
        Objective(0, "Move to the pillar");
        Objective(1, "Press the button");
    }

    public void Menu()
    {
        messageOverlay.SetActive(true);
        infoTypeWriterText.StopTyping();
        infoText.text = "You can change the comfort settings at any time by clicking on the menu button";
        infoTypeWriterText.StartTyping();
        //SetCurrentDisplay(menuDisplay, false);
    }

    public void Arrow(string textToPrint)
    {
        SetCurrentDisplay(arrowDisplay, false);
        Objective(0, textToPrint);
    }

    public void GrabObjectRight()
    {
        Objective(0, "Grab the cube with the right hand");
        Objective(1, "Release cube with the right hand");
    }
    public void GrabWatch()
    {
        Objective(0, "Grab the watch");
    }

    public void GrabGun()
    {
        ResetChecklist();
        Objective(0, "Grab gun");
        //SetCurrentDisplay(grabGunDisplay, false);
    }
    public void ShootGun()
    {
        ResetChecklist();
        Objective(0, "Fire all bullets in gun");
        //SetCurrentDisplay(shootGunDisplay, false);
    }
    public void ReloadGun()
    {
        ResetChecklist();
        Objective(0, "Drop mag");
        Objective(1, "Grab mag");
        Objective(2, "Insert mag");
        Objective(3, "Pull slide");
        //SetCurrentDisplay(reloadGunDisplay, false);
        //NextReload();
    }
    //public void NextReload()
    //{
    //    reloadGun.Display();
    //}
    public void Shotgun()
    {
        ResetChecklist();
        Objective(0, "Grab shotgun from your shoulder");
        Objective(1, "Reload shotgun");
    }
    public void KillEnemy()
    {
        ResetChecklist();
        Objective(0, "Kill Enemy");
    }
    public void Magnet()
    {
        ResetChecklist();
        Objective(0, "Grab magnet from your shoulder");
        Objective(1, "Pick up metals with magnet");
        //SetCurrentDisplay(magnetDisplay, false);
        //NextMagnet();
    }
    //public void NextMagnet()
    //{
    //    magnet.Display();
    //}
    public void MagnetMessage()
    {
        messageOverlay.SetActive(true);
        infoTypeWriterText.StopTyping();
        infoText.text = "The amount displayed shows how much metal you can spend on upgrades at an upgradestation";
        infoTypeWriterText.StartTyping();
    }

    // Methods called from relay
    public void AllChecked(float delay)
    {
        ResetTutorial();
        messageOverlay.SetActive(true);
        niceTypeWriterText.StopTyping();
        niceText.text = "Well done";
        niceTypeWriterText.StartTyping();
        Check(1);
        Invoke(nameof(ResetChecklist), delay);
    }
    //public void SetQuickTurnDisplay()
    //{
    //    SetCurrentDisplay(quickturnDisplay, false);
    //}

    public void Check(int i)
    {
        check.PlayOneShot(checkClip[i]);
    }

    public void Fill(int whichObjective)
    {
        fill[whichObjective].SetActive(true);
    }


    //Methods called from script
    private void Objective(int whichObjective, string objective)
    {
        checkboxes[whichObjective].SetActive(true);
        fill[whichObjective].SetActive(false);
        typeWriterText[whichObjective].StopTyping();
        objectiveText[whichObjective].text = objective;
        typeWriterText[whichObjective].StartTyping();
    }
    private void ResetChecklist()
    {
        niceTypeWriterText.StopTyping();
        niceText.text = "";
        infoTypeWriterText.StopTyping();
        infoText.text = "";
        messageOverlay.SetActive(false);
        for (int i = 0; i < objectiveText.Length; i++)
        {
            typeWriterText[i].StopTyping();
            objectiveText[i].text = "";
            checkboxes[i].SetActive(false);
            fill[i].SetActive(false);
        }
    }
    private void ResetTutorial()
    {
        currentDisplay = null;
        foreach (Transform t in tutorialParent)
        {
            t.gameObject.SetActive(false);
        }

    }
    private void SetCurrentDisplay(GameObject display, bool reload)
    {
        ResetTutorial();
        ResetChecklist();
        includeReloadAfterPause = reload;
        currentDisplay = display;
        currentDisplay.SetActive(true);
    }
    private void EnableCurrentDisplay(bool state)
    {
        if(currentDisplay != null && includeReloadAfterPause)
        {
            currentDisplay.SetActive(state);
        }
    }
}
