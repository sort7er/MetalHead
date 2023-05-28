using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseTurning : MonoBehaviour
{
    public string continuousDescription, snapDescription;
    public TextMeshProUGUI snapText, continousText, descriptionText;
    public Image snapImage, continousImage;
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

    public void Snap()
    {
        if (!snap)
        {
            Display(true);
            LocomotionManager.instance.SetTurning(1);
            
        }
        
    }
    public void Continous()
    {
        if (snap)
        {
            Display(false);
            LocomotionManager.instance.SetTurning(0);
            
        }
        
    }
    private void Display(bool isSnap)
    {
        if (isSnap)
        {
            if(continousText != null)
            {
                continousText.alpha = 0.2f;
                snapText.alpha = 1;
                snapImage.color = Color.white;
                continousImage.color = Color.gray;
            }
            descriptionText.text = snapDescription;
            snap = true;
            changed = true;
        }
        else
        {
            if (continousText != null)
            {
                continousText.alpha = 1;
                snapText.alpha = 0.2f;
                snapImage.color = Color.gray;
                continousImage.color = Color.white;
            }
            descriptionText.text = continuousDescription;
            snap = false;
            changed = true;
        }
    }

    private void StartSnapAnim()
    {
        int direction = Random.Range(0, 2);
        if(direction == 0)
        {
            if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
            {
                rightControllerFront.Play("JoystickLeft");
            }
            multiplier = 1;
        }
        else
        {
            if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
            {
                rightControllerFront.Play("JoystickRight");
            }
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
            if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
            {
                rightControllerFront.Play("JoystickLeft");
            }
            multiplier = 1;
        }
        else
        {
            if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
            {
                rightControllerFront.Play("JoystickRight");
            }
            multiplier = -1;
        }

        targetAngle = Quaternion.Euler(characterPivot.eulerAngles.x, characterPivot.eulerAngles.y, characterPivot.eulerAngles.z + multiplier * 105);
        Invoke(nameof(StartContinuous), 1.75f);
    }


}
