using UnityEngine;
using UnityEngine.InputSystem;

public class RequirementCheck : MonoBehaviour
{
    public InputActionAsset inputAction;
    public AmmoBag ammoBag;
    public CZ50 cz50;
    public Magnet magnet;

    private InputAction turnLeft;
    private InputAction turnRight;
    private InputAction quickturn;

    private ReturnToHolster cz50ReturnToHolser;
    private ReleaseMag releaseMag;


    private TutorialManager tutorialManager;
    private RelayToTv relay;

    private bool canTurnLeft, canTurnRight;
    private bool canQuickturn;
    private bool canPressMenu;
    private bool liftTriggerEntererd;
    private bool objectRoomEntererd;
    private bool grabObjectRight, releasedObjectRight;
    private bool grabObjectLeft, releasedObjectLeft;
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


        if(gunEmptied && !slidePulled)
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
            tutorialManager.leftQuest.Nothing();
            tutorialManager.LeftQuestActive(false);
            tutorialManager.RightQuestActive(false);
        }
    }
    public void LiftTriggerExited()
    {
        if (!buttonPressed)
        {
            GameManager.instance.leftHand.SetHandActive(false);
            GameManager.instance.rightHand.SetHandActive(false);
            tutorialManager.LeftQuestActive(true);
            tutorialManager.RightQuestActive(true);
            tutorialManager.leftQuest.Joystick(0);
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
        tutorialManager.leftQuest.Nothing();
        tutorialManager.LeftQuestActive(false);
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
            relay.CheckASpot(0);
            grabObjectRight = true;
        }
    }
    public void ReleasedObjectRight()
    {
        if (!releasedObjectRight)
        {
            tutorialManager.RightQuestActive(false);
            tutorialManager.rightQuest.Nothing();
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
            tutorialManager.leftQuest.Nothing();
            tutorialManager.LeftQuestActive(false);
            relay.CheckASpot(1);
            releasedObjectLeft = true;
        }
    }


    public void FiringRangeEntered()
    {
        if (!firingRangeEntererd)
        {
            relay.CheckASpot(0);
            firingRangeEntererd = true;
        }
    }
    public void GunGrabbed()
    {
        if (!gunGrabbed && canGrabGun)
        {
            cz50ReturnToHolser.enabled = true;
            relay.CheckASpot(0);
            gunGrabbed = true;
        }
    }
    public void ShootGun()
    {
        if (!gunEmptied)
        {
            bulletsShoot++;
            if(bulletsShoot >= 10)
            {
                relay.CheckASpot(0);
                gunEmptied = true;
            }
        }
    }
    public void MagDropped()
    {
        if (!magDropped)
        {
            relay.NextReload();
            relay.CheckASpot(0);
            magDropped = true;
        }
    }
    public void GrabMag()
    {
        if (!magGrabbed)
        {
            relay.NextReload();
            relay.CheckASpot(1);
            magGrabbed = true;
        }
    }
    public void MagInserted()
    {
        if (!magInserted)
        {
            relay.NextReload();
            relay.CheckASpot(2);
            magInserted = true;
        }
    }
    public void SlidePulled()
    {
        if (!slidePulled)
        {
            relay.CheckASpot(3);
            slidePulled = true;
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
