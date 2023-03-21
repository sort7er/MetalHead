using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform[] pointsOfDamage;
    public float[] raduis;
    public int[] damage;

    private Collider playerCollider;
    private PlayerHealth playerHealth;
    private int numberToCheck;
    private bool lethal, damageGiven;

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
}
