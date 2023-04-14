using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationController : MonoBehaviour
{
    static private bool teleportActive = false;

    public InputActionAsset inputAction;
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;
    public Hand leftHand;
    public LayerMask bothTeleportLayers;
    public LayerMask teleportLayer;
    public TeleportCheck teleportCheck;

    private XRInteractorLineVisual leftLineVisual;
    private InputAction thumbstickInputAction;
    private InputAction teleportActivate;
    private InputAction teleportCancel;
    private InputAction teleportBack;
    private InputAction snapTurn;
    private Transform teleportReticle;
    private float playerRadius;

    private void Start()
    {
        playerRadius = GameManager.instance.XROrigin.GetComponent<CharacterController>().radius;
        teleportActive = false;
        rayInteractor.enabled = false;
        leftLineVisual = rayInteractor.GetComponent<XRInteractorLineVisual>();

        teleportActivate = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        teleportActivate.Enable();
        teleportActivate.performed += OnTeleportActivate;

        teleportCancel = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        teleportCancel.Enable();
        teleportCancel.performed += OnTeleportCancel;

        teleportBack = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Back");
        teleportBack.Enable();
        teleportBack.performed += OnTeleportBack;

        snapTurn = inputAction.FindActionMap("XRI RightHand Locomotion").FindAction("Snap Turn");
        snapTurn.Enable();
        snapTurn.performed += OnSnapTurn;

        thumbstickInputAction = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        thumbstickInputAction.Enable();
    }
    private void OnDestroy()
    {
        teleportActivate.performed-= OnTeleportActivate;
        teleportCancel.performed -= OnTeleportCancel;
        teleportBack.performed -= OnTeleportBack;
        snapTurn.performed -= OnSnapTurn;

    }

    private void Update()
    {
        if(teleportReticle == null)
        {
            FindReticle();
        }

        if (!teleportActive)
        {
            return;
        }
        if (!rayInteractor.enabled)
        {
            return;
        }
        if (thumbstickInputAction.triggered)
        {
            return;
        }
        if (teleportCheck.CannotTeleport())
        {
            SetRay(false);
            return;
        }
        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rayCastHit))
        {
            SetRay(false);
            return;
        }
        if (rayCastHit.collider.gameObject.layer != 6)
        {
            SetRay(false);
            return;
        }

        TeleportRequest teleportRequest = new TeleportRequest()
        {
            destinationPosition = rayCastHit.point,
        };

        teleportationProvider.QueueTeleportRequest(teleportRequest);
        GameManager.instance.SetXROriginRotation(teleportReticle);
        SetRay(false);

    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        if(!teleportActive)
        {
            SetRay(true);
        }
    }
    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if (teleportActive && rayInteractor.enabled)
        {
            SetRay(false);
        }
    }
    private void OnTeleportBack(InputAction.CallbackContext context)
    {
        if(LocomotionManager.instance.currentMoveType == 0)
        {
            Vector3 teleportDirection = Quaternion.Euler(0, GameManager.instance.cam.transform.rotation.y, 0) * new Vector3(GameManager.instance.cam.transform.forward.x, 0, GameManager.instance.cam.transform.forward.z);
            RaycastHit hit;
            if(Physics.Raycast(GameManager.instance.XROrigin.transform.position + new Vector3(0, 0.75f, 0), -teleportDirection, out hit, 1, bothTeleportLayers))
            {
                if((GameManager.instance.XROrigin.transform.position + new Vector3(0, 0.75f, 0) - hit.point).magnitude <= 0.5f)
                {
                    Debug.Log("NoTeleport");
                }
                else
                {
                    Vector3 downPos = hit.point + teleportDirection * 0.5f;
                    RaycastHit hit2;
                    if (Physics.Raycast(downPos, Vector3.down, out hit2, 2.5f, teleportLayer))
                    {
                        TeleportRequest teleportRequest = new TeleportRequest()
                        {
                            destinationPosition = hit2.point,
                        };

                        teleportationProvider.QueueTeleportRequest(teleportRequest);
                    }
                    else
                    {
                        Debug.Log("NoTeleport");
                    }
                }
            }
            else
            {
                Vector3 downPos = GameManager.instance.XROrigin.transform.position + new Vector3(0, 0.75f, 0) - teleportDirection;
                RaycastHit hit3;
                if (Physics.Raycast(downPos, Vector3.down, out hit3, 2.5f, teleportLayer))
                {
                    TeleportRequest teleportRequest = new TeleportRequest()
                    {
                        destinationPosition = hit3.point,
                    };

                    teleportationProvider.QueueTeleportRequest(teleportRequest);
                }
                else
                {
                    Debug.Log("NoTeleport");
                }
            }
            
        }
    }
    private void OnSnapTurn(InputAction.CallbackContext context)
    {
        if (LocomotionManager.instance.currentTurnType == 1 && !teleportActive)
        {
            GameManager.instance.XROrigin.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
    private void SetRay(bool state)
    {
        if (LocomotionManager.instance.isUsingTeleport)
        {
            if (GameManager.instance.isUpgrading)
            {
                GameManager.instance.EnableRays(!state);
            }
            rayInteractor.enabled = state;
            teleportActive = state;
            leftHand.UsingRay(state);
            leftLineVisual.reticle.SetActive(state);
            LocomotionManager.instance.EnableTurning(!state);
        }
    }

    private void FindReticle()
    {
        if (FindObjectOfType<TeleportReticleRotate>() != null)
        {
            teleportReticle = FindObjectOfType<TeleportReticleRotate>().transform;
        }
    }
}
