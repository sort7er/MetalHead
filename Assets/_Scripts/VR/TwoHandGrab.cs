using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TwoHandGrab : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    public enum TwoHandRotationType { none, firstHand, secondHand };
    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;


    private XRBaseInteractor secondInteractor;
    private Quaternion interactorInitialRotation;
    private Quaternion interactorInitialRotation2;
    private Quaternion attachInitialRotation;
    private Quaternion attachInitialRotation2;
    private Quaternion initialRotationOffset;

    private void Start()
    {
        foreach(var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }

    }

    private Quaternion GetTwoHandRotationType()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.none)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.firstHand)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }
        
        return targetRotation;

    }
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {

            // Computing the rotation

            if(snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotationType();
            }
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotationType() * initialRotationOffset;
            }

        }
        base.ProcessInteractable(updatePhase);
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
      
        if (!interactor.CompareTag("Holster"))
        {
            bool isAllreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
            return base.IsSelectableBy(interactor) && !isAllreadyGrabbed;
        }
        else
        {
            return false;
        }
        
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        secondInteractor = interactor;
        interactorInitialRotation2 = interactor.transform.localRotation;
        attachInitialRotation2 = interactor.attachTransform.localRotation;
    }
    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        secondInteractor = null;
        selectingInteractor.attachTransform.localRotation = attachInitialRotation;
        //initialRotationOffset = Quaternion.Inverse(GetTwoHandRotationType()) * selectingInteractor.attachTransform.rotation;
        interactor.transform.localRotation = interactorInitialRotation2;
        interactor.attachTransform.localRotation = attachInitialRotation2;
    }
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        interactorInitialRotation = interactor.transform.localRotation;
        attachInitialRotation = interactor.attachTransform.localRotation;
    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.transform.localRotation = interactorInitialRotation;
        interactor.attachTransform.localRotation = attachInitialRotation;
        interactor.transform.localRotation = interactorInitialRotation2;
        interactor.attachTransform.localRotation = attachInitialRotation2;

    }




}
