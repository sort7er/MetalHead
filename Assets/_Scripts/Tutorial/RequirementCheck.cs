using UnityEngine;
using UnityEngine.InputSystem;

public class RequirementCheck : MonoBehaviour
{
    public InputActionAsset inputAction;

    private InputAction turnLeft;
    private InputAction turnRight;
    private InputAction quickturn;

    private RelayToTv relay;

    private bool canTurnLeft, canTurnRight;
    private bool canQuickturn;
    private bool liftTriggerEntererd;
    private bool buttonPressed;
    private bool gunGrabbed;
    private bool gunDropped;

    //Reload
    private bool magDropped;
    private bool magGrabbed;
    private bool magInserted;
    private bool slidePulled;


    private void Start()
    {
        relay = GetComponent<RelayToTv>();

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
    public void GunGrabbed()
    {
        if (!gunGrabbed)
        {
            relay.CheckASpot(0);
            gunGrabbed = true;
        }
    }
    public void GunDropped()
    {
        if (!gunDropped)
        {
            relay.CheckASpot(1);
            gunDropped = true;
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

}
