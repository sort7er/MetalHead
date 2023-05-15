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
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            slugInteractable.attachTransform = rightAttach;
        }
    }
    public void ReleaseSlug()
    {
        transform.parent = ParentManager.instance.mags;
        EnableGravity(true);
        GameManager.instance.ammoBagShotgun.ReleasingSlug();
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
