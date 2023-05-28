using UnityEngine;
using UnityEngine.InputSystem;

public class RequirementCheck : MonoBehaviour
{
    public InputActionAsset inputAction;
    public AmmoBag ammoBag;
    public CZ50 cz50;
    public Magnet magnet;
    public Transform magPos;
    public Transform slide;

    private InputAction turnLeft;
    private InputAction turnRight;
    private InputAction quickturn;

    private ReturnToHolster cz50ReturnToHolser;
    private ReleaseMag releaseMag;

    private QuestController leftQuest, rightQuest;
    private TutorialManager tutorialManager;
    private RelayToTv relay;

    private bool canTurnLeft, canTurnRight;
    private bool canQuickturn;
    private bool canPressMenu;
    private bool liftTriggerEntererd;
    private bool objectRoomEntererd;
    private bool grabObjectRight, releasedObjectRight;
    private bool grabObjectLeft, releasedObjectLeft;
    private bool gripLeft, gripRight, holdingLeft, holdingRight;
    private bool firingRangeEntererd;
    private bool buttonPressed;
    private bool gunGrabbed;
    private bool canGrabGun;
    private bool magnetRoomEntered;

    //Shooting
    private bool gunEmptied;
    private int bulletsShoot;

    //Reload
    private bool magDropped;
    private bool magGrabbed;
    private bool magInserted;
    private bool slidePulled;

    //Magnet
    private bool magnetGrabbed;
    private bool magnetPickedup;


    private void Start()
    {

        releaseMag = cz50.GetComponent<ReleaseMag>();
        cz50ReturnToHolser = cz50.GetComponent<ReturnToHolster>();
        relay = GetComponent<RelayToTv>();
        tutorialManager = GetComponent<TutorialManager>();
        cz50ReturnToHolser.enabled = false;
        leftQuest = GameManager.instance.leftHand.questController;
        rightQuest = GameManager.instance.rightHand.questController;

        turnLeft = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("TurnLeft");
        turnLeft.Enable();
        turnLeft.performed += OnTurnLeft;

        turnRight = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("TurnRight");
        turnRight.Enable();
        turnRight.performed += OnTurnRight;

        quickturn = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("Snap Turn");
        quickturn.Enable();
        quickturn.performed += OnQuickturn;

    }

    private void Update()
    {
        if(canPressMenu && GameManager.instance.isPaused)
        {
            MenuPressed();
        }


        if (canGrabGun && !slidePulled)
        {

            if (GameManager.instance.leftHand.IsHovering())
            {
                if (!gripLeft)
                {
                    leftQuest.QuestActive(true);
                    leftQuest.Grip();
                    gripLeft = true;
                    holdingLeft = false;
                }

            }
            else if (!GameManager.instance.leftHand.IsHovering() && GameManager.instance.CheckGameObject(cz50.gameObject) == 1)
            {
                if (!holdingLeft)
                {
                    if (gunEmptied && !magDropped)
                    {
                        leftQuest.Secondary();
                    }
                    else if (magDropped)
                    {
                        leftQuest.Nothing();
                        leftQuest.QuestActive(false);
                    }

                    if (magInserted)
                    {
                        rightQuest.QuestActive(true);
                        rightQuest.Grip();
                    }

                    holdingLeft = true;
                }

            }
            else if (!GameManager.instance.leftHand.IsHovering() && GameManager.instance.CheckGameObject(cz50.gameObject) != 1)
            {
                if (gripLeft!)
                {
                    leftQuest.QuestActive(false);
                    gripLeft = false;
                }

            }


            if (GameManager.instance.rightHand.IsHovering())
            {
                if (!gripRight)
                {
                    rightQuest.QuestActive(true);
                    rightQuest.Grip();
                    gripRight = true;
                    holdingRight = false;
                }
            }
            else if (!GameManager.instance.rightHand.IsHovering() && GameManager.instance.CheckGameObject(cz50.gameObject) == 2)
            {
                if (!holdingRight)
                {
                    if (gunEmptied && !magDropped)
                    {
                        rightQuest.Secondary();
                    }
                    else if (magDropped)
                    {
                        rightQuest.QuestActive(false);
                    }

                    if (magInserted)
                    {
                        leftQuest.QuestActive(true);
                        leftQuest.Grip();
                    }

                    holdingRight = true;
                }
            }
            else if (!GameManager.instance.rightHand.IsHovering() && GameManager.instance.CheckGameObject(cz50.gameObject) != 2)
            {
                if (gripRight)
                {
                    rightQuest.QuestActive(false);
                    gripRight = false;
                }
            }
        }



        if (gunEmptied && !slidePulled)
        {
            if (releaseMag.release && !magDropped)
            {
                MagDropped();
            }
            else if(!magGrabbed && magDropped && !ammoBag.tutorialCheck)
            {
                GrabMag();
            }
            else if(!magInserted && magGrabbed && releaseMag.insert)
            {
                MagInserted();
            }
            else if (!slidePulled && magInserted && !cz50.reloadNeeded)
            {
                SlidePulled();
            }
        }

        if(magnetGrabbed && !magnetPickedup)
        {
            if(magnet.GetMetalsCollected() > 0)
            {
                MagnetPickedUp();
            }
        }
    }

