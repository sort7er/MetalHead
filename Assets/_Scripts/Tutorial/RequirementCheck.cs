using UnityEngine;
using UnityEngine.InputSystem;

public class RequirementCheck : MonoBehaviour
{
    public InputActionAsset inputAction;
    public AmmoBag ammoBag;
    public CZ50 cz50;

    private InputAction turnLeft;
    private InputAction turnRight;
    private InputAction quickturn;

    private ReturnToHolster cz50ReturnToHolser;
    private ReleaseMag releaseMag;


    private TutorialManager tutorialManager;
    private RelayToTv relay;

    private bool canTurnLeft, canTurnRight;
    private bool canQuickturn;
    private bool liftTriggerEntererd;
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
    public void CanGrabGun()
    {
        canGrabGun = true;
    }
    public void CanReload()
    {
        releaseMag.TutorialCanReload(true);
        tutorialManager.SetAmmoBag(true);
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
    }
    public void ButtonPressed()
    {
        if (!buttonPressed)
        {
            relay.CheckASpot(1);
            buttonPressed = true;
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
    public void MagnetPickedUp()
    {
        if (!magnetPickedup)
        {
            relay.CheckASpot(1);
            magnetPickedup = true;
        }
    }

}
