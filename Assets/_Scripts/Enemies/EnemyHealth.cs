using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public float startPosture;
    public float postureRegenerationSpeed;

    private RunningEnemy runningEnemy;
    private Vector3 damageDir;
    private Rigidbody rb;
    private bool isDead;
    private float posture;

    private void Start()
    {
        if(GetComponent<RunningEnemy>() != null)
        {
            runningEnemy = GetComponent<RunningEnemy>();
        }
        posture = startPosture;
    }
    private void Update()
    {
        if (posture < startPosture)
        {
            posture += Time.deltaTime * postureRegenerationSpeed;
        }
        else
        {
            posture = startPosture;
        }
    }

    public void TakeDamage(int damage, float stun, Rigidbody rb1, Vector3 damageDir1)
    {
        if (!isDead)
        {
            rb = rb1;
            damageDir = damageDir1;

            health -= damage;
            posture-= stun;
            if (health < 0)
            {
                Die();
            }
            if(posture <0)
            {
                Stun();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        if(runningEnemy != null)
        {
            runningEnemy.Die();
            runningEnemy.AddForce(rb, damageDir);
            Debug.Log("Yeah");
        }
        EffectManager.instance.SpawnPickups(transform, Random.Range(3, 8));

    }
    private void Stun()
    {
        Debug.Log("Stunned");
    }
}
