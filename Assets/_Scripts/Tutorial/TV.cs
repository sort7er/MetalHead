using TMPro;
using UnityEngine;

public class TV : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource check;
    public AudioClip[] checkClip;

    [Header("Tutorial displays")]
    public TextMeshProUGUI niceText;
    public Transform tutorialParent;

    public GameObject turningDisplay;
    public GameObject quickturnDisplay;
    public GameObject movementDisplay;
    public GameObject menuDisplay;
    public GameObject arrowDisplay;
    public bool pathLeft;
    public GameObject grabGunDisplay;
    public GameObject shootGunDisplay;
    public GameObject reloadGunDisplay;
    public GameObject magnetDisplay;
    public GameObject magnetMessageDisplay;

    [Header("Checklist")]
    public GameObject[] checkboxes;
    public TextMeshProUGUI[] objectiveText;
    public GameObject[] fill;


    private GameObject currentDisplay;
    private TypeWriterText[] typeWriterText;
    private TypeWriterText niceTypeWriterText;
    private DisplayStepByStep reloadGun;
    private DisplayStepByStep magnet;

    private bool pausedChecked, includeReloadAfterPause;

    private void Start()
    {
        magnet = magnetDisplay.GetComponent<DisplayStepByStep>();
        reloadGun = reloadGunDisplay.GetComponent<DisplayStepByStep>();
        niceTypeWriterText = niceText.GetComponent<TypeWriterText>();
        niceText.text = "";
        typeWriterText = new TypeWriterText[objectiveText.Length];
        for(int i = 0; i < objectiveText.Length; i++)
        {
            typeWriterText[i] = objectiveText[i].GetComponent<TypeWriterText>();
        }
        ResetTutorial();
        ResetChecklist();
    }

    private void Update()
    {
        if(GameManager.instance.isPaused && !pausedChecked)
        {
            EnableCurrentDisplay(false);
            pausedChecked = true;
        }
        else if(!GameManager.instance.isPaused && pausedChecked)
        {
            EnableCurrentDisplay(true);
            pausedChecked = false;
        }
    }

    //Enable
    public void Turning()
    {
        Objective(0, "Turn left");
        Objective(1, "Turn right");
        SetCurrentDisplay(turningDisplay, true);
        if (LocomotionManager.instance.currentQuickTurnType == 0)
        {
            Objective(2, "Quickturn");
        }
    }

    public void Movement()
    {
        SetCurrentDisplay(movementDisplay, true);
        Objective(0, "Move to the pillar");
        Objective(1, "Press the button");
    }

    public void Menu()
    {
        SetCurrentDisplay(menuDisplay, false);
    }

    public void Arrow(string textToPrint)
    {
        SetCurrentDisplay(arrowDisplay, false);
        if (pathLeft)
        {
            arrowDisplay.transform.localScale = new Vector3(-arrowDisplay.transform.localScale.x, arrowDisplay.transform.localScale.y, arrowDisplay.transform.localScale.z);
        }
        Objective(0,textToPrint);
    }

    public void GrabGun()
    {
        ResetChecklist();
        Objective(0, "Grab gun");
        SetCurrentDisplay(grabGunDisplay, false);
    }
    public void ShootGun()
    {
        ResetChecklist();
        Objective(0, "Fire all bullets in gun");
        SetCurrentDisplay(shootGunDisplay, false);
    }
    public void ReloadGun()
    {
        ResetChecklist();
        Objective(0, "Drop mag");
        Objective(1, "Grab mag");
        Objective(2, "Insert mag");
        Objective(3, "Pull slide");
        SetCurrentDisplay(reloadGunDisplay, false);
        NextReload();
    }
    public void NextReload()
    {
        reloadGun.Display();
    }
    public void Magnet()
    {
        ResetChecklist();
        Objective(0, "Grab magnet from your shoulder");
        Objective(1, "Pick up metals with magnet");
        SetCurrentDisplay(magnetDisplay, false);
        NextMagnet();
    }
    public void NextMagnet()
    {
        magnet.Display();
    }
    public void MagnetMessage()
    {
        ResetChecklist();
        SetCurrentDisplay(magnetMessageDisplay, false);
    }

    // Methods called from relay
    public void AllChecked(float delay)
    {
        ResetTutorial();
        niceTypeWriterText.StopTyping();
        niceText.text = "Well done";
        niceTypeWriterText.StartTyping();
        Check(1);
        Invoke(nameof(ResetChecklist), delay);
    }
    public void SetQuickTurnDisplay()
    {
        SetCurrentDisplay(quickturnDisplay, false);
    }

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
        for (int i = 0; i < objectiveText.Length; i++)
        {
            objectiveText[i].text = "";
            checkboxes[i].SetActive(false);
            fill[i].SetActive(false);
        }
    }
    private void ResetTutorial()
    {
        currentDisplay = null;
        foreach(Transform t in tutorialParent)
        {
            t.gameObject.SetActive(false);
        }

    }
    private void SetCurrentDisplay(GameObject display, bool reload)
    {
        ResetTutorial();
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
