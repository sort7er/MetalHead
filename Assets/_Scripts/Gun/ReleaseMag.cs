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
    private SoundForGun soundForGun;
    private InputAction releaseMagLeft, releaseMagRight;
    private ReturnToHolster returnToHolster;
    private DynamicTrigger dynamicTrigger;
    private XRDirectInteractor rHand, lHand;
    public Transform mag, currentGameobject;
    private Rigidbody magRB;
    private Collider magCollider;
    private XRGrabInteractable magInteractable;
    private bool left;
    private bool release, insert;

    private void Start()
    {
        cz50 = GetComponent<CZ50>();
        soundForGun = GetComponent<SoundForGun>();
        returnToHolster = GetComponent<ReturnToHolster>();
        dynamicTrigger = magLocation.GetComponent<DynamicTrigger>();
        dynamicTrigger.TriggerEnabled(false);
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
        Debug.Log("lool");
        if (currentGameobject != null && currentGameobject.gameObject == dynamicTrigger.GetGameObject())
        {
            insert = true;
            soundForGun.Magazine(0);
            UpdateMag(currentGameobject);
            magInteractable.enabled = false;
            mag.parent = magLocation;
            mag.position = magEndPoint.position;
            mag.localRotation = Quaternion.identity;
            magRB.isKinematic = true;
            magRB.useGravity = false;
            magCollider.enabled = false;
            dynamicTrigger.TriggerEnabled(false);
        }

    }
    public void UpdateMag(Transform currentMag)
    {
        mag = currentMag;
        magRB = mag.GetComponent<Rigidbody>();
        magCollider = mag.GetComponent<Collider>();
        magInteractable = mag.GetComponent<XRGrabInteractable>();
    }
    private void ResetMag()
    {
        mag = null;
        magRB = null;
        magCollider = null;
    }

    private void StartRelease()
    {
        if (mag != null)
        {
            soundForGun.Magazine(2);
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
            magInteractable.enabled = true;
            Invoke("EnableTrigger", 0.25f);
            release = false;
            mag.parent = ParentManager.instance.mags;
            ResetMag();
        }
    }
    private void EnableTrigger()
    {
        dynamicTrigger.TriggerEnabled(true);
    }
    private void MagIn()
    {
        mag.position = Vector3.MoveTowards(mag.position, magLocation.position, Time.deltaTime * slideTime);
        if (Vector3.Distance(mag.position, magLocation.position) < 0.001f)
        {
            soundForGun.Magazine(1);
            insert = false;
            CanReload(true);
        }
    }
    
    public void CanReload(bool state)
    {
        reloadValid = state;
    }
    private void MagOutLeft(InputAction.CallbackContext context)
    {
        if (returnToHolster.isHolding && left)
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
}
