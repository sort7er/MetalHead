using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMovement : MonoBehaviour
{
    public TextMeshProUGUI teleportText, continuousText;
    public Image teleportImage, continuousImage;
    public Animator leftControllerSide;
    public Transform character, targetPos;
    public GameObject arm;
    public GameObject teleportMovement;

    private TutorialManager tutorialManager;
    private RelayToTv relayToTv;
    private Vector3 startPos, currentTarget;
    private float currentTime;
    private int multiplier = 1;
    private bool teleport, changed;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        relayToTv = tutorialManager.GetComponent<RelayToTv>();
        startPos = character.localPosition;
    }

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
                CancelInvoke();
                StartTeleport();
            }
            else
            {
                CancelInvoke();
                StartContinuous();
            }
            changed = false;
            arm.SetActive(false);
            character.localPosition = startPos;
        }
        if (!teleport)
        {
            currentTime += Time.deltaTime;
            character.localPosition = Vector3.Lerp(character.localPosition, currentTarget, currentTime / 2);
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
    public void Teleport()
    {
        if (!teleport)
        {
            Display(true);
            LocomotionManager.instance.SetLocomotion(1);
        }
    }
    public void Continuous()
    {
        if (teleport)
        {
            Display(false);
            LocomotionManager.instance.SetLocomotion(0);
        }
    }

    private void Display(bool isTeleport)
    {
        if (isTeleport)
        {
            if (teleportText != null)
            {
                teleportText.alpha = 1f;
                continuousText.alpha = 0.2f;
                continuousImage.color = Color.gray;
                teleportImage.color = Color.white;
            }
            teleport = true;
            changed = true;
        }
        else
        {
            if (teleportText != null)
            {
                teleportText.alpha = 0.2f;
                continuousText.alpha = 1;
                continuousImage.color = Color.white;
                teleportImage.color = Color.gray;
            }
            teleport = false;
            changed = true;
        }

    }

    private void StartContinuous()
    {
        currentTime = 0;

        if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
        {
            leftControllerSide.Play("JoystickFront");
        }

        currentTarget = targetPos.localPosition;

        Invoke(nameof(ContinousBack), 1.75f);
    }
    private void ContinousBack()
    {
        currentTime = 0;

        if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
        {
            leftControllerSide.Play("JoystickBack");
        }

        currentTarget = startPos;

        Invoke(nameof(StartContinuous), 1.75f);
    }

    private void StartTeleport()
    {
        if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
        {
            leftControllerSide.Play("JoystickFront");
        }

        currentTarget = targetPos.localPosition;

        Invoke(nameof(ShowArm), 0.1f);
        Invoke(nameof(ActualTeleport), 0.85f);

        Invoke(nameof(TeleportBack), 1.75f);
    }
    private void TeleportBack()
    {
        if (transform.parent.gameObject.activeSelf && gameObject.activeSelf)
        {
            leftControllerSide.Play("JoystickBack");
        }

        currentTarget = startPos;

        Invoke(nameof(ActualTeleport), 0.75f);
        Invoke(nameof(StartTeleport), 1.75f);
    }
    private void ShowArm()
    {
        arm.SetActive(true);
    }
    private void ActualTeleport()
    {
        character.localPosition = currentTarget;
        arm.SetActive(false);
    }
    public void Select()
    {
        tutorialManager.SetMovement(true);
        if(LocomotionManager.instance.currentMoveType == 0)
        {
            teleportMovement.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            relayToTv.TvMovement();
            tutorialManager.CloseTutorialMenu();
        }
    }
}
