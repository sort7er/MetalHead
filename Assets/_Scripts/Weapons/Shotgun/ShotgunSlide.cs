using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunSlide : MonoBehaviour
{
    public SoundForGun soundForGun;
    public Transform attachPoint;
    public float threshold;

    [HideInInspector] public bool slideStarted;

    private ShotgunRecoil shotgunRecoil;
    private Transform casingPoint;
    private GameObject casingPrefab;
    private Tac14 tac14;
    private Animator slideAnim;
    private Transform leftHand, rightHand;
    private bool slideValid, isGrabbed, hasFired;
    private float initialDistance, closeUpDistance;
    private void Start()
    {
        shotgunRecoil = GetComponentInParent<ShotgunRecoil>();
        tac14 = GetComponentInParent<Tac14>();
        slideAnim = GetComponent<Animator>();
        casingPoint = tac14.casingPoint;
        casingPrefab = tac14.casingPrefab;
    }

    private void Update()
    {
        if (!tac14.auto && isGrabbed)
        {
            rightHand = GameManager.instance.rHand.transform;
            leftHand = GameManager.instance.lHand.transform;

            if (!slideStarted && Vector3.Distance(leftHand.position, rightHand.position) < initialDistance - threshold)
            {
                SlideBack();
            }
            else if(slideStarted && Vector3.Distance(leftHand.position, rightHand.position) > closeUpDistance + threshold * 0.5f)
            {
                SlideDone();
            }
        }
    }


    private void SlideBack()
    {
        slideValid = true;
        slideStarted = true;
        slideAnim.SetBool("Slide", true);
        soundForGun.Slide();
        closeUpDistance = Vector3.Distance(leftHand.position, rightHand.position);

    }

    public void CasingOut()
    {
        if (hasFired)
        {
            hasFired = false;
            GameObject casing = Instantiate(casingPrefab, casingPoint.position, casingPoint.rotation);
            casing.transform.parent = ParentManager.instance.bullets;
        }
    }


    public void SlideDone()
    {
        slideStarted = false;
        slideAnim.SetBool("Slide", false);

        if (slideValid)
        {
            soundForGun.SlideBack();
            tac14.CockingGun();
            slideValid = false;
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
        shotgunRecoil.SetRecoil(false);
        isGrabbed = true;
        rightHand = GameManager.instance.rHand.transform;
        leftHand = GameManager.instance.lHand.transform;
        initialDistance = Vector3.Distance(leftHand.position, rightHand.position);
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
        shotgunRecoil.SetRecoil(true);
        isGrabbed = false;
        SlideDone();
    }
    public void HasFired()
    {
        hasFired = true;
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }

}
