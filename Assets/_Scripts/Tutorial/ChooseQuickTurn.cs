using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseQuickTurn : MonoBehaviour
{
    public TextMeshProUGUI enabledText, disabledText;
    public Image enabledImage, disabledImage;
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
    public void TurnEnabled()
    {
        if (!quickTurn)
        {
            Display(true);
            LocomotionManager.instance.SetQuickTurnType(1);
        }

    }
    public void TurnDisabled()
    {
        if (quickTurn)
        {
            Display(false);
            LocomotionManager.instance.SetQuickTurnType(0);
        }
    }

    private void Display(bool enabled)
    {
        if (enabled)
        {
            if (enabledText != null)
            {
                enabledText.alpha = 1f;
                disabledText.alpha = 0.2f;
                disabledImage.color = Color.gray;
                enabledImage.color = Color.white;
            }
            quickTurn = true;
        }
        else
        {
            if (enabledText != null)
            {
                enabledText.alpha = 0.2f;
                disabledText.alpha = 1;
                disabledImage.color = Color.white;
                enabledImage.color = Color.gray;
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
        Invoke(nameof(StartSnapAnim), 1.75f * 0.5f);
        if(quickTurn)
        {
            Invoke(nameof(ActualSnap), 0.125f);
        }
    }
    private void ActualSnap()
    {
        characterPivot.Rotate(0, 0, 180);
    }
}
