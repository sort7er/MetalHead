using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Weapon : MonoBehaviour
{
    public Transform[] pointsOfDamage;
    public float[] raduis;
    public int[] damage;
    public float parrySensetivity = 5f;
    public Transform canvasPos;
    public Image[] shieldImages;
    public Sprite[] shieldSprites;

    [HideInInspector] public bool isParrying;
    [HideInInspector] public int numberToCheck;

    private Vector3 leftThisFrame, leftLastFrame, leftMovementDirection;
    private Vector3 rightThisFrame, rightLastFrame, rightMovementDirection;
    private Collider playerCollider;
    private PlayerHealth playerHealth;

    private bool lethal, damageGiven, canParry, leftParry, rightParry, parryFailed, errorMargin;

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

        if(errorMargin && (leftParry || rightParry) && !parryFailed)
        {
            parryFailed = true;
            shieldImages[0].sprite = shieldSprites[2];
            shieldImages[1].gameObject.SetActive(false);
            EffectManager.instance.SpawnParrySoundEffect(transform.position, 1);
        }

        if ((leftParry || rightParry) && canParry && !isParrying && !parryFailed && Vector3.Distance(GameManager.instance.XROrigin.transform.position, transform.position) < 2)
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
            EffectManager.instance.SpawnParryEffectUI(canvasPos.position);
            isParrying = true;
            NotLethal();
            CannotParry();
        }
    }

    private void FixedUpdate()
    {
        CalculateLeftMovement();
        CalculateRightMovement();
    }

    public void IsLethal(int whichAttack)
    {
        numberToCheck = whichAttack - 1;
        lethal = true;
        CannotParry();
    }
    public void NotLethal()
    {
        lethal = false;
        damageGiven = false;
    }

    public void ErrorMargin()
    {
        errorMargin = true;
        parryFailed = false;
    }
    public void StartVisual()
    {
        shieldImages[0].sprite = shieldSprites[0];
        shieldImages[1].gameObject.SetActive(true);
    }
    public void CanParry(int whichAttack)
    {
        if(!parryFailed)
        {
            shieldImages[0].sprite = shieldSprites[1];
            EffectManager.instance.SpawnParrySoundEffect(transform.position, 0);
        }
        shieldImages[1].gameObject.SetActive(false);
        canParry = true;
        errorMargin = false;
        numberToCheck = whichAttack - 1;
    }
    public void CannotParry()
    {
        canParry = false;
    }
    public void ParryingDone()
    {
        numberToCheck = -1;
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
