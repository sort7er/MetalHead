using TMPro;
using UnityEngine;

public class ChooseMovement : MonoBehaviour
{
    public TextMeshProUGUI movementText;
    public Animator leftControllerSide;

    private float currentTime;
    private int multiplier = 1;
    private bool teleport, changed;


    private void OnEnable()
    {
        if (LocomotionManager.instance != null && LocomotionManager.instance.currentMoveType == 1)
        {
            Display(false);
        }
        else if (LocomotionManager.instance != null && LocomotionManager.instance.currentMoveType == 0)
        {
            Display(true);
        }
    }
    private void Update()
    {
        if (changed)
        {
            if (teleport)
            {
                CancelInvoke(nameof(StartContinuous));
            }
            else
            {
                StartContinuous();
            }
            changed = false;
        }
        if (!teleport)
        {
            currentTime += Time.deltaTime;
            //characterPivot.po = Quaternion.Slerp(characterPivot.rotation, targetAngle, currentTime / 2);
        }

    }

    public void SwitchMovement()
    {
        if (LocomotionManager.instance.currentMoveType == 0)
        {
            Display(false);
        }
        else
        {
            Display(true);
        }
        LocomotionManager.instance.SwitchLocomotion();
    }
    private void Display(bool isSnap)
    {
        if (isSnap)
        {
            if (movementText != null)
            {
                movementText.text = "Teleport";
            }
            teleport = true;
            changed = true;
        }
        else
        {
            if (movementText != null)
            {
                movementText.text = "Continuous movement";
            }
            teleport = false;
            changed = true;
        }
    }

    private void StartContinuous()
    {
        if (gameObject.activeSelf)
        {
            leftControllerSide.Play("JoystickFront");
        }

        Invoke(nameof(ContinousBack), 1.75f);
    }
    private void ContinousBack()
    {
        if (gameObject.activeSelf)
        {
            leftControllerSide.Play("JoystickBack");
        }

        Invoke(nameof(StartContinuous), 1.75f);
    }
}
