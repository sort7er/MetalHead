using System;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public float stunMultiplier, damageMultiplier;
    public void TakeDamage(int damage, int stun, Vector3 damageDir)
    {
        enemyHealth.TakeDamage(Mathf.CeilToInt(damage * damageMultiplier), stun * stunMultiplier, GetComponent<Rigidbody>(), damageDir);
    }
}
