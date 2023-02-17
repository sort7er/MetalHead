using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public float timeDead;

    private Rigidbody rb;
    private WhiteFlash whiteFlash;
    private bool isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        whiteFlash = GetComponent<WhiteFlash>();
    }
    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            whiteFlash.Flash();
            health -= damage;
            if (health < 0)
            {

                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        EffectManager.instance.SpawnPickups(transform, Random.Range(3, 8));
        Destroy(gameObject, timeDead);
    }
}
