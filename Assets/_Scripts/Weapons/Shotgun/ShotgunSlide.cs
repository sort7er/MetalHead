using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunSlide : MonoBehaviour
{
    public SoundForGun soundForGun;
    public Transform attachPoint;

    private XRSimpleInteractable simpleInteractable;
    private Vector3 startPos;
    private bool slideValid;

    private void Start()
    {
        startPos = transform.position;
        simpleInteractable = GetComponent<XRSimpleInteractable>();
    }

    public void SlideDone()
    {
        if (slideValid)
        {
            soundForGun.SlideBack();
        }
    }

    public void GrabSlide()
    {
        if (GameManager.instance.CheckGameObject(gameObject) == 1)
        {
            GameManager.instance.leftHand.GrabShotgunSlide(true);
            GameManager.instance.leftHand.NewParent(attachPoint, attachPoint);
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            GameManager.instance.rightHand.GrabShotgunSlide(true);
            GameManager.instance.rightHand.NewParent(attachPoint, attachPoint);
        }
    }
    public void ReleaseSlide()
    {
        if (!GameManager.instance.leftHand.IsHoldingSomething())
        {
            GameManager.instance.leftHand.OriginalParent();
            GameManager.instance.leftHand.GrabShotgunSlide(false);
        }
        if (!GameManager.instance.rightHand.IsHoldingSomething())
        {
            GameManager.instance.rightHand.OriginalParent();
            GameManager.instance.rightHand.GrabShotgunSlide(false);
        }
    }
}
