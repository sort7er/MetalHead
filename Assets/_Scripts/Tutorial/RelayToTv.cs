using UnityEngine;

public class RelayToTv : MonoBehaviour
{
    public Animator OpeningInFloor;
    public TV[] tvsInScene;

    private TutorialManager tutorialManager;
    private RequirementCheck requirementCheck;
    private bool backToMenu, quickturn;
    private int numberOfRequirements;

    private void Start()
    {
        tutorialManager = GetComponent<TutorialManager>();
        requirementCheck = GetComponent<RequirementCheck>();
    }



    //Enable
    public void TvTurning()
    {
        backToMenu = true;
        requirementCheck.CanTurn();
        AddRequirement(2);

        if (LocomotionManager.instance.currentQuickTurnType == 0)
        {
            AddRequirement(1);
            quickturn = true;
            requirementCheck.CanQuickturn();
        }

        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Turning();
        }
    }
    public void TvMovement()
    {
        AddRequirement(2);
        OpeningInFloor.SetTrigger("Lift");
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Movement();
        }
    }


    //Checkoff
    public void TurnLeftDone()
    {
        CheckOff();
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Fill(0);
        }
    }
    public void TurnRightDone()
    {
        CheckOff();
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Fill(1);
        }
    }
    public void QuickturnDone()
    {
        CheckOff();
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Fill(2);
        }
        quickturn = false;
    }
    public void LiftTriggerEntered()
    {
        CheckOff();
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Fill(0);
        }
    }

    private void CheckOff()
    {
        numberOfRequirements--;
        if (numberOfRequirements == 0)
        {
            for (int i = 0; i < tvsInScene.Length; i++)
            {
                tvsInScene[i].AllChecked();
            }

            if (backToMenu)
            {
                Invoke(nameof(NextObjective), 2.5f);
                backToMenu = false;
            }
        }
        else if (numberOfRequirements == 1 && quickturn)
        {
            for (int i = 0; i < tvsInScene.Length; i++)
            {
                tvsInScene[i].SetQuickTurnDisplay();
            }
        }
        
        if(numberOfRequirements < 0) 
        {
            for (int i = 0; i < tvsInScene.Length; i++)
            {
                tvsInScene[i].Check(0);
            }
        }

    }
    private void NextObjective()
    {
        tutorialManager.NextMenu();
    }
    public void AddRequirement(int add)
    {
        numberOfRequirements += add;
    }


}
