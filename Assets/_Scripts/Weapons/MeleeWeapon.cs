using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeleeWeapon : MonoBehaviour
{
    public int damage, stun;
    public float hitSensetivity;
    public float refreshRate;
    public Transform leftAttach, rightAttach;
    public Transform tip;

    private MeshRenderer mesh;
    private Vector3 tipThisFrame, tipLastFrame, distanceTraveled;
    private XRGrabInteractable xrGrabInteractable;
    private bool waitTime, lethal, damageGiven;

    private void Start()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void FixedUpdate()
    {
        if (!waitTime)
        {
            CalculateMovement();
        }
    }

    public void GrabMeleeWeapon()
    {
        if (GameManager.instance.CheckHand("Handle") == 1)
        {
            xrGrabInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabHandle(true);
        }
        if (GameManager.instance.CheckHand("Handle") == 2)
        {
            xrGrabInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabHandle(true);
        } 
    }
    public void ReleaseMeleeWeapon()
    {
        GameManager.instance.leftHand.GrabHandle(false);
        GameManager.instance.rightHand.GrabHandle(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageGiven)
        {
            if (other.GetComponent<BodyPart>() != null && lethal)
            {
                other.GetComponent<BodyPart>().TakeDamage(damage, stun, other.transform.position - transform.position);

                if (other.transform.GetComponent<BodyPart>() != null && other.transform.GetComponent<BodyPart>().crit)
                {
                    EffectManager.instance.SpawnMeleeEffect(other.ClosestPointOnBounds(transform.position), 1);
                }
                else if(other.transform.GetComponent<BodyPart>() != null)
                {
                    EffectManager.instance.SpawnMeleeEffect(other.ClosestPointOnBounds(transform.position), 0);
                }

                damageGiven = true;
            }
        }
    }


    private void CalculateMovement()
    {
        waitTime = true;
        Invoke(nameof(CalculateAgain), refreshRate);


        tipThisFrame = tip.transform.position - GameManager.instance.XROrigin.transform.position;
        distanceTraveled = (tipThisFrame - tipLastFrame) * 100;
        tipLastFrame = tip.transform.position - GameManager.instance.XROrigin.transform.position;

        if (distanceTraveled.magnitude > hitSensetivity)
        {
            lethal = true;
        }
        else
        {
            lethal = false;
            damageGiven = false;
        }

    }
    private void CalculateAgain()
    {
        waitTime = false;
    }
}
