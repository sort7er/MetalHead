using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabObject : MonoBehaviour
{
    public Transform leftAttach, rightAttach;
    public UnityEvent onGrabLeft, onGrabRight, onRelease;

    private XRGrabInteractable xRGrabInteractable;

    private void Start()
    {
        xRGrabInteractable = GetComponent<XRGrabInteractable>();
    }


    public void Grab()
    {
        if (GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            xRGrabInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabWrench(true);
            onGrabLeft.Invoke();
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            onGrabRight.Invoke();
            xRGrabInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabWrench(true);
        }
    }

    public void Release()
    {
        onRelease.Invoke();
        GameManager.instance.leftHand.GrabWrench(false);
        GameManager.instance.rightHand.GrabWrench(false);
    }
}
