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
    float delay;

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
        delay = 2;

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
        OpeningInFloor.SetBool("Lift", true);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Movement();
        }
    }
    public void TvMenu()
    {
        delay = 0f;
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Menu();
        }
        Invoke(nameof(TvArrow), 4f);
    }
    public void TvArrow()
    {
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow();
        }
        ground2.SetActive(true);
        doorToOpen.SetTrigger("Open");
    }
    public void TvGrabGun()
    {
        AddRequirement(1);
        requirementCheck.CanGrabGun();
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].GrabGun();
        }
    }
    public void TvShootGun()
    {
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].ShootGun();
        }
    }
    public void TvReloadGun()
    {
        delay = 2f;
        AddRequirement(4);
        requirementCheck.CanReload();
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
    public void TvArrow2()
    {
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow();
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
                tvsInScene[i].AllChecked(delay);
            }

            if (currentObjective == 1)
            {
                Invoke(nameof(NextMenu), delay);
            }
            else if(currentObjective == 2)
            {
                Invoke(nameof(TvMenu), delay);
            }
            else if (currentObjective == 3)
            {
                Invoke(nameof(TvGrabGun), delay);
            }
            else if (currentObjective == 4)
            {
                Invoke(nameof(TvShootGun), delay);
            }
            else if (currentObjective == 5)
            {
                Invoke(nameof(TvReloadGun), delay);
            }
            else if (currentObjective == 6)
            {
                Invoke(nameof(TvArrow2), delay);
            }



        }
        else if (numberOfRequirements == 1 && quickturn)
        {
            for (int i = 0; i < tvsInScene.Length; i++)
            {
                tvsInScene[i].SetQuickTurnDisplay();
            }
        }
        
        if(numberOfRequirements > 0) 
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
