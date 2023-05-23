using TMPro;
using UnityEngine;
using UnityEngine.AI;
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
    public TextMeshProUGUI movementText, turnText, quickTurnText;
    public InputActionAsset inputAction;
    public TeleportationController teleportationController;

    [HideInInspector] public bool isUsingTeleport;
    [HideInInspector] public int currentMoveType;
    [HideInInspector] public int currentTurnType;
    [HideInInspector] public int currentQuickTurnType;

    private TeleportationProvider teleportationProvider;
    private ActionBasedContinuousMoveProvider continuousMoveProvider;
    private ActionBasedSnapTurnProvider snapTurnProvider;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private NavMeshObstacle playerObstacle;


    private InputActionReference continuousMoveInputReference;
    private InputActionReference snapTurnInputReference;
    private InputActionReference continuousTurnInputReference;
    private InputActionAsset teleportationInputReference;
    private InputAction snapTurn;

    private void Start()
    {
        teleportationProvider = GetComponent<TeleportationProvider>();
        continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
        playerObstacle = GameManager.instance.XROrigin.GetComponent<NavMeshObstacle>();

        StartSettings();

        snapTurn = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("Snap Turn");
        snapTurn.Enable();
        snapTurn.performed += OnSnapTurn;
    }

    private void OnDestroy()
    {
        snapTurn.performed -= OnSnapTurn;
    }

    private void SetContinuousMoveInputReference()
    {
        if (continuousMoveProvider.leftHandMoveAction.reference != null)
        {
            continuousMoveInputReference = continuousMoveProvider.leftHandMoveAction.reference;
        }
    }

    private void SetTeleportationInputReference()
    {
        teleportationInputReference = teleportationRays.GetComponentInChildren<TeleportationController>().inputAction;
    }
    private void SetContinousTurnInputReference()
    {
        if (continuousTurnProvider.leftHandTurnAction.reference != null)
        {
            continuousTurnInputReference = continuousTurnProvider.leftHandTurnAction.reference;
        }
    }
    private void SetSnapTurnInputReference()
    {
        if (snapTurnProvider.leftHandSnapTurnAction.reference != null)
        {
            snapTurnInputReference = snapTurnProvider.leftHandSnapTurnAction.reference;
        }
    }
    private void StartSettings()
    {
        currentMoveType = PlayerPrefs.GetInt("MoveType", 0);
        currentTurnType = PlayerPrefs.GetInt("TurnType", 0);
        currentQuickTurnType = PlayerPrefs.GetInt("QuickTurnType", 1);

        if(currentMoveType == 1)
        {
            currentMoveType = 0;
        }
        else
        {
            currentMoveType = 1;
        }
        if (currentTurnType == 1)
        {
            currentTurnType = 0;
        }
        else
        {
            currentTurnType = 1;
        }
        if (currentQuickTurnType == 1)
        {
            currentQuickTurnType = 0;
        }
        else
        {
            currentQuickTurnType = 1;
        }
        SwitchLocomotion();
        SwitchTurning();
        SetQuickTurn();
        EnableMovement(true);
        EnableTurning(true);
    }
    //Locomotion
    public void SwitchLocomotion()
    {
        if(currentMoveType == 0)
        {
            currentMoveType = 1;
            movementText.text = "< Continuous Movement >";
            playerObstacle.enabled = false;
        }
        else
        {
            currentMoveType = 0;
            movementText.text = "< Teleport >";
            playerObstacle.enabled = true;
        }
        PlayerPrefs.SetInt("MoveType", currentMoveType);
    }

    public void SetLocomotion(int movetype)
    {
        currentMoveType = movetype;
        SwitchLocomotion();
    }

    private void SetCountinuous(bool value)
    {
        SetContinuousMoveInputReference();
        if (value)
        {
            continuousMoveProvider.leftHandMoveAction = new InputActionProperty(continuousMoveInputReference);
        }
        continuousMoveProvider.enabled = value;
    }
    private void SetTeleport(bool value)
    {
        SetTeleportationInputReference();
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
        PlayerPrefs.SetInt("TurnType", currentTurnType);
    }

    public void SetTurning(int currentType)
    {
        currentTurnType = currentType;
        SwitchTurning();
    }

    private void SetCountinuousTurn(bool value)
    {
        SetContinousTurnInputReference();
        if (value)
        {
            continuousTurnProvider.leftHandTurnAction = new InputActionProperty(continuousTurnInputReference);
        }
        continuousTurnProvider.enabled = value;
    }
    private void SetSnap(bool value)
    {
        SetSnapTurnInputReference();
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

    public void SetQuickTurn()
    {
        if (currentQuickTurnType == 1)
        {
            currentQuickTurnType = 0;
            quickTurnText.text = "< Enabled >";
            snapTurnProvider.enableTurnAround = true;
        }
        else if (currentQuickTurnType == 0)
        {
            currentQuickTurnType = 1;
            quickTurnText.text = "< Disabled >";
            snapTurnProvider.enableTurnAround = false;
        }
        PlayerPrefs.SetInt("QuickTurnType", currentQuickTurnType);
    }
    public void SetQuickTurnType(int type)
    {
        currentQuickTurnType = type;
        SetQuickTurn();
    }
    private void OnSnapTurn(InputAction.CallbackContext context)
    {
        if (currentTurnType == 1 && !teleportationController.GetTeleportActive() && currentQuickTurnType == 0)
        {
            GameManager.instance.XROrigin.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

}
