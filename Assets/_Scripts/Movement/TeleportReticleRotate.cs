using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportReticleRotate : MonoBehaviour
{
    public float turnSpeed;
    public InputActionAsset inputAction;

    private Vector2 direction;
    private Vector3 rotationDir;
    private InputAction rightJoystick;
    private float lastAngle, angle;
    private bool getAngle;
    private void Start()
    {
        rightJoystick = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("Move");
        rightJoystick.performed += OnRotateJoystick;
        rightJoystick.Enable();
    }
    private void OnEnable()
    {
        ResetRotation();
    }
    private void OnDestroy()
    {
        rightJoystick.performed -= OnRotateJoystick;
    }

    private void OnRotateJoystick(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        rotationDir = new Vector3(direction.x, 0, direction.y).normalized;
    }

    private void Update()
    {
        float targetAngle = Mathf.Atan2(rotationDir.x, rotationDir.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed);
        if (rotationDir.magnitude >= 0.1f)
        {
            getAngle = true;
            transform.rotation = Quaternion.Euler(0f, GameManager.instance.leftHand.transform.eulerAngles.y + angle, 0f);
        }
        else if (getAngle)
        {
            lastAngle = transform.eulerAngles.y - GameManager.instance.leftHand.transform.eulerAngles.y;
            getAngle= false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, GameManager.instance.leftHand.transform.eulerAngles.y + lastAngle, 0f);
        }
    }
    private void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0f, GameManager.instance.leftHand.transform.eulerAngles.y, 0f);
        lastAngle= 0f;
    }
}
