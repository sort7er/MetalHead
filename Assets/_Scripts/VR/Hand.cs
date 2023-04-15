using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : MonoBehaviour
{
    public GameObject handPrefab;
    public InputDeviceCharacteristics controllerCharacteristics;
    [Range(0, 1)]
    public float hoverHapticIntensity;
    public float hoverDuration;

    private XRDirectInteractor interactor;
    private GameObject spawnedHand;
    private InputDevice targetDevice;
    private Animator handAnim;
    private Vector3 originalPostion;
    private Quaternion originalRotation;

    private void Start()
    {
        interactor = GetComponent<XRDirectInteractor>();
        InitializeHand();
    }

    private void Update()
    {
        if(!targetDevice.isValid)
        {
            InitializeHand();
        }
        else
        {
            UpdateHand();
        }
    }

    private void InitializeHand()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if(devices.Count > 0 )
        {
            targetDevice = devices[0];

            spawnedHand = Instantiate(handPrefab, transform);
            originalPostion = spawnedHand.transform.localPosition;
            originalRotation = spawnedHand.transform.localRotation;
            handAnim = spawnedHand.GetComponent<Animator>();
        }
    }

    private void UpdateHand()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnim.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnim.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnim.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnim.SetFloat("Grip", 0);
        }
    }

    public void UsingRay(bool state)
    {
        if(handAnim!= null)
        {
            handAnim.SetBool("UsingRay", state);
        }
    }
    public void GrabPistol(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabPistol", state);
        }
    }
    public void GrabSlide(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabSlide", state);
        }
    }
    public void GrabSlideBack(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabSlideBack", state);
        }
    }
    public void GrabMag(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabMag", state);
        }
    }
    public void GrabHandle(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabHandle", state);
        }
    }
    public void GrabWrench(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabWrench", state);
        }
    }

    public void Hover()
    {
        List<XRBaseInteractable> hoveredObjects = new List<XRBaseInteractable>();
        interactor.GetHoverTargets(hoveredObjects);

        foreach(var interactable in hoveredObjects)
        {
            if(interactable.GetComponent<XRGrabInteractable>() != null || interactable.GetComponent<XRSimpleInteractable>() != null)
            {
                if(!interactable.isSelected)
                {
                    if (handAnim != null)
                    {
                        interactor.SendHapticImpulse(hoverHapticIntensity, hoverDuration);
                        handAnim.SetBool("Hover", true);
                    }
                }
            }
        }

        
    }
    public void HoverDone()
    {
        if (handAnim != null)
        {
            handAnim.SetBool("Hover", false);
        }
    }

    public void NewParent(Transform newParent, Transform attachTransform)
    {
        spawnedHand.transform.parent = newParent;
        spawnedHand.transform.position= attachTransform.position;
        spawnedHand.transform.rotation= attachTransform.rotation;
    }
    public void OriginalParent()
    {
        spawnedHand.transform.parent = transform;
        spawnedHand.transform.localPosition = originalPostion;
        spawnedHand.transform.localRotation = originalRotation;
    }
}
