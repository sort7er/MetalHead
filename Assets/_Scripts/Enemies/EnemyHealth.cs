using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;

    private RunningEnemy runningEnemy;
    private WhiteFlash whiteFlash;
    private bool isDead;

    private void Start()
    {
        if(GetComponent<RunningEnemy>() != null)
        {
            runningEnemy = GetComponent<RunningEnemy>();
        }
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
        if(runningEnemy != null)
        {
            runningEnemy.Die();
            Debug.Log("Yeah");
        }
        EffectManager.instance.SpawnPickups(transform, Random.Range(3, 8));

    }
}