    private void OnDestroy()
    {
        turnLeft.performed -= OnTurnLeft;
        turnRight.performed -= OnTurnRight;
        quickturn.performed -= OnQuickturn;
    }

    //Enable requirements
    public void CanTurn()
    {
        canTurnLeft = true;
        canTurnRight = true;
    }
    public void CanQuickturn()
    {
        canQuickturn = true;
    }
    public void CanPressMenu(bool state)
    {
        canPressMenu = state;
    }
    public void CanGrabGun()
    {
        canGrabGun = true;
    }

    public void CanReload()
    {
        releaseMag.TutorialCanReload(true);
    }

    //Inputs
    private void OnTurnLeft(InputAction.CallbackContext context)
    {
        if (canTurnLeft && !GameManager.instance.isPaused)
        {
            relay.CheckASpot(0);
            canTurnLeft = false;
        }
    }
    private void OnTurnRight(InputAction.CallbackContext context)
    {
        if(canTurnRight && !GameManager.instance.isPaused)
        {
            relay.CheckASpot(1);
            canTurnRight = false;
        }
    }
    private void OnQuickturn(InputAction.CallbackContext context)
    {
        if (canQuickturn && !GameManager.instance.isPaused)
        {
            relay.QuickturnDone();
            canQuickturn = false;
        }
    }
    public void LiftTriggerEntered()
    {
        if (!liftTriggerEntererd)
        {

            relay.CheckASpot(0);
            liftTriggerEntererd = true;
        }
        if(!buttonPressed)
        {
            GameManager.instance.leftHand.SetHandActive(true);
            GameManager.instance.rightHand.SetHandActive(true);
            leftQuest.QuestActive(false);
            rightQuest.QuestActive(false);
        }
    }
    public void LiftTriggerExited()
    {
        if (!buttonPressed)
        {
            GameManager.instance.leftHand.SetHandActive(false);
            GameManager.instance.rightHand.SetHandActive(false);
            leftQuest.QuestActive(true);
            rightQuest.QuestActive(true);
            leftQuest.Joystick(0);
        }
    }
    public void ButtonPressed()
    {
        if (!buttonPressed)
        {
            relay.CheckASpot(1);
            buttonPressed = true;
        }
    }
    public void MenuPressed()
    {
        leftQuest.QuestActive(false);
        GameManager.instance.leftHand.SetHandActive(true);
        CanPressMenu(false);
    }
    public void ObjectRoomEntered()
    {
        if (!objectRoomEntererd)
        {
            relay.CheckASpot(0);
            objectRoomEntererd = true;
        }
    }

