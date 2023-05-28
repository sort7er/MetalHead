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
    public TextMeshProUGUI movementText, turnText, quickTurnText;
    public InputActionAsset inputAction;
    public TeleportationController teleportationController;

    [HideInInspector] public bool isUsingTeleport;
    [HideInInspector] public int currentMoveType;
    [HideInInspector] public int currentTurnType;
    [HideInInspector] public int currentQuickTurnType;

    private ActionBasedContinuousMoveProvider continuousMoveProvider;
    private ActionBasedSnapTurnProvider snapTurnProvider;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private NavMeshObstacle playerObstacle;

    //Work around
    private float startTurnSpeed;
    private float startMoveSpeed;
    private float startSnapAmount;

    private void Start()
    {
        continuousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
        playerObstacle = GameManager.instance.XROrigin.GetComponent<NavMeshObstacle>();

        startMoveSpeed = continuousMoveProvider.moveSpeed;
        startTurnSpeed = continuousTurnProvider.turnSpeed;
        startSnapAmount = snapTurnProvider.turnAmount;


        StartSettings();
        
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
        if(value)
        {
            continuousMoveProvider.moveSpeed = startMoveSpeed;
        }
        else
        {
            continuousMoveProvider.moveSpeed = 0;
        }
    }
    private void SetTeleport(bool value)
    {
        isUsingTeleport = value;
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
                SetTeleport(true);
                SetCountinuous(false);
            }
            else if (currentMoveType == 1)
            {
                SetCountinuous(true);
                SetTeleport(false);
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
        if(value)
        {
            continuousTurnProvider.turnSpeed = startTurnSpeed;
        }
        else
        {
            continuousTurnProvider.turnSpeed = 0;
        }
    }
    private void SetSnap(bool value)
    {
        if (value)
        {
            snapTurnProvider.turnAmount = startSnapAmount;
        }
        else
        {
            snapTurnProvider.turnAmount = 0;
        }

    }

    public void EnableTurning(bool state)
    {
        if(state == false)
        {
            SetSnap(false);
            SetCountinuousTurn(false);
            TurnAround(false);
        }
        else
        {
            if(currentTurnType == 0)
            {
                SetSnap(true);
                SetCountinuousTurn(false);
            }
            else if (currentTurnType == 1)
            {
                SetCountinuousTurn(true);
                SetSnap(false);
            }
            if(currentQuickTurnType== 0)
            {
                TurnAround(true);
            }
        }
    }

    public void SetQuickTurn()
    {
        if (currentQuickTurnType == 1)
        {
            currentQuickTurnType = 0;
            quickTurnText.text = "< Enabled >";
            TurnAround(true);
        }
        else if (currentQuickTurnType == 0)
        {
            currentQuickTurnType = 1;
            quickTurnText.text = "< Disabled >";
            TurnAround(false);
        }
        PlayerPrefs.SetInt("QuickTurnType", currentQuickTurnType);
    }
    public void SetQuickTurnType(int type)
    {
        currentQuickTurnType = type;
        SetQuickTurn();
    }
    public void TurnAround(bool state)
    {
        snapTurnProvider.enableTurnAround = state;
    }

}
