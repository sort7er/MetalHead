using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    public static LocomotionManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject teleportationRays;
    public GameObject vignette;
    public TextMeshProUGUI movementText, turnText, vignetteText; 

    [HideInInspector] public bool isUsingTeleport;
 
    private TeleportationProvider teleportationProvider;
    private ActionBasedContinuousMoveProvider continuousMoveProvider;
    private ActionBasedSnapTurnProvider snapTurnProvider;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private WorkAround workAround;
    private int currentMoveType;
    private int currentTurnType;

    private InputActionReference continuousMoveInputReference;
    private InputActionReference snapTurnInputReference;
    private InputActionReference continuousTurnInputReference;
    private InputActionAsset teleportationInputReference;

    private void Start()
    {
        teleportationProvider = GetComponent<TeleportationProvider>();
        continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
        workAround = GetComponent<WorkAround>();

        Invoke("StartSettings", 0.1f);
        Invoke("SetVignette", 0.1f);
        SetContinuousMoveInputReference();
        SetTeleportationInputReference();
        SetContinousTurnInputReference();
        SetSnapTurnInputReference();
    }

    private void SetContinuousMoveInputReference()
    {
        if (continuousMoveProvider.leftHandMoveAction.reference != null)
        {
            continuousMoveInputReference = continuousMoveProvider.leftHandMoveAction.reference;
        }
        else
        {
            Debug.Log("No Continuous Move Provider Input Action was found on the Left Hand. Please set it on your  Left hand Move Action found on the Continuous Move Provider use the Locomotion Manager");
        }
    }

    private void SetTeleportationInputReference()
    {
        teleportationInputReference = teleportationRays.GetComponentInChildren<TeleportationController>().inputAction;
        if (teleportationInputReference == null)
        {
            Debug.Log("No Input Action Asset reference was found in the Teleportation Controller Fixed script. Please assign to use Locomotion Manager");
        }
    }
    private void SetContinousTurnInputReference()
    {
        if (continuousTurnProvider.leftHandTurnAction.reference != null)
        {
            continuousTurnInputReference = continuousTurnProvider.leftHandTurnAction.reference;
        }
        else
        {
            Debug.Log("No Continuous Turn Provider Input Action was found on the Left Hand. Please set it on your  Left hand Turn Action found on the Continuous Move Provider use the Locomotion Manager");
        }
    }
    private void SetSnapTurnInputReference()
    {
        if (snapTurnProvider.leftHandSnapTurnAction.reference != null)
        {
            snapTurnInputReference = snapTurnProvider.leftHandSnapTurnAction.reference;
        }
        else
        {
            Debug.Log("No Continuous Snap Provider Input Action was found on the Left Hand. Please set it on your  Left hand Snap Action found on the Continuous Move Provider use the Locomotion Manager");
        }
    }
    private void StartSettings()
    {
        SetCountinuous(false);
        SetTeleport(true);
        SetCountinuousTurn(false);
        SetSnap(true);
        movementText.text = "< Teleport >";
        turnText.text = "< Snap >";
        vignetteText.text = "< Disabled >";
    }
    //Locomotion
    public void SwitchLocomotion()
    {
        if(currentMoveType == 0)
        {
            currentMoveType = 1;
            movementText.text = "< Continuous Movement >";
        }
        else
        {
            currentMoveType = 0;
            movementText.text = "< Teleport >";
        }
    }

    private void SetCountinuous(bool value)
    {
        if (value)
        {
            continuousMoveProvider.leftHandMoveAction = new InputActionProperty(continuousMoveInputReference);
        }
        continuousMoveProvider.enabled = value;
    }
    private void SetTeleport(bool value)
    {
        if (value)
        {
            teleportationRays.GetComponentInChildren<TeleportationController>().inputAction = teleportationInputReference;

        }
        isUsingTeleport = value;
        teleportationRays.SetActive(value);
        teleportationProvider.enabled = value;
    }
    public void EnableMovement(bool state)
    {
        if (state == false)
        {
            SetCountinuous(false);
            SetTeleport(false);
        }
        else
        {
            if (currentMoveType == 0)
            {
                SetCountinuous(false);
                SetTeleport(true);
            }
            else if (currentMoveType == 1)
            {
                SetTeleport(false);
                SetCountinuous(true);
            }
        }
    }


    //Turning
    public void SwitchTurning()
    {
        if(currentTurnType == 0)
        {
            currentTurnType = 1;
            turnText.text = "< Continuous Turning >";
        }
        else
        {
            currentTurnType = 0;
            turnText.text = "< Snap >";
        }
    }
    private void SetCountinuousTurn(bool value)
    {
        if (value)
        {
            continuousTurnProvider.leftHandTurnAction = new InputActionProperty(continuousTurnInputReference);
        }
        continuousTurnProvider.enabled = value;
    }
    private void SetSnap(bool value)
    {
        if (value)
        {
            snapTurnProvider.leftHandSnapTurnAction= new InputActionProperty(snapTurnInputReference);
        }
        snapTurnProvider.enabled = value;

    }

    public void EnableTurning(bool state)
    {
        if(state == false)
        {
            SetSnap(false);
            SetCountinuousTurn(false);
        }
        else
        {
            if(currentTurnType == 0)
            {
                SetCountinuousTurn(false);
                SetSnap(true);
            }
            else if (currentTurnType == 1)
            {
                SetSnap(false);
                SetCountinuousTurn(true);
            }
        }
    }
    public void SetVignette()
    {
        if (!vignette.activeSelf)
        {
            vignette.SetActive(true);
            vignetteText.text = "< Enabled >";
        }
        else if (vignette.activeSelf)
        {
            vignette.SetActive(false);
            vignetteText.text = "< Disabled >";
        }
    }

}
