using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeleeWeapon : MonoBehaviour
{
    public int damage, stun;
    public float hitSensetivity, minTravelDistance;
    public Transform leftAttach, rightAttach;
    public Transform tip;

    private Vector3 handPos;
    private Vector3 tipThisFrame, tipLastFrame, distanceTraveled;
    private XRGrabInteractable xrGrabInteractable;
    private bool lethal, damageGiven, isHolding, left, savedPos;

    private void Start()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        CalculateMovement();
    }

    public void GrabMeleeWeapon()
    {
        if (GameManager.instance.CheckHand("Handle") == 1)
        {
            xrGrabInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabHandle(true);
            left = true;
        }
        if (GameManager.instance.CheckHand("Handle") == 2)
        {
            xrGrabInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabHandle(true);
            left = false;
        }
        isHolding = true;
    }
    public void ReleaseMeleeWeapon()
    {
        GameManager.instance.leftHand.GrabHandle(false);
        GameManager.instance.rightHand.GrabHandle(false);
        isHolding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageGiven && Vector3.Distance(handPos, tipThisFrame) >= minTravelDistance)
        {
            if (other.GetComponent<BodyPart>() != null && lethal)
            {
                other.GetComponent<BodyPart>().TakeDamage(damage, stun, other.transform.position - transform.position);

                if (other.transform.GetComponent<BodyPart>() != null && other.transform.GetComponent<BodyPart>().crit)
                {
                    EffectManager.instance.SpawnMeleeEffect(other.ClosestPointOnBounds(transform.position), 1, Quaternion.LookRotation(distanceTraveled, transform.up));
                }
                else if(other.transform.GetComponent<BodyPart>() != null)
                {
                    EffectManager.instance.SpawnMeleeEffect(other.ClosestPointOnBounds(transform.position), 0, Quaternion.LookRotation(distanceTraveled, transform.up));
                }
                damageGiven = true;
            }
        }
    }


    private void CalculateMovement()
    {
        if (isHolding)
        {
            if (left)
            {
                tipThisFrame = GameManager.instance.leftHand.transform.localPosition;
                distanceTraveled = (tipThisFrame - tipLastFrame) * 100;
                tipLastFrame = GameManager.instance.leftHand.transform.localPosition;
            }
            else
            {
                tipThisFrame = GameManager.instance.rightHand.transform.localPosition;
                distanceTraveled = (tipThisFrame - tipLastFrame) * 100;
                tipLastFrame = GameManager.instance.rightHand.transform.localPosition;
            }
        }
        else
        {
            tipThisFrame = tip.transform.position;
            distanceTraveled = (tipThisFrame - tipLastFrame) * 100;
            tipLastFrame = tip.transform.position;
        }


        if (distanceTraveled.magnitude > hitSensetivity)
        {
            lethal = true;
            if (!savedPos)
            {
                if(left)
                {
                    handPos = GameManager.instance.leftHand.transform.localPosition;
                }
                else
                {
                    handPos = GameManager.instance.rightHand.transform.localPosition;
                }
                savedPos = true;
            }
            
        }
        else
        {
            savedPos = false;
            lethal = false;
            damageGiven = false;
        }
    }
}
