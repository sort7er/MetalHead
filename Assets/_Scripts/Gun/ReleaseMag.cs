using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ReleaseMag : MonoBehaviour
{
    public float slideTime;
    public InputActionAsset releaseMagInputAction;
    public Transform magLocation, magEndPoint;

    [HideInInspector] public bool reloadValid;

    private CZ50 cz50;
    private InputAction releaseMagLeft, releaseMagRight;
    private ReturnToHolster returnToHolster;
    private XRSocketInteractor magLocationSocketInteractor;
    private XRDirectInteractor rHand, lHand;
    private Transform mag, currentGameobject;
    private Rigidbody magRB;
    private Collider magCollider;
    private bool left;
    private bool release, insert;

    private void Start()
    {
        cz50 = GetComponent<CZ50>();
        returnToHolster = GetComponent<ReturnToHolster>();
        magLocationSocketInteractor = magLocation.GetComponent<XRSocketInteractor>();
        lHand = GameManager.instance.leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = GameManager.instance.rightHand.gameObject.GetComponent<XRDirectInteractor>();
        UpdateMag(magLocation.GetChild(0));
    }
    private void OnEnable()
    {
        releaseMagLeft = releaseMagInputAction.FindActionMap("XRI LeftHand Interaction").FindAction("Secondary Button");
        releaseMagLeft.Enable();
        releaseMagLeft.performed += MagOutLeft;

        releaseMagRight = releaseMagInputAction.FindActionMap("XRI RightHand Interaction").FindAction("Secondary Button");
        releaseMagRight.Enable();
        releaseMagRight.performed += MagOutRight;
    }

    private void OnDisable()
    {
        releaseMagLeft.performed -= MagOutLeft;
        releaseMagRight.performed -= MagOutRight;
    }
    private void Update()
    {
        if(returnToHolster.isHolding)
        {
            if (GameManager.instance.CheckHand("Pistol") == 1)
            {
                left = true;
                if (rHand.GetOldestInteractableSelected() != null)
                {
                    currentGameobject = rHand.GetOldestInteractableSelected().transform;
                }
                else
                {
                    currentGameobject = null;
                }
            }
            else if (GameManager.instance.CheckHand("Pistol") == 2)
            {
                left = false;
                if (lHand.GetOldestInteractableSelected() != null)
                {
                    currentGameobject = lHand.GetOldestInteractableSelected().transform;
                }
                else
                {
                    currentGameobject = null;
                }
            }
        }

        if (release)
        {
            MagOut();
        }
        if (insert)
        {
            MagIn();
        }

    }
    public void Insert()
    {
        insert = true;
        currentGameobject.GetComponent<XRGrabInteractable>().enabled = false;
        currentGameobject.parent = magLocation;
        UpdateMag(currentGameobject);
        mag.position = magEndPoint.position;
        mag.localRotation = Quaternion.identity;
        magRB.isKinematic = true;
        magRB.useGravity = false;
        magCollider.enabled = false;
    }
    public void UpdateMag(Transform currentMag)
    {
        mag = currentMag;
        magRB = mag.GetComponent<Rigidbody>();
        magCollider = mag.GetComponent<Collider>();
        magLocationSocketInteractor.enabled = false;
    }
    private void ResetMag()
    {
        mag = null;
        magRB = null;
        magCollider = null;
        magLocationSocketInteractor.enabled = true;
    }
    private void MagOutLeft(InputAction.CallbackContext context)
    {
        if(returnToHolster.isHolding && left)
        {
            StartRelease();
        }
    }
    private void MagOutRight(InputAction.CallbackContext context)
    {
        if (returnToHolster.isHolding && !left)
        {
            StartRelease();
        }
    }
    private void StartRelease()
    {
        if (mag != null)
        {
            cz50.MagOut();
            release = true;
        }
    }
    private void MagOut()
    {
        mag.position = Vector3.MoveTowards(mag.position, magEndPoint.position, Time.deltaTime * slideTime);
        if (Vector3.Distance(mag.position, magEndPoint.position) < 0.001f)
        {
            magRB.isKinematic = false;
            magRB.useGravity = true;
            magCollider.enabled = true;
            release = false;
            mag.parent = ParentManager.instance.mags;
            ResetMag();
        }
    }
    private void MagIn()
    {
        mag.position = Vector3.MoveTowards(mag.position, magLocation.position, Time.deltaTime * slideTime);
        if (Vector3.Distance(mag.position, magLocation.position) < 0.001f)
        {
            insert = false;
            CanReload(true);
        }
    }
    
    public void CanReload(bool state)
    {
        reloadValid = state;
    }

}
