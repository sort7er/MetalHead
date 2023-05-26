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
    public float vertexCount = 12;

    [HideInInspector] public Hover currentHover;

    private Hand leftHand, rightHand;
    private XRDirectInteractor interactor;
    private GameObject spawnedHand;
    private InputDevice targetDevice;
    private Animator handAnim;
    private Vector3 originalPostion;
    private Quaternion originalRotation;
    private LineRenderer lineRenderer;
    private bool lineActive;
    private Transform lineEndPoint;

    private void Start()
    {
        leftHand = GameManager.instance.leftHand.GetComponent<Hand>();
        rightHand = GameManager.instance.rightHand.GetComponent<Hand>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
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
            if(lineActive)
            {
                Vector3 middlePoint = transform.position + transform.forward * Vector3.Distance(transform.position, lineEndPoint.position) * 0.5f;

                var pointList = new List<Vector3>();

                for (float ratio = 0; ratio <= 1; ratio += 1/ vertexCount)
                {
                    var tanget1 = Vector3.Lerp(transform.position, middlePoint, ratio);
                    var tanget2 = Vector3.Lerp(middlePoint, lineEndPoint.position, ratio);
                    var curve = Vector3.Lerp(tanget1, tanget2, ratio);

                    pointList.Add(curve);
                }

                lineRenderer.positionCount= pointList.Count;
                lineRenderer.SetPositions(pointList.ToArray());
            }

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
    public void GrabShotgun(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabShotgun", state);
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
    public void GrabShotgunSlide(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabShotgunSlide", state);
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
    public void GrabSlug(bool state)
    {
        if (handAnim != null)
        {
            handAnim.SetBool("GrabSlug", state);
        }
    }
    public void CheckHover()
    {
        List<XRBaseInteractable> hoveredObjects = new List<XRBaseInteractable>();
        interactor.GetHoverTargets(hoveredObjects);

        foreach (var interactable in hoveredObjects)
        {
            if (!interactable.GetComponent<TeleportationArea>())
            {
                if(interactable.isSelected)
                {
                    handAnim.SetBool("Hover", false);
                    lineRenderer.enabled = false;
                    lineActive = false;
                    lineEndPoint = null;
                    if (currentHover != null)
                    {
                        currentHover.Hovering(false);
                    }
                    currentHover = null;
                }
            }
        }
    }
    public void Grab()
    {
        if (!interactor.selectTarget.GetComponent<TeleportationArea>())
        {
            interactor.SendHapticImpulse(hoverHapticIntensity * 2, hoverDuration * 2);
            GameManager.instance.rightHand.CheckHover();
            GameManager.instance.leftHand.CheckHover();
        }
    }
    public void Hover()
    {
        List<XRBaseInteractable> hoveredObjects = new List<XRBaseInteractable>();
        interactor.GetHoverTargets(hoveredObjects);

        XRBaseInteractable closestInteractable = null;
        float minDist = Mathf.Infinity;

        foreach(var interactable in hoveredObjects)
        {
            if(interactable.GetComponent<XRGrabInteractable>() != null || interactable.GetComponent<XRSimpleInteractable>() != null)
            {
                if (!interactable.isSelected)
                {
                    float dist = Vector3.Distance(interactable.transform.position, transform.position);
                    if (dist < minDist)
                    {
                        closestInteractable = interactable;
                        minDist = dist;
                    }
                }
            }
        }
        

        if (closestInteractable != null)
        {
            if (handAnim != null)
            {
                interactor.SendHapticImpulse(hoverHapticIntensity, hoverDuration);
                handAnim.SetBool("Hover", true);

                if(closestInteractable.GetComponent<XRSimpleInteractable>() == null)
                {
                    lineRenderer.enabled = true;
                    lineActive = true;
                    lineEndPoint = closestInteractable.transform;
                    if (currentHover != null)
                    {
                        currentHover.Hovering(false);
                    }
                    if (closestInteractable.GetComponent<Hover>() != null)
                    {
                        closestInteractable.GetComponent<Hover>().Hovering(true);
                        currentHover = closestInteractable.GetComponent<Hover>();
                    }
                }
                
            }
        }


    }
    public void HoverDone()
    {
        if (handAnim != null)
        {
            lineRenderer.enabled = false;
            lineActive = false;
            lineEndPoint = null;
            handAnim.SetBool("Hover", false);
            if (currentHover != null)
            {
               if(leftHand.currentHover != null && rightHand.currentHover != null && leftHand.currentHover == rightHand.currentHover)
               {
                    currentHover.Hovering(true);
                }
               else
               {
                    currentHover.Hovering(false);
               }
            }
            currentHover= null;
        }
        Hover();
    }

    public void NewParent(Transform newParent, Transform attachTransform)
    {
        spawnedHand.transform.position= attachTransform.position;
        spawnedHand.transform.rotation= attachTransform.rotation;
        spawnedHand.transform.parent = newParent;
    }
    public void OriginalParent()
    {
        spawnedHand.transform.parent = transform;
        spawnedHand.transform.localPosition = originalPostion;
        spawnedHand.transform.localRotation = originalRotation;
    }
    public bool IsHoldingSomething()
    {
        if (interactor.selectTarget != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SendPulse(float intesity, float duration)
    {
        interactor.SendHapticImpulse(intesity, duration);
        Debug.Log("Beep");
    }

}
