using UnityEngine;
using UnityEngine.InputSystem;

public class RequirementCheck : MonoBehaviour
{

    public InputActionAsset inputAction;



    private InputAction turnLeft;
    private InputAction turnRight;
    private InputAction quickturn;

    private TV tv;

    private bool canTurnLeft, canTurnRight;
    private bool canQuickturn;
    private bool liftTriggerEntererd;


    private void Start()
    {
        tv = GetComponent<TV>();

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
            tv.TurnLeftDone();
            canTurnLeft = false;
        }
    }
    private void OnTurnRight(InputAction.CallbackContext context)
    {
        if(canTurnRight && !GameManager.instance.isPaused)
        {
            tv.TurnRightDone();
            canTurnRight = false;
        }
    }
    private void OnQuickturn(InputAction.CallbackContext context)
    {
        if (canQuickturn && !GameManager.instance.isPaused)
        {
            tv.QuickturnDone();
            canQuickturn = false;
        }
    }
    public void LiftTriggerEntered()
    {
        if (!liftTriggerEntererd)
        {
            tv.LiftTriggerEntered();
            liftTriggerEntererd = true;
        }
    }

}
