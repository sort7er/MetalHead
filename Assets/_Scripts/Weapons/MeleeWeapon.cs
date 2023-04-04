using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeleeWeapon : MonoBehaviour
{
    public int damage, stun;
    public float hitSensetivity;
    public Transform leftAttach, rightAttach;

    private Vector3 leftThisFrame, leftLastFrame, leftMovementDirection;
    private Vector3 rightThisFrame, rightLastFrame, rightMovementDirection;
    private XRGrabInteractable xrGrabInteractable;
    private bool isHolding, left, leftHit, rightHit, damageGiven;

    private void Start()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        if (isHolding)
        {
            if (left)
            {
                CalculateLeftMovement();
                if(leftMovementDirection.magnitude <= 2)
                {
                    damageGiven = false;
                }
            }
            else
            {
                CalculateRightMovement();
                if (rightMovementDirection.magnitude <= 2)
                {
                    damageGiven = false;
                }
            }
        }
    }

    public void GrabMeleeWeapon()
    {
        if (GameManager.instance.CheckHand("Handle") == 1)
        {
            left = true;
            xrGrabInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabHandle(true);
        }
        if (GameManager.instance.CheckHand("Handle") == 2)
        {
            left = false;
            xrGrabInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabHandle(true);
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
        if(other.GetComponent<BodyPart>() != null)
        {
            if (!damageGiven)
            {
                if (left && leftHit)
                {
                    other.GetComponent<BodyPart>().TakeDamage(damage, stun, other.transform.position - transform.position);
                    EffectManager.instance.SpawnBarrelHitEnemy(other.ClosestPointOnBounds(transform.position));
                }
                else if (!left && rightHit)
                {
                    other.GetComponent<BodyPart>().TakeDamage(damage, stun, other.transform.position - transform.position);
                    EffectManager.instance.SpawnBarrelHitEnemy(other.ClosestPointOnBounds(transform.position));
                }
                damageGiven = true;
            }
        }
    }


    public void CalculateLeftMovement()
    {
        leftThisFrame = GameManager.instance.leftHand.transform.localPosition;
        leftMovementDirection = (leftThisFrame - leftLastFrame) * 100;
        leftLastFrame = GameManager.instance.leftHand.transform.localPosition;
        if (leftMovementDirection.magnitude > hitSensetivity)
        {
            leftHit = true;
        }
        else
        {
            leftHit = false;
        }
    }
    public void CalculateRightMovement()
    {
        rightThisFrame = GameManager.instance.rightHand.transform.localPosition;
        rightMovementDirection = (rightThisFrame - rightLastFrame) * 100;
        rightLastFrame = GameManager.instance.rightHand.transform.localPosition;
        if (rightMovementDirection.magnitude > hitSensetivity)
        {
            rightHit = true;
        }
        else
        {
            rightHit = false;
        }
    }
}
