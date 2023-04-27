using TMPro;
using UnityEngine;

public class TV : MonoBehaviour
{
    [Header("Other references")]
    public TutorialManager tutorialManager;
    public Animator OpeningInFloor;

    [Header("Audio")]
    public AudioSource check;
    public AudioClip[] checkClip;

    [Header("Tutorial displays")]
    public TextMeshProUGUI niceText;
    public GameObject turningDisplay;
    public GameObject quickturnDisplay;
    public GameObject movementDisplay;

    [Header("Checklist")]
    public GameObject[] checkboxes;
    public TextMeshProUGUI[] objectiveText;
    public GameObject[] fill;


    private GameObject currentDisplay;
    private RequirementCheck requirementCheck;
    private TypeWriterText[] typeWriterText;
    private TypeWriterText niceTypeWriterText;

    private int numberOfRequirements;
    private bool quickturn, pausedChecked;
    private bool backToMenu;

    private void Start()
    {
        niceText.text = "";
        ResetTutorial();
        ResetChecklist();
        requirementCheck = GetComponent<RequirementCheck>();
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
        backToMenu = true;
        Objective(0, "Turn left");
        Objective(1, "Turn right");
        SetCurrentDisplay(turningDisplay);
        requirementCheck.CanTurn();
        if (LocomotionManager.instance.currentQuickTurnType == 0)
        {
            quickturn = true;
            requirementCheck.CanQuickturn();
            Objective(2, "Quickturn");
        }
    }

    public void Movement()
    {
        OpeningInFloor.SetTrigger("Lift");

        SetCurrentDisplay(movementDisplay);
        Objective(0, "Move to the pillar");
        Objective(1, "Press the button");
    }

    //Checkoff
    public void TurnLeftDone()
    {
        Fill(0);
        CheckOff();
    }
    public void TurnRightDone()
    {
        
        Fill(1);
        CheckOff();
    }
    public void QuickturnDone()
    {
        quickturn = false;
        Fill(2);
        CheckOff();
    }
    public void LiftTriggerEntered()
    {
        CheckOff();
        Fill(0);
    }



    // Effective methods

    private void CheckOff()
    {
        numberOfRequirements--;
        if(numberOfRequirements == 0)
        {
            ResetTutorial();

            niceText.text = "Well done";
            niceTypeWriterText.StartTyping();
            check.PlayOneShot(checkClip[1]);
            Invoke(nameof(ResetChecklist), 2);

            if (backToMenu)
            {
                Invoke(nameof(NextObjective), 2.5f);
                backToMenu = false;
            }
        }
        else if(numberOfRequirements == 1 && quickturn)
        {
            ResetTutorial();
            SetCurrentDisplay(quickturnDisplay);
            check.PlayOneShot(checkClip[0]);
        }
        else
        {
            check.PlayOneShot(checkClip[0]);
        }

    }

    private void Objective(int whichObjective, string objective)
    {
        checkboxes[whichObjective].SetActive(true);
        fill[whichObjective].SetActive(false);
        objectiveText[whichObjective].text = objective;
        typeWriterText[whichObjective].StartTyping();
        numberOfRequirements++;
    }

    private void Fill(int whichObjective)
    {
        fill[whichObjective].SetActive(true);
    }

    private void ResetTutorial()
    {
        currentDisplay = null;
        turningDisplay.SetActive(false);
        quickturnDisplay.SetActive(false);
        movementDisplay.SetActive(false);
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
    private void NextObjective()
    {
        tutorialManager.NextMenu();
    }
    private void SetCurrentDisplay(GameObject display)
    {
        currentDisplay = display;
        currentDisplay.SetActive(true);
    }
    public void EnableCurrentDisplay(bool state)
    {
        if(currentDisplay != null)
        {
            currentDisplay.SetActive(state);
        }
    }
}
