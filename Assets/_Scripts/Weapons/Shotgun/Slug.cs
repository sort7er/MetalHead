using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Slug : MonoBehaviour
{
    public Transform leftAttach, rightAttach;

    private XRGrabInteractable slugInteractable;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        EnableGravity(false);
        slugInteractable = GetComponent<XRGrabInteractable>();
    }

    public void GrabSlug()
    {
        if (GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            slugInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabSlug(true);
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            GameManager.instance.rightHand.GrabSlug(true);
            slugInteractable.attachTransform = rightAttach;
        }
    }
    public void ReleaseSlug()
    {
        transform.parent = ParentManager.instance.mags;
        EnableGravity(true);
        GameManager.instance.ammoBagShotgun.ReleasingSlug();
        GameManager.instance.rightHand.GrabSlug(false);
        GameManager.instance.leftHand.GrabSlug(false);
    }

    public void EnableGravity(bool state)
    {
        if (rb != null)
        {
            rb.useGravity = state;
            rb.isKinematic = !state;
        }
    }
}