    public void GrabObjectRight()
    {
        if(!grabObjectRight)
        {
            Guide.instance.GuideDone();
            relay.CheckASpot(0);
            grabObjectRight = true;
        }
    }
    public void ReleasedObjectRight()
    {
        if (!releasedObjectRight)
        {
            rightQuest.QuestActive(false);
            GameManager.instance.rightHand.SetHandActive(false);
            

            relay.CheckASpot(1);
            releasedObjectRight = true;
        }
    }
    public void GrabObjectLeft()
    {
        if(!grabObjectLeft)
        {
            relay.CheckASpot(0);
            grabObjectLeft = true;
        }
    }
    public void ReleasedObjectLeft()
    {

        if(!releasedObjectLeft && releasedObjectRight)
        {
            GameManager.instance.rightHand.SetHandActive(true);
            GameManager.instance.EnableRightInteractor(true);
            leftQuest.QuestActive(false);
            relay.CheckASpot(1);
            releasedObjectLeft = true;
        }
    }


    public void FiringRangeEntered()
    {
        if (!firingRangeEntererd)
        {
            Guide.instance.SetGuide(3, cz50ReturnToHolser.transform, "Grab the gun");
            relay.CheckASpot(0);
            firingRangeEntererd = true;
        }
    }
    public void GunGrabbed()
    {
        if (!gunGrabbed && canGrabGun)
        {
            Guide.instance.GuideDone();
            cz50ReturnToHolser.enabled = true;
            relay.CheckASpot(0);
            gunGrabbed = true;
        }

        if (!gunEmptied)
        {
            if(GameManager.instance.CheckGameObject(cz50.gameObject) == 1)
            {
                leftQuest.QuestActive(true);
                leftQuest.Trigger();
            }
            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 2)
            {
                rightQuest.QuestActive(true);
                rightQuest.Trigger();
            }
        }
    }
    public void ShootGun()
    {
        if (!gunEmptied)
        {

            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 1)
            {
                leftQuest.QuestActive(false);
            }
            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 2)
            {
                rightQuest.QuestActive(false);
            }


            bulletsShoot++;
            if(bulletsShoot >= 10)
            {
                relay.CheckASpot(0);
                gunEmptied = true;
                if (GameManager.instance.CheckGameObject(cz50.gameObject) == 1)
                {
                    leftQuest.QuestActive(true);
                    leftQuest.Secondary();
                }
                if (GameManager.instance.CheckGameObject(cz50.gameObject) == 2)
                {
                    rightQuest.QuestActive(true);
                    rightQuest.Secondary();
                }
            }
        }
    }
    public void MagDropped()
    {
        if (!magDropped)
        {
            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 1)
            {
                leftQuest.QuestActive(false);
            }
            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 2)
            {
                rightQuest.QuestActive(false);
            }

            relay.NextReload();
            relay.CheckASpot(0);
            magDropped = true;
            Guide.instance.SetGuide(2, GameManager.instance.ammoBag.transform, "Grab ammo");
        }
    }
    public void GrabMag()
    {
        if (!magGrabbed)
        {
            relay.NextReload();
            relay.CheckASpot(1);
            magGrabbed = true;
            Guide.instance.SetGuide(2, magPos, "Insert");
        }
    }
    public void MagInserted()
    {
        if (!magInserted)
        {
            relay.NextReload();
            relay.CheckASpot(2);
            magInserted = true;

            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 1)
            {
                rightQuest.QuestActive(true);
                rightQuest.Grip();
            }
            if (GameManager.instance.CheckGameObject(cz50.gameObject) == 2)
            {
                leftQuest.QuestActive(true);
                leftQuest.Grip();
            }
            Guide.instance.SetGuide(2, slide, "Pull slide");
        }
    }
    public void SlidePulled()
    {
        if (!slidePulled)
        {
            relay.CheckASpot(3);
            slidePulled = true;

            leftQuest.QuestActive(false);
            rightQuest.QuestActive(false);
            Guide.instance.GuideDone();
        }
    }
    public void MagnetRoomEntered()
    {
        if (!magnetRoomEntered)
        {
            relay.CheckASpot(0);
            magnetRoomEntered = true;
        }
    }

    public void MagnetGrabbed()
    {
        if (!magnetGrabbed)
        {
            relay.CheckASpot(0);
            relay.NextMagnet();
            magnetGrabbed = true;
        }
    }
    private void MagnetPickedUp()
    {
        if (!magnetPickedup)
        {
            relay.CheckASpot(1);
            magnetPickedup = true;
        }
    }

}
