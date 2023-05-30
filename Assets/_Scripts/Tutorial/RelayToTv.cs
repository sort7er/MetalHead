using UnityEngine;

public class RelayToTv : MonoBehaviour
{
    public Animator OpeningInFloor;
    public Animator[] doorToOpen;
    public GameObject[] groundToEnable;
    public TV[] tvsInScene;
    public Transform objectToGrab;
    public Transform button;
    public EnemyHealth enemy;
    public Transform enemyHead;
    
    [HideInInspector] public float delay;


    private QuestController leftQuest, rightQuest;
    private TutorialManager tutorialManager;
    private RequirementCheck requirementCheck;
    private bool quickturn;
    private int numberOfRequirements, currentObjective;

    private void Start()
    {
        leftQuest = GameManager.instance.leftHand.questController;
        rightQuest = GameManager.instance.rightHand.questController;
        for (int i = 0; i < groundToEnable.Length; i++)
        {
            groundToEnable[i].SetActive(false);
        }
        currentObjective = 0;
        tutorialManager = GetComponent<TutorialManager>();
        requirementCheck = GetComponent<RequirementCheck>();
        enemy.gameObject.SetActive(false);
    }



    //Enable
    public void TvTurning()
    {
        rightQuest.QuestActive(true);
        rightQuest.Joystick(2, true);
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
        Guide.instance.SetGuide(3, button, "Press button", false);
        AddRequirement(2);
        leftQuest.QuestActive(true);
        rightQuest.QuestActive(true);
        leftQuest.Joystick(0, true);
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
        leftQuest.QuestActive(true);
        leftQuest.Menu(true);
        Guide.instance.SetGuide(2, leftQuest.inputs[3].transform , "You can change the comfort settings at any time by clicking on the menu button", false);
        GameManager.instance.leftHand.SetHandActive(false);
        //for (int i = 0; i < tvsInScene.Length; i++)
        //{
        //    tvsInScene[i].Menu();
        //}
        Invoke(nameof(TvArrow), 7f);
    }
    public void TvArrow()
    {
        Guide.instance.GuideDone();
        CancelInvoke(nameof(TvArrow));
        requirementCheck.CanPressMenu(false);
        leftQuest.QuestActive(false);
        GameManager.instance.leftHand.SetHandActive(true);
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow("Proceed out of the room");
        }
        groundToEnable[0].SetActive(true);
        doorToOpen[0].SetTrigger("Open");
    }
    public void TvGrabRight()
    {
        Guide.instance.SetGuide(3, objectToGrab, "Grab the cube", false);
        delay = 0;
        AddRequirement(2);

        leftQuest.QuestActive(false);
        GameManager.instance.leftHand.SetHandActive(false);
        GameManager.instance.EnableLeftInteractor(false);

        rightQuest.QuestActive(true);
        rightQuest.Grip(true);
        GameManager.instance.rightHand.SetHandActive(true);



        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].GrabObjectRight();
        }
    }
    public void TvWatch()
    {
        delay = 4;
        AddRequirement(1);

        GameManager.instance.EnableLeftInteractor(true);
        leftQuest.QuestActive(true);
        leftQuest.Grip(true);
        GameManager.instance.leftHand.SetHandActive(true);

        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].GrabWatch();
        }
    }

    public void TvArrow05()
    {
        delay = 0;
        groundToEnable[1].SetActive(true);
        doorToOpen[1].SetTrigger("Open");
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow("Proceed out of the room");
        }
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
    //public void NextReload()
    //{
    //    for (int i = 0; i < tvsInScene.Length; i++)
    //    {
    //        tvsInScene[i].NextReload();
    //    }
    //}
    public void TvArrow2()
    {
        delay = 0;
        groundToEnable[2].SetActive(true);
        doorToOpen[2].SetTrigger("Open");
        AddRequirement(1);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Arrow("Proceed out of the room, or keep practicing");
        }
    }
    public void TvShotgun()
    {
        delay = 0f;
        tutorialManager.SetShotgun(true);
        Guide.instance.SetGuide(2, tutorialManager.shotgun.transform, "Grab shotgun from over your sholder", true);
        AddRequirement(2);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Shotgun();
        }
    }
    public void TVKillEnemy()
    {
        doorToOpen[3].SetTrigger("Open");
        enemy.gameObject.SetActive(true);
        Invoke(nameof(TVKillEnemyDelay), 1.5f);
        
    }
    private void TVKillEnemyDelay()
    {
        AddRequirement(1);
        Guide.instance.SetGuide(3, enemyHead, "Kill", false);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].KillEnemy();
        }
        GameManager.instance.SetTimeScaleSmooth(0.2f);
    }
    public void TvMagnet()
    {
        delay = 2f;
        tutorialManager.SetMagnet(true);
        AddRequirement(2);
        Guide.instance.SetGuide(2, tutorialManager.magnet.transform, "Grab magnet from over your sholder", true);
        for (int i = 0; i < tvsInScene.Length; i++)
        {
            tvsInScene[i].Magnet();
        }
    }
    public void NextMagnet()
    {
        //for (int i = 0; i < tvsInScene.Length; i++)
        //{
        //    tvsInScene[i].NextMagnet();
        //}
    }
    public void TVMagnetMessage()
    {
        Guide.instance.SetGuide(2, tutorialManager.magnet.transform, "The amount displayed shows how much you can spend on upgrades at an upgradestation", false);
        //for (int i = 0; i < tvsInScene.Length; i++)
        //{
        //    tvsInScene[i].MagnetMessage();
        //}
        Invoke(nameof(TvArrow3), 7f);
    }
    public void TvArrow3()
    {
        Guide.instance.GuideDone();
        groundToEnable[3].SetActive(true);
        doorToOpen[4].SetTrigger("Open");
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
                rightQuest.Nothing();
                Invoke(nameof(NextMenu), delay);
            }
            else if(currentObjective == 2)
            {
                Invoke(nameof(TvMenu), delay);
            }
            else if (currentObjective == 3)
            {
                Invoke(nameof(TvGrabRight), delay);
            }
            else if (currentObjective == 4)
            {
                Invoke(nameof(TvWatch), delay);
            }
            else if (currentObjective == 5)
            {
                Invoke(nameof(TvArrow05), delay);
            }
            else if (currentObjective == 6)
            {
                Invoke(nameof(TvGrabGun), delay);
            }
            else if (currentObjective == 7)
            {
                Invoke(nameof(TvShootGun), delay);
            }
            else if (currentObjective == 8)
            {
                Invoke(nameof(TvReloadGun), delay);
            }
            else if (currentObjective == 9)
            {
                Invoke(nameof(TvArrow2), delay);
            }
            else if (currentObjective == 10)
            {
                Invoke(nameof(TvShotgun), delay);
            }
            else if (currentObjective == 11)
            {
                Invoke(nameof(TVKillEnemy), delay);
            }
            else if (currentObjective == 12)
            {
                Invoke(nameof(TvMagnet), delay);
            }
            else if (currentObjective == 13)
            {
                Invoke(nameof(TVMagnetMessage), delay);
            }



        }
        else if (numberOfRequirements == 1 && quickturn)
        {
            //for (int i = 0; i < tvsInScene.Length; i++)
            //{
            //    //tvsInScene[i].SetQuickTurnDisplay();
            //}
            rightQuest.Joystick(1, true);
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
        rightQuest.QuestActive(false);
    }

}
