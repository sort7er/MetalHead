using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public bool pickupsDisapear;
    public int startHealth;
    public float startPosture;
    public float postureRegenerationSpeed;
    public float timeBeforeHide;
    public HitConstraints hitConstraints;

    private RunningEnemy runningEnemy;
    private Vector3 damageDir;
    private Rigidbody rb;
    private bool isDead;
    private int health, currentBodyPart;
    private float posture, hideTimer;

    private void Start()
    {
        if(GetComponent<RunningEnemy>() != null)
        {
            runningEnemy = GetComponent<RunningEnemy>();
        }
        posture = startPosture;
        health = startHealth;
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

        if(hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
            if(hideTimer >= timeBeforeHide)
            {
                runningEnemy.Hide();
                hideTimer = 0;
            }
        }
        else
        {
            hideTimer = 0;
        }
    }

    public void TakeDamage(int damage, float stun, Rigidbody rb1, Vector3 damageDir1, int bodyPart)
    {
        if (!isDead)
        {
            rb = rb1;
            damageDir = damageDir1;

            Vector3 pointToCheck = GameManager.instance.XROrigin.transform.position - transform.position;

            if (Vector3.Dot(pointToCheck, transform.forward) < 0)
            {
                hitConstraints.Hit(true, bodyPart);
            }
            else
            {
                hitConstraints.Hit(false, bodyPart);
            }


            TakeStun(stun, bodyPart);
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            hideTimer += startHealth * 0.01f - health * 0.01f + damage * 0.1f;

            runningEnemy.EnemyAlert(GameManager.instance.XROrigin.transform.position);
        }
    }
    public void TakeStun(float stun, int bodyPart)
    {
        posture -= stun;
        currentBodyPart = bodyPart;
        if (posture < 0)
        {
            Stun();
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            runningEnemy.Die();
            runningEnemy.AddForce(rb, damageDir);
            EffectManager.instance.SpawnPickups(transform, Random.Range(3, 8), pickupsDisapear);
        }
    }
    private void Stun()
    {
        posture = startPosture;
        runningEnemy.Stun(currentBodyPart);
    }
    public int GetCurrentHealth()
    {
        return health;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
