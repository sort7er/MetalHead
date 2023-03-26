using UnityEngine;
using UnityEngine.XR;

public class Weapon : MonoBehaviour
{
    public Transform[] pointsOfDamage;
    public float[] raduis;
    public int[] damage;
    public float parrySensetivity = 5f;

    [HideInInspector] public bool isParrying;

    private Vector3 leftThisFrame, leftLastFrame, leftMovementDirection;
    private Vector3 rightThisFrame, rightLastFrame, rightMovementDirection;
    private Collider playerCollider;
    private PlayerHealth playerHealth;
    private int numberToCheck;
    private bool lethal, damageGiven, canParry, leftParry, rightParry;
    public float test;

    private void Start()
    {
        playerCollider = GameManager.instance.XROrigin.GetComponent<Collider>();
        playerHealth = GameManager.instance.XROrigin.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (lethal && (playerCollider.ClosestPointOnBounds(pointsOfDamage[numberToCheck].position) - pointsOfDamage[numberToCheck].position).magnitude <= raduis[numberToCheck] && !damageGiven)
        {
            damageGiven = true;
            EffectManager.instance.SpawnHitPlayerEffect(playerCollider.ClosestPointOnBounds(pointsOfDamage[numberToCheck].position));
            playerHealth.TakeDamage(damage[numberToCheck]);
        }
        CalculateLeftMovement();
        CalculateRightMovement();


        if ((leftParry || rightParry) && canParry && !isParrying)
        {
            Vector3 positionToSpawn;
            if (leftParry)
            {
                positionToSpawn = GameManager.instance.leftHand.transform.position + (pointsOfDamage[numberToCheck].position - GameManager.instance.leftHand.transform.position) * 0.2f;
            }
            else
            {
                positionToSpawn = GameManager.instance.rightHand.transform.position + (pointsOfDamage[numberToCheck].position - GameManager.instance.rightHand.transform.position) * 0.2f;
            }
            EffectManager.instance.SpawnParryEffect(positionToSpawn);
            CannotParry();
            NotLethal();
            isParrying = true;
        }
    }

    public void IsLethal(int whichAttack)
    {
        numberToCheck = whichAttack - 1;
        lethal = true;
    }
    public void NotLethal()
    {
        numberToCheck = 0;
        lethal = false;
        damageGiven = false;
    }

    public void CanParry()
    {
        canParry = true;
    }
    public void CannotParry()
    {
        canParry = false;
    }
    public void ParryingDone()
    {
        isParrying = false;
    }
    public void CalculateLeftMovement()
    {
        leftThisFrame = GameManager.instance.leftHand.transform.localPosition;
        leftMovementDirection = (leftThisFrame - leftLastFrame) * 100;
        leftLastFrame = GameManager.instance.leftHand.transform.localPosition;
        if(leftMovementDirection.magnitude > parrySensetivity)
        {
            leftParry = true;
        }
        else
        {
            leftParry = false;
        }
    }
    public void CalculateRightMovement()
    {
        rightThisFrame = GameManager.instance.rightHand.transform.localPosition;
        rightMovementDirection = (rightThisFrame - rightLastFrame) * 100;
        rightLastFrame = GameManager.instance.rightHand.transform.localPosition;
        if (rightMovementDirection.magnitude > parrySensetivity)
        {
            rightParry = true;
        }
        else
        {
            rightParry = false;
        }
    }
}
