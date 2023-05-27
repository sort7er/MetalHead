using UnityEngine;

public class RelayToTv : MonoBehaviour
{
    public Animator OpeningInFloor;
    public Animator[] doorToOpen;
    public GameObject[] groundToEnable;
    public TV[] tvsInScene;

    private TutorialManager tutorialManager;
    private RequirementCheck requirementCheck;
    private bool quickturn;
    private int numberOfRequirements, currentObjective;
    private float delay;

    private void Start()
    {
        for(int i = 0; i < groundToEnable.Length; i++)
        {
            groundToEnable[i].SetActive(false);
        }
        currentObjective = 0;
        tutorialManager = GetComponent<TutorialManager>();
        requirementCheck = GetComponent<RequirementCheck>();
    }



    //Enable
    public void TvTurning()
    {
        tutorialManager.RightQuestActive(true);
        tutorialManager.rightQuest.Joystick(2);
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
        tutorialManager.LeftQuestActive(true);
        tutorialManager.leftQuest.Joystick(0);
        OpeningInFloor.SetBool("Lift", true);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Movement();
        }
    }
    public void TvMenu()
    {
        delay = 0f;
        requirementCheck.CanPressMenu(true);
        tutorialManager.LeftQuestActive(true);
        tutorialManager.leftQuest.Menu();
        GameManager.instance.leftHand.SetHandActive(false);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Menu();
        }
        Invoke(nameof(TvArrow), 7f);
    }
    public void TvArrow()
    {
        requirementCheck.CanPressMenu(false);
        tutorialManager.leftQuest.Nothing();
        tutorialManager.LeftQuestActive(false);
        GameManager.instance.leftHand.SetHandActive(true);
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow("Proceed out of the room");
        }
        groundToEnable[0].SetActive(true);
        doorToOpen[0].SetTrigger("Open");
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
        delay = 0;
        groundToEnable[1].SetActive(true);
        doorToOpen[1].SetTrigger("Open");
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow("Proceed out of the room, or keep practicing");
        }
    }
    public void TvMagnet()
    {
        delay = 2f;
        tutorialManager.SetMagnet(true);
        AddRequirement(2);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Magnet();
        }
    }
    public void NextMagnet()
    {
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].NextMagnet();
        }
    }
    public void TVMagnetMessage()
    {
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].MagnetMessage();
        }
        Invoke(nameof(TvArrow3), 7f);
    }
    public void TvArrow3()
    {
        groundToEnable[2].SetActive(true);
        doorToOpen[2].SetTrigger("Open");
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow("Proceed out of the room");
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
        tutorialManager.rightQuest.Nothing();
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
                tutorialManager.rightQuest.Nothing();
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
            else if (currentObjective == 7)
            {
                Invoke(nameof(TvMagnet), delay);
            }
            else if (currentObjective == 8)
            {
                Invoke(nameof(TVMagnetMessage), delay);
            }



        }
        else if (numberOfRequirements == 1 && quickturn)
        {
            for (int i = 0; i < tvsInScene.Length; i++)
            {
                tvsInScene[i].SetQuickTurnDisplay();
                tutorialManager.rightQuest.Joystick(1);
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
