using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionManager : MonoBehaviour
{
    public static LocomotionManager instance;

    private void Awake()
    {
        instance = this;
    }
    public GameObject teleportationRays;

    [HideInInspector] public bool isUsingTeleport;
 
    private TeleportationProvider teleportationProvider;
    private ContinuousMoveProviderBase continuousMoveProvider;
    private ActionBasedSnapTurnProvider snapTurnProvider;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private int currentMoveType;
    private int currentTurnType;

    private void Start()
    {
        teleportationProvider = GetComponent<TeleportationProvider>();
        continuousMoveProvider = GetComponent<ContinuousMoveProviderBase>();
        snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
    }


    //Locomotion
    public void SwitchLocomotion(int locomotionValue)
    {
        currentMoveType = locomotionValue;

        if(currentMoveType == 0)
        {
            SetCountinuous(false);
            SetTeleport(true);
        }
        else if (currentMoveType == 1)
        {
            SetCountinuous(true);
            SetTeleport(false);
        }
    }

    private void SetCountinuous(bool value)
    {
        continuousMoveProvider.enabled = value;
    }
    private void SetTeleport(bool value)
    {
        isUsingTeleport = value;
        teleportationRays.SetActive(value);
        teleportationProvider.enabled = value;
    }

    //Turning
    public void SwitchTurning(int turnValue)
    {
        currentTurnType = turnValue;

        if (currentTurnType == 0)
        {
            SetCountinuousTurn(false);
            SetSnap(true);
        }
        else if (currentTurnType == 1)
        {
            SetCountinuousTurn(true);
            SetSnap(false);
        }
    }
    private void SetCountinuousTurn(bool value)
    {
        continuousTurnProvider.enabled = value;
    }
    private void SetSnap(bool value)
    {
        snapTurnProvider.enabled = value;
    }

    public void EnableTurning(bool state)
    {
        if(state == false)
        {
            snapTurnProvider.enabled = false;
            continuousTurnProvider.enabled = false;
        }
        else
        {
            if(currentTurnType == 0)
            {
                snapTurnProvider.enabled = true;
                continuousTurnProvider.enabled = false;
            }
            else if (currentTurnType == 1)
            {
                snapTurnProvider.enabled = false;
                continuousTurnProvider.enabled = true;
            }
        }
    }
}
