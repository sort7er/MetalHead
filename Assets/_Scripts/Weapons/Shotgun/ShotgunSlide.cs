using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShotgunSlide : MonoBehaviour
{
    public SoundForGun soundForGun;
    public Transform attachPoint;
    public float threshold, sensitivity, limit;

    private XRSimpleInteractable simpleInteractable;
    private Tac14 tac14;
    private Vector3 startPos;
    private Transform leftHand, rightHand;
    private Vector3 distanceAtThreshold;
    private bool slideValid, follow, left;
    public float initialDistance, difference;
    private void Start()
    {
        tac14 = GetComponentInParent<Tac14>();
        startPos = transform.localPosition;
        simpleInteractable = GetComponent<XRSimpleInteractable>();
    }

    private void Update()
    {
        if (!tac14.auto && simpleInteractable.enabled)
        {
            rightHand = GameManager.instance.rHand.transform;
            leftHand = GameManager.instance.lHand.transform;


            if (left)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + GameManager.instance.leftHand.transform.localPosition.z - transform.localPosition.z);
            }
            //else
            //{
            //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - difference * sensitivity);
            //}


            //if (Vector3.Distance(leftHand.position, rightHand.position) < initialDistance - threshold)
            //{
            //    if (!follow)
            //    {
            //        follow = true;
            //        if (left)
            //        {
            //            distanceAtThreshold = GameManager.instance.lHand.transform.position;
            //        }
            //        else
            //        {
            //            distanceAtThreshold = GameManager.instance.rHand.transform.position;
            //        }
            //    }
            //}
            //else
            //{
            //    if (follow)
            //    {
            //        follow = false;
            //        transform.localPosition = startPos;
            //    }
            //}
        }
        
        //if (follow)
        //{
            

        //    if (left)
        //    {
        //        difference = (distanceAtThreshold - GameManager.instance.lHand.transform.position).magnitude;
        //        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - difference * sensitivity);
        //    }
        //    else
        //    {
        //        difference = (distanceAtThreshold - GameManager.instance.rHand.transform.position).magnitude;
        //        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - difference * sensitivity);
        //    }

        //    //if (transform.localPosition.z < limit)
        //    //{
        //    //    if (follow)
        //    //    {
        //    //        soundForGun.Slide();
        //    //    }
        //    //    follow = false;
        //    //    slideValid = true;
        //    //}

        //}

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
            left = true;
            GameManager.instance.leftHand.GrabShotgunSlide(true);
            GameManager.instance.leftHand.NewParent(attachPoint, attachPoint);
        }
        if (GameManager.instance.CheckGameObject(gameObject) == 2)
        {
            left = false;
            GameManager.instance.rightHand.GrabShotgunSlide(true);
            GameManager.instance.rightHand.NewParent(attachPoint, attachPoint);
        }

        initialDistance = Vector3.Distance(rightHand.position, leftHand.position);
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
