using TMPro;
using UnityEngine;

public class TV : MonoBehaviour
{
    public TutorialManager tutorialManager;

    [Header("Audio")]
    public AudioSource check;
    public AudioClip[] checkClip;

    [Header("Tutorial displays")]
    public TextMeshProUGUI niceText;
    public GameObject turningDisplay;
    public GameObject quickturnDisplay;

    [Header("Checklist")]
    public GameObject[] checkboxes;
    public TextMeshProUGUI[] objectiveText;
    public GameObject[] fill;


    private RequirementCheck requirementCheck;
    private TypeWriterText[] typeWriterText;
    private TypeWriterText niceTypeWriterText;

    private int numberOfRequirements;
    private bool quickturn;

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

    //Enable
    public void Turning()
    {
        Objective(0, "Turn left");
        Objective(1, "Turn right");
        turningDisplay.SetActive(true);
        requirementCheck.CanTurn();
        if (LocomotionManager.instance.currentQuickTurnType == 0)
        {
            quickturn = true;
            requirementCheck.CanQuickturn();
            Objective(2, "Quickturn");
        }
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
            Invoke(nameof(NextObjective), 2.5f);
        }
        else if(numberOfRequirements == 1 && quickturn)
        {
            ResetTutorial();
            quickturnDisplay.SetActive(true);
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
        turningDisplay.SetActive(false);
        quickturnDisplay.SetActive(false);
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
}
