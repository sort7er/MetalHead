using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Trigger : MonoBehaviour
{
    public string tagToCompare;
    public InputActionAsset triggerInput;

    private Animator triggerAnim;
    private InputAction triggerLeft, triggerRight;
    private float leftTriggerValue, rightTriggerValue;


    private void Start()
    {
        triggerAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (GameManager.instance.CheckHand("Pistol") == 1)
        {
            triggerAnim.SetFloat("Trigger", leftTriggerValue);
        }
        else if (GameManager.instance.CheckHand("Pistol") == 2)
        {
            triggerAnim.SetFloat("Trigger", rightTriggerValue);
        }
    }
    private void OnEnable()
    {
        triggerLeft = triggerInput.FindActionMap("XRI LeftHand Interaction").FindAction("Activate Value");
        triggerLeft.Enable();
        triggerLeft.performed += TriggerLeft;

        triggerRight = triggerInput.FindActionMap("XRI RightHand Interaction").FindAction("Activate Value");
        triggerRight.Enable();
        triggerRight.performed += TriggerRight;
    }

    private void OnDisable()
    {
        triggerLeft.performed -= TriggerLeft;
        triggerRight.performed -= TriggerRight;
    }
    private void TriggerLeft(InputAction.CallbackContext context)
    {
        leftTriggerValue = context.ReadValue<float>();
    }
    private void TriggerRight(InputAction.CallbackContext context)
    {
        rightTriggerValue = context.ReadValue<float>();
    }

}
