using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Slide : MonoBehaviour
{
    public string tagToCompare;
    public float limit;
    public float sensitivity;
    public CZ50 cz50;
    public ReleaseMag releaseMag;
    public SoundForGun soundForGun;

    private XRDirectInteractor rHand, lHand;
    private float difference;
    private Vector3 oldPos, newPos;
    private bool followLeftHand, followRightHand, follow, slideValid;



    private void Start()
    {
        lHand = GameManager.instance.leftHand.gameObject.GetComponent<XRDirectInteractor>();
        rHand = GameManager.instance.rightHand.gameObject.GetComponent<XRDirectInteractor>();
    }
    private void Update()
    {
        if (follow)
        {
            if (followRightHand)
            {
                newPos = GameManager.instance.rightHand.transform.localPosition;
                difference = (newPos - oldPos).magnitude;
                oldPos = GameManager.instance.rightHand.transform.localPosition;

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - difference * sensitivity);
            }
            else if (followLeftHand)
            {
                newPos = GameManager.instance.leftHand.transform.localPosition;
                difference = (newPos - oldPos).magnitude;
                oldPos = GameManager.instance.leftHand.transform.localPosition;

                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - difference * sensitivity);
            }
        }


        if(transform.localPosition.z < limit)
        {
            if (follow)
            {
                soundForGun.Slide(0);
            }
            follow = false;
            slideValid = true;
        }
    }

    public void Follow()
    {
        follow = true;
        slideValid = false;
        if (rHand.selectTarget != null && rHand.selectTarget.CompareTag(tagToCompare))
        {
            newPos = GameManager.instance.rightHand.transform.localPosition;
            oldPos = GameManager.instance.rightHand.transform.localPosition;
            followRightHand = true;
        }
        else if (lHand.selectTarget != null && lHand.selectTarget.CompareTag(tagToCompare))
        {
            newPos = GameManager.instance.leftHand.transform.localPosition;
            oldPos = GameManager.instance.leftHand.transform.localPosition;
            followLeftHand = true;
        }
    }
    public void DontFollow()
    {
        followRightHand = false;
        followLeftHand = false;
        follow= false;

    }
    public void SlideDone()
    {
        soundForGun.Slide(1);
        if(slideValid && releaseMag.reloadValid)
        {
            cz50.Reload();
            releaseMag.CanReload(false);
        }
    }
}
