using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Trigger : MonoBehaviour
{
    public string tagToCompare;
    public InputActionAsset triggerInput;

    private Animator triggerAnim;
    private InputAction triggerLeft, triggerRight;
    private XRDirectInteractor rHand, lHand;
    private float leftTriggerValue, rightTriggerValue;


    private void Start()
    {
        lHand = GameManager.instance.leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = GameManager.instance.rightHand.gameObject.GetComponent<XRDirectInteractor>();
        triggerAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (rHand.selectTarget != null && rHand.selectTarget.CompareTag(tagToCompare))
        {
            triggerAnim.SetFloat("Trigger", rightTriggerValue);
        }
        else if (lHand.selectTarget != null && lHand.selectTarget.CompareTag(tagToCompare))
        {
            triggerAnim.SetFloat("Trigger", leftTriggerValue);
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
        leftTriggerValue = context.ReadValue<float>() * 2;
    }
    private void TriggerRight(InputAction.CallbackContext context)
    {
        rightTriggerValue = context.ReadValue<float>() * 2;
    }

}
