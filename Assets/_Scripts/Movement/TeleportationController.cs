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

    private XRInteractorLineVisual leftLineVisual;
    private InputAction thumbstickInputAction;
    private InputAction teleportActivate;
    private InputAction teleportCancel;
    private Transform teleportReticle;

    private void Start()
    {
        rayInteractor.enabled = false;
        leftLineVisual = rayInteractor.GetComponent<XRInteractorLineVisual>();

        teleportActivate = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        teleportActivate.Enable();
        teleportActivate.performed += OnTeleportActivate;

        teleportCancel = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        teleportCancel.Enable();
        teleportCancel.performed += OnTeleportCancel;

        thumbstickInputAction = inputAction.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        thumbstickInputAction.Enable();
    }
    private void OnDestroy()
    {
        teleportActivate.performed-= OnTeleportActivate;
        teleportCancel.performed -= OnTeleportCancel;

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
    private void SetRay(bool state)
    {
        rayInteractor.enabled = state;
        teleportActive = state;
        leftHand.UsingRay(state);
        leftLineVisual.reticle.SetActive(state);
        GameManager.instance.EnableSnap(!state);
    }

    private void FindReticle()
    {
        if (FindObjectOfType<TeleportReticleRotate>() != null)
        {
            teleportReticle = FindObjectOfType<TeleportReticleRotate>().transform;
        }
    }
}
