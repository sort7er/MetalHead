using TMPro;
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
    public GameObject vignette;
    public TextMeshProUGUI movementText, turnText, vignetteText; 

    [HideInInspector] public bool isUsingTeleport;
 
    private TeleportationProvider teleportationProvider;
    private ContinuousMoveProviderBase continuousMoveProvider;
    private ActionBasedSnapTurnProvider snapTurnProvider;
    private ActionBasedContinuousTurnProvider continuousTurnProvider;
    private WorkAround workAround;
    private int currentMoveType;
    private int currentTurnType;

    private void Start()
    {
        teleportationProvider = GetComponent<TeleportationProvider>();
        continuousMoveProvider = GetComponent<ContinuousMoveProviderBase>();
        snapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        continuousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
        workAround = GetComponent<WorkAround>();

        Invoke("StartSettings", 0.1f);
        Invoke("SetVignette", 0.1f);
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
            workAround.EnableMove();
        }
        else
        {
            continuousMoveProvider.enabled = value;
        }
    }
    private void SetTeleport(bool value)
    {
        isUsingTeleport = value;
        teleportationRays.SetActive(value);
        if (value)
        {
            workAround.EnableTeleport();
        }
        else
        {
            teleportationProvider.enabled = value;
        }
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
            workAround.EnableTurning();
        }
        else
        {
            continuousTurnProvider.enabled = value;
        }
    }
    private void SetSnap(bool value)
    {
        if (value)
        {
            workAround.EnableSnap();
        }
        else
        {
            snapTurnProvider.enabled = value;
        }
        
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
