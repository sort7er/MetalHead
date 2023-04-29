using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ReleaseMag : MonoBehaviour
{
    public float slideTime;
    public InputActionAsset releaseMagInputAction;
    public Transform magLocation, magEndPoint;
    public GameObject magPrefab;

    [HideInInspector] public bool reloadValid;
    [HideInInspector] public bool canReload;
    [HideInInspector] public bool release;
    [HideInInspector] public bool insert;

    private CZ50 cz50;
    private SoundForGun soundForGun;
    private InputAction releaseMagLeft, releaseMagRight;
    private ReturnToHolster returnToHolster;
    private DynamicTrigger dynamicTrigger;
    private XRDirectInteractor rHand, lHand;
    private Transform mag, currentGameobject;
    private Collider magCollider, magSphereCollider;
    private XRGrabInteractable magInteractable;
    private Mag magInGun;
    private bool left;

    private void Start()
    {

        if(FindObjectOfType<TutorialManager>() != null)
        {
            TutorialCanReload(false);
        }
        else
        {
            TutorialCanReload(true);
        }

        cz50 = GetComponent<CZ50>();
        soundForGun = GetComponent<SoundForGun>();
        returnToHolster = GetComponent<ReturnToHolster>();
        dynamicTrigger = magLocation.GetComponent<DynamicTrigger>();
        dynamicTrigger.TriggerDisabled(true);
        lHand = GameManager.instance.leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = GameManager.instance.rightHand.gameObject.GetComponent<XRDirectInteractor>();
        GameObject newMag = Instantiate(magPrefab, magLocation.position, magLocation.rotation);
        UpdateMag(newMag.transform);
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
            if (GameManager.instance.CheckHand("Gun") == 1)
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
            else if (GameManager.instance.CheckHand("Gun") == 2)
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
        if (currentGameobject != null && currentGameobject.gameObject == dynamicTrigger.GetGameObject())
        {
            insert = true;
            soundForGun.Magazine(0);
            UpdateMag(currentGameobject);
            mag.position = magEndPoint.position;
            mag.localRotation = Quaternion.identity;
            magInGun.EnableGravity(false);
            magCollider.enabled = false;
            magSphereCollider.enabled = false;
            dynamicTrigger.TriggerDisabled(true);
        }

    }
    public void UpdateMag(Transform currentMag)
    {
        mag = currentMag;
        magCollider = mag.GetComponent<Collider>();
        magSphereCollider = mag.GetComponent<SphereCollider>();
        magInteractable = mag.GetComponent<XRGrabInteractable>();
        magInGun = mag.GetComponent<Mag>();
        magInteractable.enabled = false;
        mag.parent = magLocation;
        cz50.MagIn(magInGun);
    }
    private void ResetMag()
    {
        mag = null;
        magCollider = null;
        magSphereCollider = null;
    }

    private void StartRelease()
    {
        if (mag != null && !insert)
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
            magCollider.enabled = true;
            magSphereCollider.enabled = true;
            magInteractable.enabled = true;
            Invoke("EnableTrigger", 0.25f);
            release = false;
            mag.parent = ParentManager.instance.mags;
            magInGun.EnableGravity(true);
            magInGun.CheckForDestroyMag();
            ResetMag();
            
        }
    }
    private void EnableTrigger()
    {
        dynamicTrigger.TriggerDisabled(false);
    }
    private void MagIn()
    {
        if (mag != null)
        {
            mag.position = Vector3.MoveTowards(mag.position, magLocation.position, Time.deltaTime * slideTime);
            if (Vector3.Distance(mag.position, magLocation.position) < 0.001f)
            {
                soundForGun.Magazine(1);
                insert = false;
                CanReload(true);
            }
        }
    }
    
    public void CanReload(bool state)
    {
        reloadValid = state;
    }
    private void MagOutLeft(InputAction.CallbackContext context)
    {
        if (returnToHolster.isHolding && left && canReload)
        {
            StartRelease();
        }
    }
    private void MagOutRight(InputAction.CallbackContext context)
    {
        if (returnToHolster.isHolding && !left && canReload)
        {
            StartRelease();
        }
    }

    public void TutorialCanReload(bool state)
    {
        canReload = state;
    }
}
