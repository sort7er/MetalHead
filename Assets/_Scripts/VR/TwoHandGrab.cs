using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TwoHandGrab : XRGrabInteractable
{
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAllreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isAllreadyGrabbed;
    }
}
