using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform[] pointsOfDamage;
    public float[] raduis;
    public int[] damage;

    [HideInInspector] public bool isParrying;

    private Parry parry;
    private Collider playerCollider;
    private PlayerHealth playerHealth;
    private int numberToCheck, numberToCheckParry;
    private bool lethal, damageGiven, canParry;

    private void Start()
    {
        parry = GameManager.instance.XROrigin.GetComponent<Parry>();
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

        if (parry.isParrying && canParry && !isParrying)
        {
            parry.DoneParrying();
            EffectManager.instance.SpawnParryEffect(playerCollider.ClosestPointOnBounds(pointsOfDamage[numberToCheck].position));
            CannotParry();
            NotLethal();
            isParrying  = true;
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

    public void CanParry(int whichAttack)
    {
        numberToCheckParry = whichAttack;
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
}
