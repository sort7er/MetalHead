using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TwoHandGrab : XRGrabInteractable
{
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
}
