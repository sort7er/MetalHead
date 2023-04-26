using TMPro;
using UnityEngine;

public class ChooseTurning : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    public Transform characterPivot;
    public Animator rightControllerFront;

    private float currentTime;
    private int multiplier = 1;
    private bool snap, changed;
    private Quaternion targetAngle;


    private void OnEnable()
    {
        if (LocomotionManager.instance != null && LocomotionManager.instance.currentTurnType == 1)
        {
            Display(false);
        }
        else if(LocomotionManager.instance != null && LocomotionManager.instance.currentTurnType == 0)
        {
            Display(true);
        }
    }
    private void Update()
    {
        if (changed)
        {
            if (snap)
            {
                CancelInvoke(nameof(StartContinuous));
                StartSnapAnim();
            }
            else
            {
                CancelInvoke(nameof(StartSnapAnim));
                CancelInvoke(nameof(ActualSnap));
                StartContinuous();
            }
            changed = false;
        }
        if (!snap)
        {
            currentTime += Time.deltaTime;
            characterPivot.rotation = Quaternion.Slerp(characterPivot.rotation, targetAngle, currentTime / 2);
        }

    }

    public void SwitchTurning()
    {
        if (LocomotionManager.instance.currentTurnType == 0)
        {
            Display(false);
        }
        else
        {
            Display(true);
        }
        LocomotionManager.instance.SwitchTurning();
    }
    private void Display(bool isSnap)
    {
        if (isSnap)
        {
            turnText.text = "Snap turning";
            snap = true;
            changed = true;
        }
        else
        {
            turnText.text = "Continuous turning";
            snap = false;
            changed = true;
        }
    }

    private void StartSnapAnim()
    {
        int direction = Random.Range(0, 2);
        if(direction == 0)
        {
            rightControllerFront.Play("JoystickLeft");
            multiplier = 1;
        }
        else
        {
            rightControllerFront.Play("JoystickRight");
            multiplier = -1;
        }
        Invoke(nameof(ActualSnap), 0.25f);
        Invoke(nameof(StartSnapAnim), 1.75f);
    }
    private void ActualSnap()
    {
        characterPivot.Rotate(0, 0, multiplier * 45);
    }
    private void StartContinuous()
    {
        currentTime = 0;
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            rightControllerFront.Play("JoystickLeft");
            multiplier = 1;
        }
        else
        {
            rightControllerFront.Play("JoystickRight");
            multiplier = -1;
        }

        targetAngle = Quaternion.Euler(characterPivot.eulerAngles.x, characterPivot.eulerAngles.y, characterPivot.eulerAngles.z + multiplier * 105);
        Invoke(nameof(StartContinuous), 1.75f);
    }


}
