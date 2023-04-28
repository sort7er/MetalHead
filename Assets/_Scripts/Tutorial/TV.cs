using TMPro;
using UnityEngine;

public class TV : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource check;
    public AudioClip[] checkClip;

    [Header("Tutorial displays")]
    public TextMeshProUGUI niceText;
    public GameObject turningDisplay;
    public GameObject quickturnDisplay;
    public GameObject movementDisplay;
    public GameObject menuDisplay;
    public GameObject arrowDisplay;
    public bool pathLeft;

    [Header("Checklist")]
    public GameObject[] checkboxes;
    public TextMeshProUGUI[] objectiveText;
    public GameObject[] fill;


    private GameObject currentDisplay;
    private TypeWriterText[] typeWriterText;
    private TypeWriterText niceTypeWriterText;

    private bool pausedChecked, includeReloadAfterPause;

    private void Start()
    {
        niceText.text = "";
        ResetTutorial();
        ResetChecklist();
        niceTypeWriterText = niceText.GetComponent<TypeWriterText>();
        typeWriterText = new TypeWriterText[objectiveText.Length];
        for(int i = 0; i < objectiveText.Length; i++)
        {
            typeWriterText[i] = objectiveText[i].GetComponent<TypeWriterText>();
        }
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

    public void Arrow()
    {
        SetCurrentDisplay(arrowDisplay, false);
        if (pathLeft)
        {
            arrowDisplay.transform.localScale = new Vector3(-arrowDisplay.transform.localScale.x, arrowDisplay.transform.localScale.y, arrowDisplay.transform.localScale.z);
        }
        Objective(0, "Proceed out of the room");
    }

    // Methods called from relay
    public void AllChecked()
    {
        ResetTutorial();
        niceText.text = "Well done";
        niceTypeWriterText.StartTyping();
        Check(1);
        Invoke(nameof(ResetChecklist), 2);
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
        objectiveText[whichObjective].text = objective;
        typeWriterText[whichObjective].StartTyping();
    }
    private void ResetChecklist()
    {
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
        turningDisplay.SetActive(false);
        quickturnDisplay.SetActive(false);
        movementDisplay.SetActive(false);
        menuDisplay.SetActive(false);
        arrowDisplay.SetActive(false);
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
