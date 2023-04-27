using TMPro;
using UnityEngine;

public class ChooseQuickTurn : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    public Transform characterPivot;
    public Animator rightControllerFront;

    private bool quickTurn, changed;
    private Quaternion targetAngle;


    private void OnEnable()
    {
        if (LocomotionManager.instance != null && LocomotionManager.instance.currentQuickTurnType == 1)
        {
            Display(false);
        }
        else if (LocomotionManager.instance != null && LocomotionManager.instance.currentQuickTurnType == 0)
        {
            Display(true);
        }
    }

    public void SwitchQuickturn()
    {
        if (LocomotionManager.instance.currentQuickTurnType == 0)
        {
            Display(false);
        }
        else
        {
            Display(true);
        }
        LocomotionManager.instance.SetQuickTurn();
    }
    private void Display(bool enabled)
    {
        if (enabled)
        {
            if(turnText!= null)
            {
                turnText.text = "Enabled";
            }
            quickTurn = true;
        }
        else
        {
            if (turnText != null)
            {
                turnText.text = "Disabled";
            }
            quickTurn = false;
        }
        CancelInvoke();
        StartSnapAnim();
    }

    private void StartSnapAnim()
    {
        if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
        {
            rightControllerFront.Play("JoystickBack");
        }
        Invoke(nameof(StartSnapAnim), 1.75f);
        if(quickTurn)
        {
            Invoke(nameof(ActualSnap), 0.25f);
        }
    }
    private void ActualSnap()
    {
        characterPivot.Rotate(0, 0, 180);
    }
}
