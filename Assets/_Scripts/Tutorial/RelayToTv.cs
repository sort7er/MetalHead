using UnityEngine;

public class RelayToTv : MonoBehaviour
{
    public Animator OpeningInFloor;
    public Animator doorToOpen;
    public GameObject ground2;
    public TV[] tvsInScene;

    private TutorialManager tutorialManager;
    private RequirementCheck requirementCheck;
    private bool quickturn;
    private int numberOfRequirements, currentObjective;

    private void Start()
    {
        ground2.SetActive(false);
        currentObjective = 0;
        tutorialManager = GetComponent<TutorialManager>();
        requirementCheck = GetComponent<RequirementCheck>();
    }



    //Enable
    public void TvTurning()
    {
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
    public void TvMenu()
    {
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Menu();
        }
        Invoke(nameof(TvArrow), 6f);
    }
    public void TvArrow()
    {
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow();
        }
        ground2.SetActive(true);
        doorToOpen.SetTrigger("Open");
    }
    public void TvGrabGun()
    {
        AddRequirement(2);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].GrabGun();
        }
    }
    public void TvReloadGun()
    {
        AddRequirement(4);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].ReloadGun();
        }
    }
    public void NextReload()
    {
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].NextReload();
        }
    }

    //Checkoff
    public void CheckASpot(int objectiveToFill)
    {
        CheckOff();
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Fill(objectiveToFill);
        }
    }
    public void QuickturnDone()
    {
        CheckASpot(2);
        quickturn = false;
    }

    private void CheckOff()
    {
        numberOfRequirements--;
        if (numberOfRequirements == 0)
        {
            currentObjective++;

            for (int i = 0; i < tvsInScene.Length; i++)
            {
                tvsInScene[i].AllChecked();
            }

            if(currentObjective == 1)
            {
                Invoke(nameof(NextMenu), 2.5f);
            }
            else if(currentObjective == 2)
            {
                Invoke(nameof(TvMenu), 2.5f);
            }
            else if (currentObjective == 3)
            {
                Debug.Log("Nice");
                //Invoke(nameof(TvMenu), 2.5f);
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
    public void AddRequirement(int add)
    {
        numberOfRequirements += add;
    }
    private void NextMenu()
    {
        tutorialManager.NextMenu();
    }

}
