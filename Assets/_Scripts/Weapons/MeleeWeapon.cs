using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeleeWeapon : MonoBehaviour
{
    public int damage, stun;
    public float hitSensetivity, minTravelDistance;
    public Transform leftAttach, rightAttach;
    public Transform tip;
    [Range(0, 1)]
    public float hapticIntensity;
    public float duration;

    private Vector3 handPos;
    private Vector3 tipThisFrame, tipLastFrame, distanceTraveled;
    private XRGrabInteractable xrGrabInteractable;
    private XRDirectInteractor leftInteractor, rightInteractor;
    private bool lethal, canPlaySound, damageGiven, effectSpawned, isHolding, left, savedPos;

    private void Start()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        leftInteractor = GameManager.instance.leftHand.GetComponent<XRDirectInteractor>();
        rightInteractor = GameManager.instance.rightHand.GetComponent<XRDirectInteractor>();
    }

    private void Update()
    {
        CalculateMovement();
    }

    public void GrabMeleeWeapon()
    {
        if (GameManager.instance.CheckHand("Wrench") == 1)
        {
            xrGrabInteractable.attachTransform = leftAttach;
            GameManager.instance.leftHand.GrabWrench(true);
            left = true;
        }
        if (GameManager.instance.CheckHand("Wrench") == 2)
        {
            xrGrabInteractable.attachTransform = rightAttach;
            GameManager.instance.rightHand.GrabWrench(true);
            left = false;
        }
        isHolding = true;
    }
    public void ReleaseMeleeWeapon()
    {
        GameManager.instance.leftHand.GrabWrench(false);
        GameManager.instance.rightHand.GrabWrench(false);
        isHolding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageGiven && Vector3.Distance(handPos, tipThisFrame) >= minTravelDistance)
        {
            if (other.GetComponent<BodyPart>() != null && lethal)
            {
                other.GetComponent<BodyPart>().TakeDamage(damage, stun, other.transform.position - transform.position);
                other.GetComponent<BodyPart>().enemyHealth.GetComponent<RunningEnemy>().KnockBack();

                if (left)
                {
                    leftInteractor.SendHapticImpulse(hapticIntensity, duration);
                }
                else
                {
                    rightInteractor.SendHapticImpulse(hapticIntensity, duration);
                }


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
            else if(other.GetComponent<BodyPart>() != null && !effectSpawned && !lethal && canPlaySound)
            {
                EffectManager.instance.SpawnMeleeEffect(other.ClosestPointOnBounds(transform.position), 2, Quaternion.identity);
                effectSpawned = true;
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

        if (distanceTraveled.magnitude > hitSensetivity * 0.4f)
        {
            canPlaySound = true;
        }
        else
        {
            if(isHolding)
            {
                effectSpawned = false;
                canPlaySound = false;
            }
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
            if (isHolding)
            {
                damageGiven = false;

            }
        }

    }
}
