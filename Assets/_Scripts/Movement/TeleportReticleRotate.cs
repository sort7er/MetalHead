using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportReticleRotate : MonoBehaviour
{
    public float turnSpeed;
    public InputActionAsset inputAction;
    public LayerMask groundMask;

    private Vector2 direction;
    private Vector3 rotationDir;
    private InputAction rightJoystick;
    private float lastAngle, angle;
    private bool getAngle;

    private Vector3 normalDirection;
    private Vector3 forwardDirection, rightDirection;
    private float raycastAngleX, raycastAngleZ;
    private float differenceAngleX, differenceAngleZ;

    private void OnEnable()
    {
        ResetRotation();
        rightJoystick = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("Move");
        rightJoystick.performed += OnRotateJoystick;
        rightJoystick.Enable();
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2, groundMask))
        {
            normalDirection = hit.normal;
            forwardDirection = new Vector3(transform.forward.x, 0f, transform.forward.z);
            rightDirection = new Vector3(transform.right.x, 0f, transform.right.z);
            differenceAngleX = Vector3.Angle(normalDirection, forwardDirection);
            differenceAngleZ = Vector3.Angle(normalDirection, rightDirection);
            raycastAngleX = 90f - differenceAngleX;
            raycastAngleZ = 90f - differenceAngleZ;
        }


        float targetAngle = Mathf.Atan2(rotationDir.x, rotationDir.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed);
        if (rotationDir.magnitude >= 0.1f)
        {
            getAngle = true;
            transform.rotation = Quaternion.Euler(raycastAngleX, GameManager.instance.leftHand.transform.eulerAngles.y + angle, -raycastAngleZ);
        }
        else if (getAngle)
        {
            lastAngle = transform.eulerAngles.y - GameManager.instance.leftHand.transform.eulerAngles.y;
            getAngle = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(raycastAngleX, GameManager.instance.leftHand.transform.eulerAngles.y + lastAngle, -raycastAngleZ);
        }
    }
    private void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(raycastAngleX, GameManager.instance.leftHand.transform.eulerAngles.y, -raycastAngleZ);
        lastAngle= 0f;
    }
}
